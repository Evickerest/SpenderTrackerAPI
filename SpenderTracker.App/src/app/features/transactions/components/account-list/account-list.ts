import { Component, computed, inject, linkedSignal, signal } from '@angular/core';
import { APIService } from '../../../../shared/services/apiservice';
import { rxResource } from '@angular/core/rxjs-interop';
import { Account, defaultAccount } from '../../models/account';
import { CurrencyPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Field, form } from '@angular/forms/signals';
import { catchError, of, switchMap } from 'rxjs';

@Component({
  selector: 'app-account-list',
  imports: [CurrencyPipe, RouterLink, Field],
  templateUrl: './account-list.html',
  styleUrl: './account-list.css',
})
export class AccountList {
    private readonly apiService = inject(APIService);

    accountResource = rxResource({
        stream: () => this.apiService.getAll<Account[]>("accounts")
    });

    accounts = linkedSignal(() => this.accountResource.hasValue() ? this.accountResource.value() : []);
    account = signal<Account>(defaultAccount);
    accountForm = form(this.account);
    isAdding = signal<boolean>(false);

    addAccount() {
        if (this.isAdding()) return;

        this.accounts.update(a => [...a, defaultAccount]);
        this.account.set(defaultAccount);
        this.setEditing(0);
        this.isAdding.set(true);
    }

    editAccount(id: number) {
        if (this.isAdding()) {
            this.accountsRemoveById(0);
            this.isAdding.set(false);
        }

        this.setEditing(id);
    }

    saveAccount() {
        if (this.isAdding()) {
            this.insertAccountToApi(this.account());
            this.isAdding.set(false);
        } else {
            this.updateAccountToApi(this.account());
        }
    }

    insertAccountToApi(account: Account) {
        this.apiService.insert<Account>("accounts", account).pipe(
            catchError(() => of(null))
        ).subscribe(data => {
            if (data == null) {
                alert("Failed to create account.");
                return;
            }

            this.accounts.update(a => 
                a.map(e => {
                    if (e.id === 0) return data; 
                    return e;
                })
            )
            this.clearEditing()
        });
    }

    updateAccountToApi(account: Account) {
        this.apiService.update<Account>("accounts", account, account.id).pipe(
            switchMap(() => of(true)),
            catchError(() => of(false))
        ).subscribe(success => {
            if (!success) {
                alert("Failed to update account.");
                return;
            }

            this.accounts.update(list => {
                return list.map(e => {
                    if (e.id == account.id) return account;
                    return e;
                })
            });
            this.clearEditing()
        });
    }

    deleteAccount(id: number) {
        const res = confirm("Are you sure you want to delete this account?");
        if (!res) return;
        
        this.apiService.delete<Account>("accounts", id).subscribe(success => {
            console.log(success);
            if (!success) {
                alert("Failed to update account.");
                return;
            }

            this.accountsRemoveById(id);
            this.clearEditing()
        });
    }

    accountsRemoveById(id: number) {
        this.accounts.update(list => {
            const idx = list.findIndex(l => l.id === id);
            list.splice(idx, 1);
            return list;
        });
    }

    setEditing(id: number) {
        this.accounts.update(list => 
            list.map(e => {
                if (e.id === id) {
                    this.account.set(e);
                    e.isEditing = true;
                } else {
                    e.isEditing = false;
                }
                return e;
            })
        );
    }

    clearEditing() {
        this.accounts.update(list => list.map(l => ({...l, isEditing: false})));
        this.accountsRemoveById(0);
        this.isAdding.set(false);
    }
}
