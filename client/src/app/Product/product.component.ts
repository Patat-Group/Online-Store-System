import { ProductsService } from './../Services/ProductServices/products.service';
import { Component, OnInit } from '@angular/core';
import { Products } from '../Models/Products';
import { ActivatedRoute } from '@angular/router';
import { ITag } from '../Models/Tags';
import { CategoryServicesService } from '../Services/CategoryServices/category-services.service';
import { DataSharingForSearchService } from '../Services/data-sharing-for-search.service';
import { UsersService } from "../Services/UserServices/user-services.service";
import { Observable } from "rxjs";
import { ProductsParam } from '../Models/ProductParams';
import { PaginatedResult, Pagination } from '../Models/Pagination';
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss']
})
export class ProductComponent implements OnInit {

  products: Products[] | any = [];
  searchProduct: string | any = "";
  categoryId: string | any = "";
  productsTags: ITag[] | any = [];
  productsUsersRatesAvg: any = [];
  productsUsersRatesCount: any = [];
  tagId: any = 0;
  sortId = 0;
  lengthProducts = 0;
  categoryName: string | any = "";
  productsParams: ProductsParam = new ProductsParam();
  pagination: Pagination | any = {};
  currentPage = 1;
  imageMain: string | any;



  constructor(private productsService: ProductsService, private route: ActivatedRoute, private usersService: UsersService,
    private categoryService: CategoryServicesService, private dataSharing: DataSharingForSearchService) {


  }

  ngOnInit(): void {
    this.categoryId = this.route.snapshot.paramMap.get('id');
    this.tagId = this.route.snapshot.paramMap.get('tagId');
    if (this.tagId != null) {
      console.log(this.tagId);
      this.productsService.getTagName(this.tagId).subscribe((data: any) => {
        console.log(data);
        this.categoryName = data.name;
      }, error => console.log(error))
    }
    else
      this.tagId = 0;
    // this.dataSharing.sharingDate.subscribe(val => {
    //     this.searchProduct = val;
    // })
    this.searchProduct = localStorage.getItem("search");
    this.categoryName = localStorage.getItem("search");
    console.log(this.categoryName);
    this.loadProduct();
    this.loadTags();
    this.loadCategory();
    localStorage.removeItem('search');
  }

  loadRatings() {
    for (var i in this.products) {
      this.getUserRate(this.products[i].username, i);
    }
  }

  loadProduct() {
    this.productsService.getProductsWithCategory(+this.categoryId, this.tagId,
      this.sortId, this.searchProduct, this.currentPage, this.pagination?.itemsPerPage).subscribe((list: PaginatedResult<Products[]>) => {
        this.products = list.result;
        //  console.log("products   " + this.products.imageUrl);
        // for(let i =0 ; i <this.products.length ; i ++){
        //   console.log(this.products.imagesUrl[0]);
        //   console.log(this.products.imageUrl);

        // }
        this.lengthProducts = this.products.length;
        this.pagination = list.pagination;
        this.currentPage = list.pagination.currentPage;
        // console.log(this.pagination);
        this.loadRatings();
      }, error => console.log(error))
  }
  loadTags() {
    if (this.categoryId != null && this.categoryId != undefined) {
      this.productsService.getTags(this.categoryId).subscribe(list => {
        this.productsTags = [{ id: 0, name: "All" }, ...list];
      })
    }
    else {
      this.productsTags.push({ id: 0, name: "All" });
    }
  }

  onTagSelected(ev: any) {
    this.tagId = ev.target.value;
    this.currentPage = 1;
    this.loadProduct();
  }
  onSortSelected(ev: any) {
    this.sortId = ev.target.value;
    this.currentPage = 1;
    this.loadProduct();
  }
  loadCategory() {
    if (this.categoryId != null) {
      this.categoryService.GetCategoryById(this.categoryId).subscribe(category => {
        this.categoryName = category.name;
      })
    }
  }
  getUserRate(username: string, id: any) {
    this.usersService.getUserRateWithUsername(username).subscribe((data: any) => {
      const userRate = data;
      const totalRateCount = userRate.oneStarCount
        + userRate.twoStarCount
        + userRate.threeStarCount
        + userRate.fourStarCount
        + userRate.fiveStarCount;
      let ratioOfTotalRate =
        userRate.oneStarCount
        + 2 * userRate.twoStarCount
        + 3 * userRate.threeStarCount
        + 4 * userRate.fourStarCount
        + 5 * userRate.fiveStarCount;
      if (totalRateCount == 0) {
        ratioOfTotalRate = 0;
      }
      else {
        ratioOfTotalRate = this.roundFloat(ratioOfTotalRate / totalRateCount);
      }
      this.productsUsersRatesAvg[id] = ratioOfTotalRate;
      this.productsUsersRatesCount[id] = totalRateCount;
    }, (error: any) => {
      this.productsUsersRatesAvg[id] = 0;
      this.productsUsersRatesCount[id] = 0;
      console.log(error)
    });
  }
  roundFloat(number: any) {
    return Math.round((number + Number.EPSILON) * 10) / 10;
  }
  pageChanged(event: any): void {
    this.currentPage = event.page;
    console.log("sd  " + event.page);
    this.loadProduct();
  }
}
