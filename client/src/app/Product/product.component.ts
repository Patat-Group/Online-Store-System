import { ProductsService } from './../Services/ProductServices/products.service';
import { Component, OnInit } from '@angular/core';
import { Products } from '../Models/Products';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  products: Products[] | any;

  constructor(private productsService: ProductsService, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.loadProduct();
  }


  loadProduct() {
    var idx = this.route.snapshot.paramMap.get('id');
    console.log("immmm     ", idx)
    if(idx !=null)
    {
      this.productsService.getProductsWithCategory(+idx).subscribe(list => {
        this.products = list;
      }, error => console.log(error))
    }
  }
}
