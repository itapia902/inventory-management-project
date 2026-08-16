export interface Product {
  id: string;
  name: string;
  description: string;
  category: string;
  price: number;
  stock: number;
  imageUrl: string | null;
  createdDateTime: string;
  updatedDateTime: string | null;
}

export interface CreateProductRequest {
  name: string;
  description: string;
  category: string;
  price: number;
  stock: number;
  imageUrl: string | null;
}

export interface UpdateProductRequest {
  name: string;
  description: string;
  category: string;
  price: number;
  imageUrl: string | null;
}

export interface ProductFilter {
  name?: string;
  category?: string;
  minPrice?: number;
  maxPrice?: number;
  minStock?: number;
  sortBy?: string;
  sortDirection?: string;
  page: number;
  pageSize: number;
}