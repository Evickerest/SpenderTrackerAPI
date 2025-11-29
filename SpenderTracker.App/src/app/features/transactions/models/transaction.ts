export interface Transaction {
    id: number,
    transactionTypeId: number,
    transactionGroupId: number,
    transactionMethodId: number,
    accountId: number,
    amount: number,
    description: string | null,
    timestamp: string,
    localTimestamp: string
}
