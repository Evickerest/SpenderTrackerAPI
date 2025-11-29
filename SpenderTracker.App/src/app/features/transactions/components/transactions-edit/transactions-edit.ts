import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TransactionGroup } from '../../models/transaction-group';
import { TransactionMethod } from '../../models/transaction-method';
import { TransactionType } from '../../models/transaction-type';
import { APIService } from '../../../../shared/services/apiservice';
import { switchMap, of, catchError } from 'rxjs';
import { Account } from '../../models/account';
import { Router, RouterLink } from "@angular/router";
import { Transaction } from '../../models/transaction';
import { rxResource } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-transactions-edit',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './transactions-edit.html',
  styleUrl: './transactions-edit.css',
})
export class TransactionsEdit {
    private readonly apiService = inject(APIService);
    private readonly router = inject(Router);
    private readonly hasError = computed(() => 
        this.types.error() || this.groups.error() || this.methods.error() || this.accounts.error() 
    );

    transactionInput = input<Transaction>();
    isNewForm = signal<boolean>(false);
    transaction = signal<Transaction>({} as Transaction);

    types = rxResource({ 
        stream: () => this.apiService.getAll<TransactionType[]>("transaction-types", true)
    });

    groups = rxResource({ 
        stream: () => this.apiService.getAll<TransactionGroup[]>("transaction-groups", true)
    });

    methods = rxResource({ 
        stream: () => this.apiService.getAll<TransactionMethod[]>("transaction-methods", true)
    });

    accounts = rxResource({ 
        stream: () => this.apiService.getAll<Account[]>("accounts", true)
    });

    form = new FormGroup({
        id: new FormControl<number|null>(null),
        amount: new FormControl<number|null>(null, [Validators.required]),
        transactionTypeId: new FormControl<number|null>(null, [Validators.required]),
        transactionGroupId: new FormControl<number|null>(null, [Validators.required]),
        transactionMethodId: new FormControl<number|null>(null, [Validators.required]),
        accountId: new FormControl<number|null>(null, [Validators.required]),
        description: new FormControl('' , [Validators.required, Validators.maxLength(150)]),
        localTimestamp: new FormControl('', [Validators.required])
    });

    ngOnInit() {
        const t = this.transactionInput()

        if (t) {
            this.transaction.set(t);
            this.isNewForm.set(false);
            // Convert UTC time to Local time
            t.localTimestamp = this.toLocalDateTime(t.timestamp);
            this.resetForm();
        } else {
            this.transaction.set(this.getDefaultTransaction());
            this.isNewForm.set(true);
        }
    }

    getDefaultTransaction(): Transaction {
        const t = {} as Transaction;
        t.id = 0;
        t.localTimestamp = this.toLocalDateTime(new Date().toISOString());
        return t;
    }

    saveTransaction() {
        if (!this.validateTransaction()) return;

        const t: Transaction = this.form.value as Transaction;
        // Convert user's local time back to UTC
        t.timestamp = this.toUtcDateTime(t.localTimestamp);

        if (this.isNewForm()) {
            this.createTransaction(t);
        } else {
            this.updateTransaction(t);
        }
    }

    updateTransaction(t: Transaction) {
        this.apiService.update<Transaction>("transactions", t, t.id).pipe(
            switchMap(() => of(true)),
            catchError(() => of(false))
        ).subscribe(success => {
            if (success) {
                this.transaction.set(t);
                this.resetForm();
                alert("Saved transaction");
            } else {
                alert("Failed to update transaction");
            }
        });
    }

    createTransaction(t: Transaction) {
        this.apiService.insert<Transaction>("transactions", t).subscribe(newTransaction => {
            if (newTransaction) {
                this.transaction.set(newTransaction);
                this.isNewForm.set(false);
            } else {
                alert("Failed to create transaction.");
            }
        });
    }

    deleteTransaction() {
        const res = confirm("Are you sure you want to delete this transaction?");
        if (!res) return;

        const t: Transaction = this.form.value as Transaction;
        this.apiService.delete<Transaction>("transactions", t.id).subscribe(success => {
            if (success) {
                this.closeForm(true);
            } else {
                alert("Failed to delete transaction.");
            }
        });
    }

    resetForm() {
        this.form.reset(this.transaction());
    }

    hasChanges(): boolean {
        return this.form.dirty;
    }

    validateTransaction(): boolean {
        return this.form.valid
    }

    closeForm(isSaving: boolean) {
        if (this.hasChanges() && !isSaving) {
            const res = confirm("The form has changes. Do you wish to continue and lose your changes?");
            if (!res) return;
        }

        this.router.navigate([""]);
    }

    toLocalDateTime(utcDateTime: string): string {
        const date = new Date(utcDateTime);
        if (!date) return "";
        date.setMinutes(date.getMinutes() - date.getTimezoneOffset());

        return date.toISOString().slice(0, 16); // yyyy-MM-ddTHH:MM
    }

    toUtcDateTime(localDateTime: string): string {
        const date = new Date(localDateTime);
        if (!date) return "";

        return date.toISOString();
    }
}
