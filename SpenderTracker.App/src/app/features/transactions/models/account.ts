export interface Account {
    id: number,
    accountName: string,
    balance: number
    isEditing: boolean
}

export const defaultAccount: Account = {
    id: 0,
    accountName: "",
    balance: 0,
    isEditing: false
}