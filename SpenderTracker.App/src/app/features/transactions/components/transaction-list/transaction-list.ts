import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { APIService } from '../../../../shared/services/apiservice';
import { TransactionType } from '../../models/transaction-type';
import { TransactionGroup } from '../../models/transaction-group';
import { Transaction } from '../../models/transaction';
import { FormsModule } from '@angular/forms';
import { catchError, forkJoin, Observable, of, switchMap, tap } from 'rxjs';
import { TransactionMethod } from '../../models/transaction-method';
import { Account } from '../../models/account';

@Component({
  selector: 'app-transaction-list',
  imports: [FormsModule],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList implements OnInit {
    private readonly apiService = inject(APIService);
    private readonly cd = inject(ChangeDetectorRef);

    transactions: Transaction[];
    transactionTypes: TransactionType[];
    transactionGroups: TransactionGroup[];
    transactionMethods: TransactionMethod[];
    accounts: Account[];
    isAddingTransaction = false;

    ngOnInit() {
        this.getResources().subscribe(success => {
            if (success) {
                this.getAllTransactions();
            } else {
                alert("Failed to get resources. Try reloading the page.");
            }
        })
    }
    
    getAllTransactions() {
        this.apiService.getAll<Transaction[]>("transactions").subscribe(data => {
            // // Fix UTC time
            data.forEach(t => t.localTimestamp = new Date().toLocaleTimeString());
            console.log(data);

            this.transactions = data;
            this.cd.detectChanges();
        });
    }

    getResources(): Observable<boolean> {
        const types$ = this.apiService.getAll<TransactionType[]>("transaction-types", true).pipe(
            tap(data => this.transactionTypes = data)
        );
        const groups$ = this.apiService.getAll<TransactionGroup[]>("transaction-groups", true).pipe(
            tap(data => this.transactionGroups = data)
        );
        const methods$ = this.apiService.getAll<TransactionMethod[]>("transaction-methods", true).pipe(
            tap(data => this.transactionMethods = data)
        );
        const accounts$ = this.apiService.getAll<Account[]>("accounts", true).pipe(
            tap(data => this.accounts = data)
        );

        return forkJoin([types$, groups$, methods$, accounts$]).pipe(
            switchMap(() => of(true)),
            catchError(() => of(false))
        );
    }
    
    addTransaction() {
        const newExists = this.transactions.some(t => t.id === 0);
        if (newExists) return;

        const transaction = {} as Transaction;
        transaction.timestamp = new Date().toISOString();
        transaction.amount = 0;
        transaction.id = 0;
        transaction.isEditing = true;
        this.transactions.unshift(transaction);
    }

    getTransactionById(id: number): Transaction {
        return this.transactions.find(t => t.id === id)!;
    }

    editTransaction(transaction: Transaction) {
        transaction.isEditing = true;
    }

    saveTransaction(transaction: Transaction) {
        transaction.isEditing = false;

        if (transaction.id === 0) {
            this.insertTransaction(transaction);
        } else {
            this.updateTransaction(transaction);
        }
    }

    deleteTransaction(transaction: Transaction) {
        const res = confirm("Are you sure you want to delete this transaction?");
        if (!res) return;

        this.apiService.delete("transactions", transaction.id).subscribe(success => {
            if (success) {
                this.transactions.splice(this.transactions.indexOf(transaction), 1);
                this.cd.detectChanges();
            } else {
                alert("Failed to delete transaction.");
            }
        })
    }

    insertTransaction(transaction: Transaction) {
        this.apiService.insert<Transaction>("transactions", transaction).pipe(
            catchError(() => of(null))
        ).subscribe(data => {
            if (data) {
                transaction.id = data.id;
                this.cd.detectChanges();
            } else {
                alert("Failed to create transaction.");
            }
        });
    }

    updateTransaction(transaction: Transaction) {
        this.apiService.update<Transaction>("transactions", transaction, transaction.id).pipe(
            switchMap(() => of(true)),
            catchError(() => of(false))
        ).subscribe(success => {
            if (!success) {
                alert("Failed to update transaction.");
            }
        })
    }
}
