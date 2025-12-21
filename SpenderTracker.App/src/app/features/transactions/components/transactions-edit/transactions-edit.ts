import { Component, computed, effect, inject, signal } from '@angular/core';
import { TransactionGroup } from '../../models/transaction-group';
import { TransactionMethod } from '../../models/transaction-method';
import { TransactionType } from '../../models/transaction-type';
import { APIService } from '../../../../shared/services/apiservice';
import { switchMap, of, catchError, map, tap, EMPTY, finalize, forkJoin } from 'rxjs';
import { Account } from '../../models/account';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { defaultTransaction, Transaction, transactionSchema } from '../../models/transaction';
import { rxResource, takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { UrlService } from '../../../../shared/services/url-service';
import { Field, form } from '@angular/forms/signals';
import { ValidationError } from "../../../../shared/components/validation-error/validation-error";
import { SelectBox } from '../../../../shared/components/select-box/select-box';

@Component({
  selector: 'app-transactions-edit',
  imports: [Field, RouterLink, ValidationError, SelectBox],
  templateUrl: './transactions-edit.html',
  styleUrl: './transactions-edit.css',
})
export class TransactionsEdit {
    private readonly apiService = inject(APIService);
    private readonly urlService = inject(UrlService);
    private readonly router = inject(Router);
    private readonly route = inject(ActivatedRoute);

    transaction = signal<Transaction>(defaultTransaction);
    form = form(this.transaction, transactionSchema);

    // Form state signals
    isNewForm = signal<boolean>(true);

    constructor() {
        // Fetch either transaction from either or create a new transaction
        // based on the id in the url
        this.route.paramMap.pipe(
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
                return EMPTY;
            }))
        .subscribe(t => {
            this.transaction.set(t);
            // this.rebindForm();
        });
    }

    getDefaultTransaction(): Transaction {
        const t = {id: 0} as Transaction;
        return t;
    }

    saveTransaction() {
        if (!this.validateTransaction()) return;

        const t: Transaction = this.form().value();
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

        const t: Transaction = this.form().value();
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
            this.form().reset(this.transaction());
        }
    }

    hasChanges(): boolean {
        return this.form().dirty();
    }

    validateTransaction(): boolean {
        return this.form().valid();
    }

    closeForm(isSaving: boolean) {
        if (this.hasChanges() && !isSaving) {
            const res = confirm("The form has changes. Do you wish to continue and lose your changes?");
            if (!res) return;
        }

        const redirectUrl = this.urlService.url() ?? "";
        console.log(redirectUrl);
        this.router.navigateByUrl(redirectUrl);
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
