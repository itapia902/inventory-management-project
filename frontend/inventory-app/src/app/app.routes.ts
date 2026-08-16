import { Routes } from '@angular/router';

const loadProductList = () => import('./features/products/pages/product-list/product-list').then(m => m.ProductList);

export const routes: Routes = [
  { 
    path: '', 
    redirectTo: 'productos', 
    pathMatch: 'full' 
  },
  {
    path: 'productos',
    loadComponent: loadProductList
  },
  { 
    path: '**', 
    redirectTo: 'productos' 
  }
];