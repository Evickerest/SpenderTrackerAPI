import { Component, computed, effect, ElementRef, inject, linkedSignal, signal, viewChild } from '@angular/core';
import { APIService } from '../../../../shared/services/apiservice';
import { Transaction } from '../../models/transaction';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { rxResource, toSignal } from '@angular/core/rxjs-interop';
import { TransactionListDto } from '../../models/transaction-list-dto';
import { DatePipe, NgClass, Location, CurrencyPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { SelectBox } from "../select-box/select-box";
import { concat, delay, delayWhen, from, of, scan, startWith } from 'rxjs';
import { UrlService } from '../../../../shared/services/url-service';


@Component({
  selector: 'app-transaction-list',
  imports: [FormsModule, DatePipe, NgClass, SelectBox, CurrencyPipe, RouterLink],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList {
    private readonly apiService = inject(APIService);
    private readonly urlService = inject(UrlService);
    private readonly router = inject(Router);
    private readonly route = inject(ActivatedRoute);
    private readonly location = inject(Location);
    private readonly typeSelect = viewChild.required<SelectBox>("typeSelect");
    private readonly groupSelect = viewChild.required<SelectBox>("groupSelect");
    private readonly methodSelect = viewChild.required<SelectBox>("methodSelect");
    private readonly accountSelect = viewChild.required<SelectBox>("accountSelect");
    private readonly DELAY_LOADING_MS = 300;

    selectedTypeId = signal<number|undefined>(undefined);
    selectedGroupId = signal<number|undefined>(undefined);
    selectedMethodId = signal<number|undefined>(undefined);
    selectedAccountId = signal<number|undefined>(undefined);

    params = computed(() => ({
        typeId: this.selectedTypeId(),
        groupId: this.selectedGroupId(),
        methodId: this.selectedMethodId(),
        accountId: this.selectedAccountId()
    }));

    // Transactions Resource
    transactions = rxResource({
        params: this.params,
        stream: (p) => this.apiService.getAll<TransactionListDto[]>(`transactions${this.getQueryFilterString(p.params)}`)
    });

    isLoading = computed(() => 
        (this.transactions.isLoading() ||
        this.typeSelect().isLoading() ||
        this.groupSelect().isLoading() ||
        this.methodSelect().isLoading() ||
        this.accountSelect().isLoading()) && !this.hasError()
    );

    hasError = computed(() => 
        this.transactions.error()?.cause ||
        this.typeSelect().error()?.cause ||
        this.groupSelect().error()?.cause ||
        this.methodSelect().error()?.cause ||
        this.accountSelect().error()?.cause
    );

    showLoading = signal(false);

    eff = effect(() => {
        if (this.isLoading()) {
            this.showLoading.set(false);
            setTimeout(() => this.showLoading.set(true), this.DELAY_LOADING_MS);
        }
    })

    getQueryFilterString(params: any): string {
        const filters = [];
        if (params.typeId && params.typeId !== "null") filters.push(`typeId=${params.typeId}`);
        if (params.groupId && params.groupId !== "null") filters.push(`groupId=${params.groupId}`);
        if (params.methodId && params.methodId !== "null") filters.push(`methodId=${params.methodId}`);
        if (params.accountId && params.accountId !== "null") filters.push(`accountId=${params.accountId}`);
        let filterString = filters.join("&");
        if (filterString.length > 1) filterString = "?" + filterString;

        this.location.go(filterString);

        return filterString;
    }

    addTransaction() {
        this.urlService.url.set(this.location.path());
        this.router.navigate(["transactions", "new"]);
    }

    editTransaction(id: number) {
        this.urlService.url.set(this.location.path());
        console.log(this.urlService.url());
        this.router.navigate(["transactions", id, "edit"]);
    }

    deleteTransaction(id: number) {
        const res = confirm("Are you sure you want to delete this transaction?");
        if (!res) return;

        this.apiService.delete<Transaction>("transactions", id) .subscribe(success => {
            if (!success) {
                alert("Failed to delete transaction.");
            }
        });
    }
}
