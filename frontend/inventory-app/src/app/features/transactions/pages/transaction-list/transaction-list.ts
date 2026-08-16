import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { finalize } from 'rxjs';
import { Transaction, TransactionFilter, TransactionType } from '../../models/transaction.model';
import { TransactionService } from '../../services/transaction.service';
import { ProductService } from '../../../products/services/product.service';
import { Product } from '../../../products/models/product.model';
import { NotificationService } from '../../../../core/services/notification.service';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-transaction-list',  
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
    MatSelectModule,
    MatDatepickerModule,
    MatCardModule,
    MatProgressBarModule,
    MatTooltipModule
  ],
  templateUrl: './transaction-list.html',
  styleUrl: './transaction-list.scss'
})
export class TransactionList implements OnInit {
  private readonly transactionService = inject(TransactionService);
  private readonly productService = inject(ProductService);
  private readonly notification = inject(NotificationService);

  readonly transactions = signal<Transaction[]>([]);
  readonly products = signal<Product[]>([]);
  readonly totalItems = signal(0);
  readonly loading = signal(false);
  readonly transactionType = TransactionType;

  readonly displayedColumns = [
    'transactionDate',
    'typeName',
    'productName',
    'productStock',
    'quantity',
    'unitPrice',
    'totalPrice',
    'detail',
    'actions'
  ];

  filter: TransactionFilter = { page: 1, pageSize: 10 };
  dateFrom: Date | null = null;
  dateTo: Date | null = null;

  ngOnInit(): void {
    this.loadProducts();
    this.load();
  }

  private loadProducts(): void {
    this.productService.getAll({ page: 1, pageSize: 100 }).subscribe((result) => {
      this.products.set(result.items);
    });
  }

  load(): void {
    this.loading.set(true);

    this.filter = {
      ...this.filter,
      dateFrom: this.dateFrom ? this.toIsoDate(this.dateFrom) : undefined,
      dateTo: this.dateTo ? this.toIsoDate(this.dateTo) : undefined
    };

    this.transactionService.getAll(this.filter)
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe((result) => {
        this.transactions.set(result.items);
        this.totalItems.set(result.totalItems);
      });
  }

  private toIsoDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
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
    this.dateFrom = null;
    this.dateTo = null;
    this.load();
  }

  deleteTransaction(transaction: Transaction): void {
    const message = `¿Eliminar esta ${transaction.typeName.toLowerCase()} de ${transaction.quantity} unidades de "${transaction.productName}"? El stock se revertirá.`;
    const isConfirmed = window.confirm(message);

    if (!isConfirmed) return;

    this.transactionService.delete(transaction.id).subscribe(() => {
      this.notification.success('Transacción eliminada y stock revertido.');
      this.load();
    });
  }
}