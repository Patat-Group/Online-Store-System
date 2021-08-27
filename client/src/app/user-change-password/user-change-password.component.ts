import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import {UsersService} from "../Services/UserServices/user-services.service";
import {StaticFileServicesService} from "../Services/StaticFileServices/staticfile-services.service";
import {FormBuilder} from "@angular/forms";
import {Router} from "@angular/router";
import { MatDialogModule } from '@angular/material/dialog';
import { MatDialog} from '@angular/material/dialog';

@Component({
  selector: 'app-user-change-password',
  templateUrl: './user-change-password.component.html',
  styleUrls: ['./user-change-password.component.scss']
})
export class UserChangePasswordComponent implements OnInit {
  @ViewChild('updatePasswordMessage', {static: true})
  updatePasswordMessage!: TemplateRef<any>;
  showPassword=false;
  emptyOldPassword: any;
  showOldPassword =false;
  currentOldPassword="";
  currentNewPassword2="";
  currentNewPassword="";
  emptyPassword=false;
  isPasswordMinimumLengthGood=false;
  isPasswordAtLeastOneDigit=false;
  isPasswordAtLeastOneUppercase=false;
  isPasswordAtLeastOneLowercase=false;
  emptyNewPasswordAfterModify=true;

  showNewPassword=false;
  emptyNewPassword=false;
  emptyNewPassword2: any;
  emptyNewPassword2AfterModify=true;
  isNewPassword2Matches: any;
  isImageLoading=false;
  imageToShow:any;
  currnetUsername:any;
  errorInRegister: boolean=false;
  errorMessage:string="";
  updateForm = this.formBuilder.group({
    InputOldPassword: '',
    InputNewPassword: '',
    InputNewPassword2: '',

  });
  constructor(
    private usersService: UsersService,
    private staticFileServicesService: StaticFileServicesService,
    private formBuilder: FormBuilder,
    private router: Router,
    private dialog: MatDialog,
  ) {}

  ngOnInit(): void {
    if (this.usersService.isLoggedOut())
      this.router.navigateByUrl("/login");
    else
    {
      this.currnetUsername=localStorage.getItem("username");
      this.getupdateImage();
    }
  }

  toggleShow() {
    this.showPassword=!this.showPassword;
  }

  toggleShowOldPassword() {
    this.showOldPassword=!this.showOldPassword;
  }

  onOldPasswordChange(event: any) {
    if(event.target.value!="")
    {
      this.emptyOldPassword=false;
    this.currentOldPassword=event.target.value;
    }
    else
    {
      this.emptyOldPassword=true;
    }

  }

  onNewPasswordChange(event: any)
  {
    if(event.target.value!="") {
      this.emptyNewPasswordAfterModify = false;
      this.emptyPassword=false;
      this.currentNewPassword=event.target.value;
      this.isPasswordAtLeastOneDigit=this.checkIfStringHaveAtLeastOneDigit(this.currentNewPassword);
      this.isPasswordAtLeastOneLowercase=this.checkIfStringHaveAtLeastOneLowercase(this.currentNewPassword);
      this.isPasswordAtLeastOneUppercase=this.checkIfStringHaveAtLeastOneUppercase(this.currentNewPassword);
      this.isPasswordMinimumLengthGood=this.checkIfStringHaveMinimumLengthGood(this.currentNewPassword,6);
    }
    else {
      this.emptyNewPasswordAfterModify = true;
      this.isPasswordAtLeastOneDigit=false;
      this.isPasswordAtLeastOneLowercase=false;
      this.isPasswordAtLeastOneUppercase=false;
      this.isPasswordMinimumLengthGood=false;
      this.emptyPassword=true;
      this.currentNewPassword="";
    }
    if(this.currentNewPassword==this.currentNewPassword2 && this.currentNewPassword2!="")
      this.isNewPassword2Matches=true;
    else
      this.isNewPassword2Matches=false;

  }
  checkIfStringHaveAtLeastOneDigit(s:string)
  {
    const regexToCheck=/\d/;
    return regexToCheck.test(s);
  }
  checkIfStringHaveAtLeastOneLowercase(s:string)
  {
    const regexToCheck=/[a-z]/;
    return regexToCheck.test(s);
  }
  checkIfStringHaveAtLeastOneUppercase(s:string)
  {
    const regexToCheck=/[A-Z]/;
    return regexToCheck.test(s);
  }
  checkIfStringHaveMinimumLengthGood(s:string ,n:any)
  {
    return s.length>=n;
  }

  toggleShowNewPassword() {
    this.showNewPassword=!this.showNewPassword;
  }

  onPassword2Change(event: any) {
    if(event.target.value!="") {
      this.emptyNewPassword2=false;
      this.emptyNewPassword2AfterModify=false;
      this.currentNewPassword2=event.target.value;
      if(this.currentNewPassword==this.currentNewPassword2) {
        if(this.isNewPassword2Matches==false)
          this.isNewPassword2Matches = true;
      }
      else {
        if(this.isNewPassword2Matches==true)
          this.isNewPassword2Matches = false;
      }
    }
    else
    {
      this.emptyNewPassword2AfterModify=true;
      this.isNewPassword2Matches=false;
    }
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

  getupdateImage() {
    this.isImageLoading = true;
    this.staticFileServicesService.getLoginImage().subscribe(data => {
      this.createImageFromBlob(data);
      this.isImageLoading = false;
    }, error => {
      this.isImageLoading = false;
    });
  }

  onSubmit() {
    console.log("xxx");
    if (this.currentNewPassword != "" && this.currentNewPassword2 != ""&& this.currentOldPassword != "" ) {
      this.emptyNewPassword2 = false;
      this.emptyNewPassword = false;
      this.emptyPassword = false;
      this.emptyOldPassword = false;
      this.errorInRegister = false;
      if(  this.isPasswordMinimumLengthGood && this.isPasswordAtLeastOneUppercase
        && this.isPasswordAtLeastOneLowercase && this.isPasswordAtLeastOneDigit
        && this.isNewPassword2Matches)
      {
        this.usersService.updateUserPassword(this.currentOldPassword,this.currentNewPassword).subscribe(data => {
          const result= data;
          console.log(result);
          this.errorInRegister=false;
          this.errorMessage="Password Update Succeeded";
          this.dialog.open(this.updatePasswordMessage);
        }, error => {
          console.log(error);
          this.errorInRegister = true;
          this.errorMessage="Wrong Old Password";
          this.dialog.open(this.updatePasswordMessage);
        });
      }
    }
    else
    {
      if (this.currentOldPassword == "")
        this.emptyOldPassword = true;
      if (this.currentNewPassword == "")
        this.emptyNewPassword = true;
      if (this.currentNewPassword2 == "")
        this.emptyNewPassword2 = true;
      this.errorInRegister = true;
    }

  }
}
