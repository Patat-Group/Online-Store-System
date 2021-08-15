import { Component, OnInit } from '@angular/core';
import {NgbDropdownConfig} from '@ng-bootstrap/ng-bootstrap';
import {UsersService} from "../Services/UserServices/user-services.service";
import {Router} from '@angular/router';
@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss'],
  providers: [NgbDropdownConfig]
})
export class NavBarComponent implements OnInit {
  collapsed = true;
  isUserLoggedIn=false;
  username:string | any;
  constructor(    private usersService: UsersService,
                  private router: Router,
  ) {
      usersService.getLoggedInName
        .subscribe(user => this.changeUsername(user));
  }
  private changeUsername(name: string): void {
    this.username = name;
    if(name!=null)
      this.isUserLoggedIn=true;
    else
      this.isUserLoggedIn=false;
  }
  ngOnInit(): void {
    if(this.usersService.isLoggedIn())
    {
      this.isUserLoggedIn=true;
      this.username=localStorage.getItem('username');
    }
  }
  ngOnChanges(){
    if(this.usersService.isLoggedIn())
    {
      this.isUserLoggedIn=true;
      this.username=localStorage.getItem('username');
    }
  }
  logoutUser()
  {
    this.usersService.logout();
    this.isUserLoggedIn=false;
  }
  reloadComponent() {
   this.ngOnChanges();
  }
 toggleCollapsed(): void {
    this.collapsed = !this.collapsed;
  }

}
export class NgbdDropdownConfig {
  constructor(config: NgbDropdownConfig) {
    // customize default values of dropdowns used by this component tree
    config.placement = 'top-left';
    config.autoClose = false;
  }
}
