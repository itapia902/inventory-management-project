import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'productos', pathMatch: 'full' },
  {
    path: 'productos',
    loadComponent: () =>
      import('./features/products/pages/product-list/product-list').then(m => m.ProductList)
  },
  {
    path: 'productos/nuevo',
    loadComponent: () =>
      import('./features/products/pages/product-form/product-form').then(m => m.ProductForm)
  },
  {
    path: 'productos/editar/:id',
    loadComponent: () =>
      import('./features/products/pages/product-form/product-form').then(m => m.ProductForm)
  },

  { 
  path: 'transacciones',
  loadComponent: () =>
    import('./features/transactions/pages/transaction-list/transaction-list').then(m => m.TransactionList)
    },

    {
    path: 'transacciones/nueva',
    loadComponent: () => import('./features/transactions/pages/transaction-form/transaction-form').then(m => m.TransactionForm)
    },
    {
    path: 'transacciones/editar/:id',
    loadComponent: () => import('./features/transactions/pages/transaction-form/transaction-form').then(m => m.TransactionForm)
    },

    {
        path: 'productos/detalle/:id',
        loadComponent: () => import('./features/products/pages/product-detail/product-detail').then(m => m.ProductDetail)
    },

    { path: '**', redirectTo: 'productos' }
  
];