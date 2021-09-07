import { Component, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { Category } from '../Models/Category';
import { ImageForAdd } from '../Models/ImageForAdd';
import { ProductsForAdd } from '../Models/ProductForAdd';
import { CategoryServicesService } from '../Services/CategoryServices/category-services.service';
import { ProductImagesService } from '../Services/ProductImage/product-images.service';
import { ProductsService } from '../Services/ProductServices/products.service';
import { StaticFileServicesService } from '../Services/StaticFileServices/staticfile-services.service';
import { UsersService } from '../Services/UserServices/user-services.service';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.scss']
})
export class AddProductComponent implements OnInit {

  AddProductForm = this.formBuilder.group({
    InputProductName: '',
    InputPrice: 0.0,
    InputShortDescription: '',
    InputLongDescription: ''
  });

  categories: Category[] | any = [];
  categoryId: number = 1;
  fileImage: any = [];
  imageToShow: any;
  isImageLoading: boolean | any;
  constructor(private usersService: UsersService,
    private categoryService: CategoryServicesService,
    private productService: ProductsService,
    private productImageService: ProductImagesService,
    private staticFileServicesService: StaticFileServicesService,
    private formBuilder: FormBuilder,
    private router: Router) { }

  ngOnInit(): void {
    this.getMainImage();
    this.categoryService.GetCategories().subscribe(list => {
      this.categories = list;
    }, err => console.log(err));
  }
  onSubmit() {
    let productForAdd = new ProductsForAdd();
    productForAdd.name = this.AddProductForm.value.InputProductName;
    productForAdd.shortDescription = this.AddProductForm.value.InputShortDescription;
    productForAdd.longDescription = this.AddProductForm.value.InputLongDescription;
    productForAdd.price = this.AddProductForm.value.InputPrice;
    productForAdd.categoryId = this.categoryId;
    // console.log("product name: " + productForAdd.name);
    // console.log("product sdec: " + productForAdd.shortDescription);
    // console.log("product ldec: " + productForAdd.longDescription);
    // console.log("product price: " + productForAdd.price);
    // console.log("product categoryId: " + productForAdd.categoryId);
    let idx = 0;
    (this.productService.AddProduct(productForAdd)).subscribe((id: number) => {
      idx = id;
      for (let i = 0; i < this.fileImage.length; i++) {
        let imageForAdd = new ImageForAdd();
        imageForAdd.file = this.fileImage[i];
        (this.productImageService.addImage(idx, imageForAdd.file)).subscribe(() => {
        }, err => console.log(err));
      }
    }, err => console.log(err))
    this.router.navigateByUrl("/profile")
  }
  onCategorySelected(ev: any) {
    this.categoryId = ev.target.value;
  }
  SelectedFiles(ev: any) {
    const filesSelected = ev.target.files;
    let isImage = true;

    for (let i = 0; i < filesSelected.length; i++) {
      this.fileImage[i] = (filesSelected[i]);
    }
  }

  async update() {
    console.log("update..");
  }
  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.imageToShow = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }

  getMainImage() {
    this.isImageLoading = true;
    this.staticFileServicesService.getLoginImage().subscribe(data => {
      this.createImageFromBlob(data);
      this.isImageLoading = false;
    }, error => {
      this.isImageLoading = false;
    });
  }

}
