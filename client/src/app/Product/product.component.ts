import { ProductsService } from './../Services/ProductServices/products.service';
import { Component, OnInit } from '@angular/core';
import { Products } from '../Models/Products';
import { ActivatedRoute } from '@angular/router';
import { ITag } from '../Models/Tags';
import { CategoryServicesService } from '../Services/CategoryServices/category-services.service';
import { DataSharingForSearchService } from '../Services/data-sharing-for-search.service';

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
    tagId = 0;
    sortId = 0;
    lengthProducts = 0;
    categoryName: string | any = "";


    constructor(private productsService: ProductsService, private route: ActivatedRoute,
        private categoryService: CategoryServicesService, private dataSharing: DataSharingForSearchService) {
    }

    ngOnInit(): void {
        this.categoryId = this.route.snapshot.paramMap.get('id');
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

    loadProduct() {
        this.productsService.getProductsWithCategory(+this.categoryId, this.tagId,
            this.sortId, this.searchProduct).subscribe(list => {
                this.products = list;
                this.lengthProducts = this.products.length;
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
        this.loadProduct();
    }
    onSortSelected(ev: any) {
        this.sortId = ev.target.value;
        this.loadProduct();
    }
    loadCategory() {
        if (this.categoryId != null) {
            this.categoryService.GetCategoryById(this.categoryId).subscribe(category => {
                this.categoryName = category.name;
            })
        }
    }
}