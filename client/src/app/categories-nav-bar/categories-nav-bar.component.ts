import { Component, OnInit, ViewChild } from '@angular/core';
import {MatMenuModule, MatMenuTrigger} from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule} from '@angular/material/button';
import {ScrollingModule} from '@angular/cdk/scrolling';
import {MatSelectModule} from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import {CategoryServicesService} from "../Services/CategoryServices/category-services.service";
@Component({
  selector: 'app-categories-nav-bar',
  templateUrl: './categories-nav-bar.component.html',
  styleUrls: ['./categories-nav-bar.component.scss']
})
export class CategoriesNavBarComponent implements OnInit {
  @ViewChild(MatMenuTrigger) trigger: MatMenuTrigger | undefined;
  isCategoryLoading: boolean | undefined;
  categoryItems:any;

  getCategories() {
    this.isCategoryLoading = true;
    this.categoryService.GetCategories().subscribe(data => {
      this.categoryItems=data;
      console.log(data);
      this.isCategoryLoading = false;
    }, error => {
      this.isCategoryLoading = false;
      console.log(error);
    });
  }

  constructor( private categoryService: CategoryServicesService,) { }
  ngOnInit(): void {
    this.getCategories();
  }

}
