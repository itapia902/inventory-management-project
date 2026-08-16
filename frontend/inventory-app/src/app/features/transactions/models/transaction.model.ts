export enum TransactionType {
  Purchase = 1,
  Sale = 2
}

export interface Transaction {
  id: string;
  transactionDate: string;
  type: TransactionType;
  typeName: string;
  productId: string;
  productName: string;
  productStock: number;
  quantity: number;
  unitPrice: number;
  totalPrice: number;
  detail: string | null;
}

export interface CreateTransactionRequest {
  transactionDate: string;
  type: TransactionType;
  productId: string;
  quantity: number;
  unitPrice: number;
  detail: string | null;
}

export interface UpdateTransactionRequest {
  transactionDate: string;
  quantity: number;
  unitPrice: number;
  detail: string | null;
}

export interface TransactionFilter {
  productId?: string;
  type?: TransactionType;
  dateFrom?: string;
  dateTo?: string;
  sortBy?: string;
  sortDirection?: string;
  page: number;
  pageSize: number;
}