import { Component, OnInit } from '@angular/core';
import {UsersService} from "../Services/UserServices/user-services.service";
import {StaticFileServicesService} from "../Services/StaticFileServices/staticfile-services.service"
import {FormBuilder} from "@angular/forms";
import {Router} from "@angular/router";
import { UserInfo } from '../Models/UserInfo';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  imageToShow: any;
  registerResult:any;
  showPassword = false;
  registerPhase2 = false;
  currentPassword:string|any;
  currentUsername:string|any;
  currentEmail:string|any;
  currentPassword2:string|any;
  currentPhoneNumber:string|any;
  currentLastName:string|any;
  currentFirstName:string|any;
  currentAddress:string|any;
  currentGender="male";
  errorInRegister = false;
  emptyUsername = false;
  emptyEmail = false;
  emptyPassword = false;
  emptyFirstName = false;
  emptyLastName = false;
  emptyPhoneNumber = false;
  emptyAddress = false;
  isPasswordMinimumLengthGood=false;
  isPasswordAtLeastOneDigit=false;
  isPasswordAtLeastOneUppercase=false;
  isPasswordAtLeastOneLowercase=false;
  isUsernameLengthGood=false;
  isUsernameValid=false;
  isEmailValid=false;
  isEmailNotUsed=false;
  emptyPasswordAfterModify = true;
  emptyPassword2AfterModify = true;
  isPassword2Matches=false;
  emptyUsernameAfterModify = true;
  emptyEmailAfterModify = true;
  emptyPassword2 = false;
  errorRegisterMessage="Invalid username/email or password";
  isImageLoading: boolean | any;
  registerForm = this.formBuilder.group({
    InputEmail: '',
    InputUsername: '',
    InputPassword: '',
    InputPassword2: '',

  });
  registerForm2 = this.formBuilder.group({
    InputFirstName: '',
    InputLastName: '',
    InputAddress: '',
    InputPhoneNumber: '',

  });


  constructor(
    private staticFileServicesServicea: StaticFileServicesService,
    private usersService: UsersService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}
  onSubmit(): void {
    if (this.currentEmail != "" && this.currentUsername != ""&& this.currentPassword != "" && this.currentPassword2 != "") {
      this.emptyUsername = false;
      this.emptyEmail = false;
      this.emptyPassword = false;
      this.emptyPassword2 = false;
      this.errorInRegister = false;
      if(this.isUsernameValid && this.isUsernameLengthGood
        && this.isEmailNotUsed && this.isEmailValid
        && this.isPasswordMinimumLengthGood && this.isPasswordAtLeastOneUppercase
        && this.isPasswordAtLeastOneLowercase && this.isPasswordAtLeastOneDigit
        && this.isPassword2Matches)
      {
        this.usersService.register(this.currentUsername,this.currentEmail, this.currentPassword).subscribe(data => {
          this.registerResult = data;
          localStorage.setItem('username', this.registerResult.username);
          localStorage.setItem('email', this.registerResult.email);
          localStorage.setItem('token', this.registerResult.token);
          this.usersService.setUsername(this.registerResult.username);
          this.registerPhase2=true;

        }, error => {
          console.log(error);
          this.errorInRegister = true;
        });
      }
    }
    else
    {
      if (this.registerForm.value.InputEmail == "")
        this.emptyUsername = true;
      if (this.registerForm.value.InputPassword == "")
        this.emptyPassword = true;
      this.errorInRegister = true;
    }
  }
  omitSpecialChar(event:any)
  {
    var k;
    k = event.charCode;
    return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 95 || k == 46 || k==8 || (k >= 48 && k <= 57));
  }
  onUsernameChange(event: any)
  {
    if(event.target.value!="") {
      this.emptyUsername = false;
      this.emptyUsernameAfterModify = event.target.value == "";
      this.currentUsername = event.target.value;
      this.isUsernameLengthGood=this.checkIfStringHaveLengthBetweenGood(this.currentUsername,3,20);
      if(this.isUsernameLengthGood)
      {
        this.usersService.checkIfUserExist(this.currentUsername).subscribe(data => {
          this.isUsernameValid=data=="false"
        }, error => {
          console.log(error);
        });
      }
      else
        this.isUsernameValid=false;
    }
    else
    {  this.emptyUsernameAfterModify = true;
      this.isUsernameLengthGood=false;
      this.isUsernameValid=false;
      this.emptyUsername=true;
    ;}
  }
  onEmailChange(event: any)
  {
    if(event.target.value!="")
    {
      this.emptyEmail=false;
      this.emptyEmailAfterModify=false;
      this.currentEmail=event.target.value;
      this.isEmailValid=this.checkIfStringIsVaildEmail(this.currentEmail);
      if(this.isEmailValid)
      {
        this.usersService.checkIfEmailExist(this.currentEmail).subscribe(data => {
          this.isEmailNotUsed=data=="false"
        }, error => {
          console.log(error);
        });
      }
      else
        this.isEmailNotUsed=false;

    }
    else
    {
      this.emptyEmail=true;
      this.emptyEmailAfterModify=true;
      this.isEmailValid=false;
      this.isEmailNotUsed=false;
    }
  }
  onPasswordChange(event: any)
  {
    if(event.target.value!="") {
      this.emptyPasswordAfterModify = false;
      this.emptyPassword=false;
      this.currentPassword=event.target.value;
      this.isPasswordAtLeastOneDigit=this.checkIfStringHaveAtLeastOneDigit(this.currentPassword);
      this.isPasswordAtLeastOneLowercase=this.checkIfStringHaveAtLeastOneLowercase(this.currentPassword);
      this.isPasswordAtLeastOneUppercase=this.checkIfStringHaveAtLeastOneUppercase(this.currentPassword);
      this.isPasswordMinimumLengthGood=this.checkIfStringHaveMinimumLengthGood(this.currentPassword,6);
    }
    else {
      this.emptyPasswordAfterModify = true;
      this.isPasswordAtLeastOneDigit=false;
      this.isPasswordAtLeastOneLowercase=false;
      this.isPasswordAtLeastOneUppercase=false;
      this.isPasswordMinimumLengthGood=false;
      this.emptyPassword=true;
      this.currentPassword="";
    }
    if(this.currentPassword==this.currentPassword2 && this.currentPassword2!="")
      this.isPassword2Matches=true;
    else
      this.isPassword2Matches=false;

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
  checkIfStringIsVaildEmail(s:string)
  {
    const regexToCheck = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regexToCheck.test(s.toLowerCase());
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
  checkIfStringHaveLengthBetweenGood(s:string ,a:any,b:any)
  {
    return s.length>=a && s.length<=b;
  }

  onPassword2Change(event: any)
  {
    if(event.target.value!="") {
        this.emptyPassword2=false;
        this.emptyPassword2AfterModify=false;
        this.currentPassword2=event.target.value;
        if(this.currentPassword==this.currentPassword2) {
          if(this.isPassword2Matches==false)
          this.isPassword2Matches = true;
        }
        else {
          if(this.isPassword2Matches==true)
          this.isPassword2Matches = false;
        }
    }
    else
    {
      this.emptyPassword2AfterModify=true;
      this.isPassword2Matches=false;
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
  toggleShow() {
    this.showPassword = !this.showPassword;
  }
  getRegisterImage() {
    this.isImageLoading = true;
    this.staticFileServicesServicea.getLoginImage().subscribe((data: Blob) => {
      this.createImageFromBlob(data);
      this.isImageLoading = false;
    }, (error: any) => {
      this.isImageLoading = false;
    });
  }

  ngOnInit(): void {
    if(this.usersService.isLoggedIn())
      this.router.navigateByUrl("/profile");
    else
      this.getRegisterImage();
  }

  onSubmit2() {
    if(this.emptyAddress==false
      && this.emptyFirstName==false
      && this.emptyLastName==false
      && this.emptyPhoneNumber==false
    )
    {
      const userInfo= new UserInfo();
      userInfo.address=this.currentAddress;
      userInfo.firstName=this.currentFirstName;
      userInfo.lastName=this.currentLastName;
      userInfo.phoneNumber=this.currentPhoneNumber;
      userInfo.gender=this.currentGender;
      this.usersService.updateUserInfo(userInfo).subscribe( (data: any)=> {
        if(data.message=="Update Succeeded")
          this.router.navigateByUrl("/");
      }, (error: any) => {
        console.log(error);
      });
    }

  }

  omitSpecialCharToPhoneNumber(event: KeyboardEvent) {
    var k;
    k = event.charCode;
    return (k==8 || k==43 || (k >= 48 && k <= 57));
  }

  onGenderChange(event: any) {
    this.currentGender=event.target.value;
  }

  onLastNameChange(event: any) {
    this.emptyLastName=event.target.value=="";
    this.currentLastName=event.target.value;
  }

  onAddressChange(event: any) {
    this.emptyAddress=event.target.value=="";
    this.currentAddress=event.target.value;
  }

  onFirstNameChange(event: any) {
    this.emptyFirstName=event.target.value=="";
    this.currentFirstName=event.target.value;
  }

  onPhoneNumberChange(event: any) {
    this.emptyPhoneNumber=event.target.value=="";
    this.currentPhoneNumber=event.target.value;
  }
}
