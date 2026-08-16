import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { NotificationService } from '../../../../core/services/notification.service';
import { Product } from '../../../products/models/product.model';
import { ProductService } from '../../../products/services/product.service';
import { CreateTransactionRequest, TransactionType, UpdateTransactionRequest } from '../../models/transaction.model';
import { TransactionService } from '../../services/transaction.service';

@Component({
  selector: 'app-transaction-form',
  imports: [
    CurrencyPipe,
    ReactiveFormsModule,
    MatButtonModule,
    MatDatepickerModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule
  ],
  templateUrl: './transaction-form.html',
  styleUrl: './transaction-form.scss'
})
export class TransactionForm implements OnInit {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private transactionService = inject(TransactionService);
  private productService = inject(ProductService);
  private notification = inject(NotificationService);

  products = signal<Product[]>([]);
  isEditMode = signal(false);
  saving = signal(false);
  currentStock = signal<number | null>(null);
  total = signal(0);
  stockError = signal<string | null>(null);

  transactionId: string | null = null;
  originalQuantity = 0;
  originalType = TransactionType.Purchase;

  form = this.fb.group({
    productId: ['', Validators.required],
    type: [TransactionType.Purchase, Validators.required],
    transactionDate: [new Date(), Validators.required],
    quantity: [1, [Validators.required, Validators.min(1)]],
    unitPrice: [0, [Validators.required, Validators.min(0.01)]],
    detail: ['']
  });

  ngOnInit(): void {
    this.loadProducts();

    this.transactionId = this.route.snapshot.paramMap.get('id');
    if (this.transactionId) {
      this.isEditMode.set(true);
      this.loadTransaction(this.transactionId);
    }

    this.form.valueChanges.subscribe(() => this.recalculate());
  }

  loadProducts(): void {
    this.productService.getAll({ page: 1, pageSize: 100 }).subscribe(result => {
      this.products.set(result.items);
    });
  }

  loadTransaction(id: string): void {
    this.transactionService.getById(id).subscribe(transaction => {
      this.originalQuantity = transaction.quantity;
      this.originalType = transaction.type;
      this.currentStock.set(transaction.productStock);

      this.form.patchValue({
        productId: transaction.productId,
        type: transaction.type,
        transactionDate: new Date(transaction.transactionDate),
        quantity: transaction.quantity,
        unitPrice: transaction.unitPrice,
        detail: transaction.detail ?? ''
      });

      this.form.get('productId')!.disable();
      this.form.get('type')!.disable();
    });
  }

  onProductChange(productId: string): void {
    const product = this.products().find(p => p.id === productId);
    this.currentStock.set(product ? product.stock : null);
    this.recalculate();
  }

  recalculate(): void {
    const value = this.form.getRawValue();
    const quantity = value.quantity ?? 0;
    const unitPrice = value.unitPrice ?? 0;

    this.total.set(quantity * unitPrice);
    this.validateStock();
  }

  validateStock(): void {
    const stock = this.currentStock();
    const value = this.form.getRawValue();
    const quantity = value.quantity ?? 0;

    if (stock === null || quantity <= 0) {
      this.stockError.set(null);
      return;
    }

    const newDelta = value.type === TransactionType.Purchase ? quantity : -quantity;
    const oldDelta = this.isEditMode()
      ? (this.originalType === TransactionType.Purchase ? this.originalQuantity : -this.originalQuantity)
      : 0;
    const adjustment = newDelta - oldDelta;

    if (stock + adjustment < 0) {
      this.stockError.set(`Stock insuficiente. Disponible: ${stock}`);
    } else {
      this.stockError.set(null);
    }
  }

  save(): void {
    if (this.form.invalid || this.stockError()) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();
    this.saving.set(true);

    if (this.isEditMode()) {
      const request: UpdateTransactionRequest = {
        transactionDate: this.toLocalIso(value.transactionDate!),
        quantity: value.quantity!,
        unitPrice: value.unitPrice!,
        detail: value.detail || null
      };

      this.transactionService.update(this.transactionId!, request).subscribe({
        next: () => this.onSuccess('Transacción actualizada correctamente'),
        error: () => this.saving.set(false)
      });
    } else {
      const request: CreateTransactionRequest = {
        transactionDate: this.toLocalIso(value.transactionDate!),
        type: value.type!,
        productId: value.productId!,
        quantity: value.quantity!,
        unitPrice: value.unitPrice!,
        detail: value.detail || null
      };

      this.transactionService.create(request).subscribe({
        next: () => this.onSuccess('Transacción creada correctamente'),
        error: () => this.saving.set(false)
      });
    }
  }

  onSuccess(message: string): void {
    this.notification.success(message);
    this.router.navigate(['/transacciones']);
  }

  toLocalIso(date: Date): string {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  const hours = String(date.getHours()).padStart(2, '0');
  const minutes = String(date.getMinutes()).padStart(2, '0');

  return `${year}-${month}-${day}T${hours}:${minutes}:00`;
}

  cancel(): void {
    this.router.navigate(['/transacciones']);
  }
}