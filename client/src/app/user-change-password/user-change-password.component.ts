import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-user-change-password',
  templateUrl: './user-change-password.component.html',
  styleUrls: ['./user-change-password.component.scss']
})
export class UserChangePasswordComponent implements OnInit {
  showPassword=false;
  emptyOldPassword: any;
  showOldPassword =false;
  currentOldPassword="";

  constructor() { }

  ngOnInit(): void {
  }

  toggleShow() {
    this.showPassword=!this.showPassword;
  }

  toggleShowOldPassword() {
    this.showOldPassword=!this.showOldPassword;
  }

  onOldPasswordChange(event: any) {
    this.currentOldPassword=event.target.value;
  }
}
