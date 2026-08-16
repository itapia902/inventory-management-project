import { CurrencyPipe } from '@angular/common';
import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-product-detail',
  imports: [CurrencyPipe, MatButtonModule, MatCardModule, MatIconModule],
  templateUrl: './product-detail.html',
  styleUrl: './product-detail.scss'
})
export class ProductDetail implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private productService = inject(ProductService);

  product = signal<Product | null>(null);
  loading = signal(true);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.loading.set(false);
      return;
    }

    this.productService.getById(id).subscribe({
      next: product => {
        this.product.set(product);
        this.loading.set(false);
      },
      error: () => this.loading.set(false)
    });
  }

  goToEdit(): void {
    this.router.navigate(['/productos/editar', this.product()!.id]);
  }

  goBack(): void {
    this.router.navigate(['/productos']);
  }
}