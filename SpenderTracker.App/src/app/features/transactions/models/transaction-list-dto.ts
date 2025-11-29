export interface TransactionListDto {
    id: number,
    type: number,
    group: number,
    method: number,
    account: number,
    amount: number,
    description: string | null,
    timestamp: string,
}
