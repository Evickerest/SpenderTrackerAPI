import { maxLength, required, schema } from "@angular/forms/signals"

export interface Transaction {
    id: number,
    transactionTypeId: number,
    transactionGroupId: number,
    transactionMethodId: number,
    accountId: number,
    amount: number,
    description: string,
    timestamp: string,
    localTimestamp: string
}

export const defaultTransaction: Transaction = {
    id: NaN,
    transactionTypeId: NaN,
    transactionGroupId: NaN,
    transactionMethodId: NaN,
    accountId: NaN,
    amount: NaN,
    description: "",
    timestamp: "",
    localTimestamp: ""
}

export const transactionSchema = schema<Transaction>(root => {
    required(root.amount, { message: "Amount is required." });
    required(root.transactionTypeId, { message: "Type is required." });
    required(root.transactionGroupId, { message: "Group is required." });
    required(root.transactionMethodId, { message: "Method is required." });
    required(root.accountId, { message: "Account is required." });
    required(root.localTimestamp, { message: "Timestamp is required." });
    required(root.description, { message: "Description is required." });

    maxLength(root.description, 150, { message: "Description cannot be more than 150 characters." });
});
