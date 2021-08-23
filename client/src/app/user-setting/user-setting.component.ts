import { Component, OnInit } from '@angular/core';
import {UsersService} from "../Services/UserServices/user-services.service";
import {StaticFileServicesService} from "../Services/StaticFileServices/staticfile-services.service";
import {FormBuilder} from "@angular/forms";
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
  updateForm = this.formBuilder.group({
    InputFirstName: '',
    InputLastName: '',
    InputAddress: '',
    InputDescription: '',
    InputPhoneNumber:'',

  });
  constructor(
    private usersService: UsersService,
    private staticFileServicesService: StaticFileServicesService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}

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
    }, (error: any) => {
      console.log(error)
    });


  }
  onSubmit() {

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
