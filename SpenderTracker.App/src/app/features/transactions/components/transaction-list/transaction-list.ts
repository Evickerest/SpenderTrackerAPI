import { Component, inject, OnInit } from '@angular/core';
import { APIService } from '../../../../shared/services/apiservice';
import { AsyncPipe, DatePipe, NgClass } from '@angular/common';
import { TransactionType } from '../../models/transaction-type';
import { TransactionGroup } from '../../models/transaction-group';
import { TransactionListDto } from '../../models/transaction-list-dto';
import { Transaction } from '../../models/transaction';
import { MethodListDto } from '../../models/method-list-dto';
import { FormsModule, NgModel } from '@angular/forms';

@Component({
  selector: 'app-transaction-list',
  imports: [AsyncPipe, DatePipe, NgClass, FormsModule],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList implements OnInit {
    private readonly apiService = inject(APIService);

    transactionTypes$ = this.apiService.getAll<TransactionType[]>("transaction-types", true);
    transactionGroups$ = this.apiService.getAll<TransactionGroup[]>("transaction-groups", true);
    accountMethods$ = this.apiService.getAll<MethodListDto[]>("accounts/methods", true);

    transactions: TransactionListDto[];
    newTransaction: Transaction;
    isAddingTransaction = false;

    ngOnInit() {
        this.apiService.getAll<TransactionListDto[]>("transactions").subscribe(data => {
            this.transactions = data;
        });
     }

    addTransaction() {
        this.newTransaction = {} as Transaction;
        this.newTransaction.timestamp = new Date().toISOString();
        this.newTransaction.amount = 0;
        this.isAddingTransaction = true;
    }

    handleMethodChange(e: any) {
        const selectBox = document.querySelector("#transaction-method-select") as HTMLSelectElement;
        const options = selectBox.options[selectBox.selectedIndex] as HTMLOptionElement;
        const accountLabel = document.querySelector("#account-name") as HTMLSpanElement;
        accountLabel.textContent = options.dataset["account"]!;
    }

    saveTransaction(id: number) {
        this.apiService.insert<Transaction>("transactions", this.newTransaction).subscribe(data => {
        })
    }
}
