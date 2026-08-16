import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { NotificationService } from '../../../../core/services/notification.service';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-form',
  imports: [
    CommonModule, ReactiveFormsModule, RouterLink,
    MatButtonModule, MatCardModule, MatFormFieldModule,
    MatIconModule, MatInputModule, MatProgressBarModule
  ],
  templateUrl: './product-form.html',
  styleUrl: './product-form.scss'
})
export class ProductForm implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly productService = inject(ProductService);
  private readonly notification = inject(NotificationService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal(false);
  readonly saving = signal(false);
  readonly isEditMode = signal(false);

  private productId: string | null = null;

  readonly form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    description: ['', [Validators.required, Validators.maxLength(500)]],
    category: ['', [Validators.required, Validators.maxLength(100)]],
    price: [0, [Validators.required, Validators.min(0)]],
    stock: [0, [Validators.required, Validators.min(0)]],
    imageUrl: ['', [Validators.pattern(/^https?:\/\/.+/)]]
  });

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id');

    if (this.productId) {
      this.isEditMode.set(true);
      this.form.controls.stock.disable();
      this.loadProduct(this.productId);
    }
  }

  private loadProduct(id: string): void {
    this.loading.set(true);

    this.productService.getById(id).subscribe({
      next: product => {
        this.form.patchValue({
          name: product.name,
          description: product.description,
          category: product.category,
          price: product.price,
          stock: product.stock,
          imageUrl: product.imageUrl ?? ''
        });
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
        this.router.navigate(['/productos']);
      }
    });
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const value = this.form.getRawValue();

    if (this.isEditMode() && this.productId) {
      this.productService.update(this.productId, {
        name: value.name,
        description: value.description,
        category: value.category,
        price: value.price,
        imageUrl: value.imageUrl || null
      }).subscribe({
        next: () => {
          this.notification.success('Producto actualizado correctamente.');
          this.router.navigate(['/productos']);
        },
        error: () => this.saving.set(false)
      });
      return;
    }

    this.productService.create({
      name: value.name,
      description: value.description,
      category: value.category,
      price: value.price,
      stock: value.stock,
      imageUrl: value.imageUrl || null
    }).subscribe({
      next: () => {
        this.notification.success('Producto creado correctamente.');
        this.router.navigate(['/productos']);
      },
      error: () => this.saving.set(false)
    });
  }
} 