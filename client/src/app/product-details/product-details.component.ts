import {Component, OnInit, TemplateRef,Renderer2,ElementRef} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import * as $ from 'jquery';
import { IProductImages } from '../Models/ProductImages';
import { ProductImagesService } from '../Services/ProductImage/product-images.service';
import { ProductsService } from '../Services/ProductServices/products.service';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDialog} from '@angular/material/dialog';
import { ProductTag } from '../Models/ProductTag';
import {StaticFileServicesService} from "../Services/StaticFileServices/staticfile-services.service";

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  productId: string | any = "";
  product= new IProductImages ();
  productTags:any;
  currentImage:any;
  constructor(private router: Router,
              private route: ActivatedRoute,
              private productImagesService: ProductImagesService,
              private staticFileServicesService: StaticFileServicesService,
              private productsService: ProductsService,
              private dialog: MatDialog,
              private renderer: Renderer2,
              private el: ElementRef,
              ) { }
  openDialogWithTemplateRef(templateRef: TemplateRef<any>) {
    this.dialog.open(templateRef);
  }
  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('idx');
    // console.log("id " + this.productId);
    this.getProductWithImages();
    // console.log("p " + this.product);
  }

  toggleClass(event: any, className: string) {
    const hasClass = event.target.classList.contains(className);
    if(!hasClass)
      this.renderer.addClass(event.target, className);
  }
  getProductWithImages() {
    this.productImagesService.getProduct(this.productId).subscribe(data => {
      this.product = data;
      this.currentImage=this.product.imagesUrl[0];
      this.getProductTags();
    }, err => console.log(err));
  }
  getProductTags()
  {
    this.productsService.getProductTags(this.productId).subscribe(data => {
      this.productTags = data;
    }, err => console.log(err));
  }
  onTagClick(tagId:number) {
    this.router.navigateByUrl("/products/tags/"+tagId);
  }

  changeCurrentImage(img: any) {
    this.currentImage=img;

  }
}
