import { TransactionMethod } from "./transaction-method";

export interface MethodListDto {
    accountName: string,
    methods: TransactionMethod[]
}
