import { Component, OnInit } from '@angular/core';
import { IProduct } from "../interfaces/product.interface";
import { ProductService } from './product.service';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  errorMessage:any   
  private _listFilter: string = '';
  
  filteredProducts: IProduct[]= [];
  productlist: IProduct[] = [];

  constructor(private productService: ProductService) {}
  
  ngOnInit(): void {
    debugger
    this.productService.getProducts().subscribe({
      next: products => {
        this.productlist = products;
        debugger            
      },
      error: err => this.errorMessage = err
    });              
  }

}
