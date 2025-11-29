import { ResolveFn } from '@angular/router';
import { APIService } from '../../../shared/services/apiservice';
import { inject } from '@angular/core';
import { Transaction } from '../models/transaction';

export const transactionResolver: ResolveFn<Transaction | null> = (route, state) => {
    const apiService = inject(APIService);
    const strId = route.paramMap.get("id");

    if (!strId) return null;
    const id = parseInt(strId);
    if (!id) return null;

    return apiService.getById<Transaction>("transactions", id);
};
