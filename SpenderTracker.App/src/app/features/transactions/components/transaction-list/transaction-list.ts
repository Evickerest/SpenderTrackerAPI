import { Component, inject } from '@angular/core';
import { APIService } from '../../../../shared/services/apiservice';
import { Transaction } from '../../models/transaction';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from "@angular/router";
import { rxResource } from '@angular/core/rxjs-interop';
import { TransactionListDto } from '../../models/transaction-list-dto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-transaction-list',
  imports: [FormsModule, DatePipe, RouterLink],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.css',
})
export class TransactionList {
    private readonly apiService = inject(APIService);
    private readonly router = inject(Router);

    transactions = rxResource({
        stream: () => this.apiService.getAll<TransactionListDto[]>("transactions")
    });

    addTransaction() {
        this.router.navigate(["transactions", "new"]);
    }

    editTransaction(id: number) {
        this.router.navigate(["transactions", id, "edit"]);
    }

    deleteTransaction(id: number) {
        const res = confirm("Are you sure you want to delete this transaction?");
        if (res) {
            this.apiService.delete<Transaction>("transactions", id).subscribe(success => {
                if (!success) {
                    alert("Failed to delete transaction.");
                }
            })
        }
    }
}
