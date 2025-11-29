import { Routes } from '@angular/router';
import { Transactions } from './features/transactions/transactions';
import { TransactionsEdit } from './features/transactions/components/transactions-edit/transactions-edit';
import { transactionResolver } from './features/transactions/resolvers/transaction-resolver';
import { ErrorPage } from './features/error/error';

export const routes: Routes = [
    {
        path: "",
        component: Transactions
    },
    {
        path: "error",
        component: ErrorPage 
    },
    {
        path: "transactions/:id/edit",
        component: TransactionsEdit,
        resolve: {
            transaction: transactionResolver
        }
    },
    {
        path: "transactions/new",
        component: TransactionsEdit
    },
];
