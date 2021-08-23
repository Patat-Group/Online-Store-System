import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as $ from 'jquery';
import { IProductImages } from '../Models/ProductImages';
import { ProductImagesService } from '../Services/ProductImage/product-images.service';


@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  productId: string | any = "";
  product: IProductImages |any =null;

  constructor(private route: ActivatedRoute, private productImagesService: ProductImagesService) { }

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('idx');
    console.log("id " + this.productId);
    this.getProductWithImages();
    console.log("p " + this.product);
    $('img').click(function () {
      $('img').removeClass('selected')
      $(this).addClass('selected');
    });
  }

  getProductWithImages() {
    this.productImagesService.getProduct(this.productId).subscribe(data => {
      this.product = data;
    }, err => console.log(err));
  }

}
