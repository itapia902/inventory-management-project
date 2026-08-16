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
  { path: '**', redirectTo: 'productos' }
];