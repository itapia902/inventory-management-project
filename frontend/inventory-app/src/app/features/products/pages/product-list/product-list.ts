import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { finalize } from 'rxjs';
import { Product, ProductFilter } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { NotificationService } from '../../../../core/services/notification.service';

@Component({
  selector: 'app-product-list',
  imports: [
    CommonModule,
    FormsModule,
    RouterLink,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatProgressBarModule
  ],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss'
})
export class ProductList implements OnInit {
  private readonly productService = inject(ProductService);
  private readonly notification = inject(NotificationService);

  readonly products = signal<Product[]>([]);
  readonly totalItems = signal(0);
  readonly loading = signal(false);
  displayedColumns = ['name', 'category', 'price', 'stock', 'imageUrl', 'actions'];

  filter: ProductFilter = { page: 1, pageSize: 10 };

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading.set(true);

    this.productService.getAll(this.filter)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (result) => {
          this.products.set(result.items);
          this.totalItems.set(result.totalItems);
        }
      });
  }

  onPageChange(event: PageEvent): void {
    this.filter = {
      ...this.filter,
      page: event.pageIndex + 1,
      pageSize: event.pageSize
    };
    this.load();
  }

  applyFilters(): void {
    this.filter = { ...this.filter, page: 1 };
    this.load();
  }

  clearFilters(): void {
    this.filter = { page: 1, pageSize: this.filter.pageSize };
    this.load();
  }

  deleteProduct(product: Product): void {
    const isConfirmed = window.confirm(`¿Eliminar el producto "${product.name}"?`);
    
    if (!isConfirmed) return;

    this.productService.delete(product.id).subscribe(() => {
      this.notification.success('Producto eliminado correctamente.');
      this.load();
    });
  }
}