import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TransactionGroup } from '../../models/transaction-group';
import { TransactionMethod } from '../../models/transaction-method';
import { TransactionType } from '../../models/transaction-type';
import { APIService } from '../../../../shared/services/apiservice';
import { switchMap, of, catchError, map, tap, EMPTY, finalize, forkJoin } from 'rxjs';
import { Account } from '../../models/account';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { Transaction } from '../../models/transaction';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-transactions-edit',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './transactions-edit.html',
  styleUrl: './transactions-edit.css',
})
export class TransactionsEdit {
    private readonly apiService = inject(APIService);
    private readonly router = inject(Router);
    private readonly route = inject(ActivatedRoute);

    // Form resources signals
    types = signal<TransactionType[]>([]);
    groups = signal<TransactionGroup[]>([]);
    methods = signal<TransactionMethod[]>([]);
    accounts = signal<Account[]>([]);
    transaction = signal<Transaction|undefined>(undefined);

    // Form state signals
    isNewForm = signal<boolean>(true);
    transactionLoading = signal<boolean>(true);
    resourcesLoading = signal<boolean>(true);
    errorMessage = signal<string|undefined>(undefined);
    showLoading = signal<boolean>(false);
    formLoading = computed(() => (this.transactionLoading() || this.resourcesLoading()) && !this.errorMessage());

    // Form contract
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

    constructor() {
        // Fetch either transaction from either or create a new transaction
        // based on the id in the url
        this.route.paramMap.pipe(
            tap(() => {
              this.transactionLoading.set(true);
              setTimeout(() => this.showLoading.set(true), 300) // Wait 300 ms before showing loading icon
            }),
            map(params => params.get("id")),
            switchMap(paramId => {
                const id = parseInt(paramId ?? "");
                if (id) {
                    this.isNewForm.set(false);
                    return this.apiService.getById<Transaction>("transactions", id).pipe(
                        takeUntilDestroyed()
                    )
                } else {
                    this.isNewForm.set(true);
                    return of(this.getDefaultTransaction());
                }
            }),
            catchError((err: HttpErrorResponse) => {
                this.errorMessage.set(err.error);
                return EMPTY;
            }))
        .subscribe(t => {
            this.showLoading.set(false);
            this.transactionLoading.set(false);
            this.transaction.set(t);
            this.rebindForm();
        });

        // Fetch all resources
        forkJoin({
            types: this.apiService.getAll<TransactionType[]>("transaction-types", true).pipe(takeUntilDestroyed()),
            groups: this.apiService.getAll<TransactionGroup[]>("transaction-groups", true).pipe(takeUntilDestroyed()),
            methods: this.apiService.getAll<TransactionMethod[]>("transaction-methods", true).pipe(takeUntilDestroyed()),
            accounts: this.apiService.getAll<Account[]>("accounts", true).pipe(takeUntilDestroyed())
        }).pipe(
            tap(() => this.resourcesLoading.set(true)),
            catchError((err: HttpErrorResponse) => {
                console.log(err);
                this.transactionLoading.set(false);
                this.errorMessage.set(err.error);
                return EMPTY;
            }),
            finalize(() => this.resourcesLoading.set(false))
        ).subscribe(resources => {
            this.types.set(resources.types);
            this.groups.set(resources.groups);
            this.methods.set(resources.methods);
            this.accounts.set(resources.accounts);
        });
    }

    getDefaultTransaction(): Transaction {
        const t = {id: 0} as Transaction;
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
                this.rebindForm();
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
                this.rebindForm();
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

    rebindForm() {
        const t = this.transaction();
        if (t) {
            // Convert UTC time to local on the transaction
            t.localTimestamp = this.toLocalDateTime(t.timestamp || new Date().toISOString());
            this.form.reset(this.transaction());
        }
    }

    hasChanges(): boolean {
        return this.form.dirty;
    }

    validateTransaction(): boolean {
        this.form.updateValueAndValidity();
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
