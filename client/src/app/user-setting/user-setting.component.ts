import { Component, OnInit } from '@angular/core';
import {UsersService} from "../Services/UserServices/user-services.service";
import {StaticFileServicesService} from "../Services/StaticFileServices/staticfile-services.service";
import {FormGroup, FormBuilder, Validators } from '@angular/forms';
import {Router} from "@angular/router";
import {UserInfo} from "../Models/UserInfo";

@Component({
  selector: 'app-user-setting',
  templateUrl: './user-setting.component.html',
  styleUrls: ['./user-setting.component.scss']
})
export class UserSettingComponent implements OnInit {
  imageToShow: any;
  isImageLoading: boolean | any;
  currentGender: string | any;
  currnetUsername: string | any;
  userInfo= new UserInfo();
  constructor(
    private usersService: UsersService,
    private staticFileServicesService: StaticFileServicesService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}
  updateForm = this.formBuilder.group({
    InputFirstName: '',
    InputLastName: '',
    InputAddress: '',
    InputDescription: '',
    InputPhoneNumber:'',
    InputTelegram:'',
    InputWhatsapp:'',
    InputFacebook:'',
  });

  ngOnInit(): void {
    if (this.usersService.isLoggedOut())
      this.router.navigateByUrl("/login");
    else
    {
      this.currnetUsername=localStorage.getItem("username");
      this.userInfo.gender="";
      this.getUserData();
      this.getupdateImage();
    }
  }
  getUserData() {
    this.usersService.getUserDataWithUsername(this.currnetUsername).subscribe((data: any) => {
      console.log(data)
      this.userInfo=data;
      this.updateForm.value.InputAddress=this.userInfo.address;
      this.updateForm.value.InputFirstName=this.userInfo.firstName;
      this.updateForm.value.InputLastName=this.userInfo.lastName;
      this.updateForm.value.InputPhoneNumber=this.userInfo.phoneNumber;
      this.updateForm.value.InputDescription=this.userInfo.description;
      this.updateForm.value.InputFacebook=this.userInfo.facebookUrl;
      this.updateForm.value.InputWhatsapp=this.userInfo.whatsappUrl;
      this.updateForm.value.InputTelegram=this.userInfo.telegramUrl;
      this.currentGender=this.userInfo.gender?.toLowerCase();
    }, (error: any) => {
      console.log(error)
    })
  }
  onSubmit() {
      const userInfo= new UserInfo();
      console.log(this.updateForm.value.InputAddress);
      userInfo.address=this.updateForm.value.InputAddress;
      userInfo.firstName=this.updateForm.value.InputFirstName;
      userInfo.lastName=this.updateForm.value.InputLastName;
      userInfo.phoneNumber=this.updateForm.value.InputPhoneNumber;
      userInfo.facebookUrl=this.updateForm.value.InputFacebook;
      userInfo.whatsappUrl=this.updateForm.value.InputWhatsapp;
      userInfo.telegramUrl=this.updateForm.value.InputTelegram;
      userInfo.description=this.updateForm.value.InputDescription;
      userInfo.gender=this.currentGender;
      this.usersService.updateUserInfo(userInfo).subscribe( (data: any)=> {
        if(data.message=="Update Succeeded")
          this.router.navigateByUrl("/profile");
      }, (error: any) => {
        console.log(error);
      });

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
  onGenderChange(event: any) {
    this.currentGender=event.target.value;
  }
  omitSpecialCharToPhoneNumber(event: KeyboardEvent) {
    var k;
    k = event.charCode;
    return (k==8 || k==43 || (k >= 48 && k <= 57));
  }

}
