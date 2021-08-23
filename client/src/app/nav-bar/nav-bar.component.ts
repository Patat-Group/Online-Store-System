import { Component, OnInit } from '@angular/core';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { UsersService } from "../Services/UserServices/user-services.service";
import { Router } from '@angular/router';
import { ProductsService } from '../Services/ProductServices/products.service';
import { DataSharingForSearchService } from '../Services/data-sharing-for-search.service';
@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  providers: [NgbDropdownConfig]
})
export class NavBarComponent implements OnInit {
  collapsed = true;
  isUserLoggedIn = false;
  username: string | any;

  constructor(private usersService: UsersService,
    private router: Router,
    private productService: ProductsService,
    private dataSharingForSearch: DataSharingForSearchService
  ) {
    usersService.getLoggedInName
      .subscribe(user => this.changeUsername(user));

  }
  private changeUsername(name: string): void {
    this.username = name;
    if (name != null)
      this.isUserLoggedIn = true;
    else
      this.isUserLoggedIn = false;
  }
  ngOnInit(): void {
    if (this.usersService.isLoggedIn()) {
      this.isUserLoggedIn = true;
      this.username = localStorage.getItem('username');
    }
  }
  ngOnChanges() {
    if (this.usersService.isLoggedIn()) {
      this.isUserLoggedIn = true;
      this.username = localStorage.getItem('username');
    }
  }
  logoutUser() {
    this.usersService.logout();
    this.isUserLoggedIn = false;
    this.router.navigateByUrl("/");
  }
  reloadComponent() {
    this.ngOnChanges();
  }
  toggleCollapsed(): void {
    this.collapsed = !this.collapsed;
  }
  public search(ev: any) {
    var str = ev.target.value;
    DataSharingForSearchService.data = str.toString();
    localStorage.setItem('search', str);
    DataSharingForSearchService.sharedData = DataSharingForSearchService.data;
    this.router.navigate(['products']);
  }

  // get data(): any {
  //   return this.dataSharingForSearch.sharedData;
  // }
  // set data(value: any) {
  //   this.dataSharingForSearch.sharedData = value;
  // }

}
export class NgbdDropdownConfig {
  constructor(config: NgbDropdownConfig) {
    // customize default values of dropdowns used by this component tree
    config.placement = 'top-left';
    config.autoClose = false;
  }
}
