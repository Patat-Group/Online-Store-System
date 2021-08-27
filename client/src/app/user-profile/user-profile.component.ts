import { Component, OnInit, Output, EventEmitter,TemplateRef } from '@angular/core';
import { StaticFileServicesService } from '../Services/StaticFileServices/staticfile-services.service';
import {UsersService} from "../Services/UserServices/user-services.service";
import {ActivatedRoute, Router} from "@angular/router";
import { HttpEventType } from '@angular/common/http';
import { UserInfo } from '../Models/UserInfo';
import { UserRate } from '../Models/UserRate';
import { MatDialogModule } from '@angular/material/dialog';
import { MatDialog} from '@angular/material/dialog';
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {

  @Output() public onUploadImageFinished = new EventEmitter();
  newRateValue: any;
  constructor( private staticFileServicesService: StaticFileServicesService,
               private usersService: UsersService,
               private router: Router,
               private route: ActivatedRoute,
               private dialog: MatDialog
               ) {
    usersService.getLoggedInName
      .subscribe(user => this.changeUsername(user));
  }

  userPhoto: any;
  facebookImage: any;
  whatsappImage: any;
  telegramImage: any;
  telephoneImage: any;
  genderImage: any;
  isImageLoading :boolean | any;
  isPhotoHover=false;
  isUserLoggedIn=this.usersService.isLoggedIn();
  isUserOwnProfile=false;
  ratioOfOneStar=0;
  ratioOfTwoStar=0;
  ratioOfThreeStar=0;
  ratioOfFourStar=0;
  ratioOfFiveStar=0;
  currentSelectedRate=0;
  ratioOfTotalRate=0;
  ratioColor='bg-secondary';
  ratioWord="";
  totalRateCount=0;
  currentLastSeen:any;
  secondsFromLastSeen:any;
  wordPresentLastSeen:any;
  isSettingRate=false;
  currnetUsername:any;
  userInfo= new UserInfo();
  userRate=new UserRate();
  openDialogWithTemplateRef(templateRef: TemplateRef<any>) {
    this.dialog.open(templateRef);
  }
  ngOnInit(): void {
    const username = this.route.snapshot.paramMap.get('username');
    if(username==null) {
      if (this.usersService.isLoggedOut())
        this.router.navigateByUrl("/login");
      else {
        this.checkUsername();
        this.currnetUsername=localStorage.getItem('username');
        this.getUserData();
        this.getUserToUserRate();
        this.getFacebookImage();
        this.getTelegramImage();
        this.getTelephoneImage();
        this.getWhatsappImage();

      }
    }
    else
    {
      const username = this.route.snapshot.paramMap.get('username');
      // @ts-ignore
      this.usersService.checkIfUserExist(username).subscribe((data: string) => {
        if(data=="false")
        {
          this.router.navigateByUrl("/");
          return;
          // user not found so navigate to main page
        }
        this.checkUsername();
        // @ts-ignore
        this.currnetUsername=username.toLowerCase();
        this.getUserData();
        this.getFacebookImage();
        this.getTelegramImage();
        this.getTelephoneImage();
        this.getWhatsappImage();
        if(this.usersService.isLoggedIn())
          this.getUserToUserRate();
      }, (error: any) => {
        console.log(error);
      });
    }
    // console.log("Own Profile : "+this.isUserOwnProfile);
    // console.log("Logged In : "+this.isUserLoggedIn);
  }

  private getUserToUserRate() {
    this.usersService.getUserToUserRate(this.currnetUsername).subscribe((data: any) => {
      if(data!="User Not Rated Yet") {
        this.currentSelectedRate = data;
      }
    }, (error: any) => {
      console.log(error);
    });
    }
  private checkUsername(){
    const username = this.route.snapshot.paramMap.get('username');
    if(this.usersService.isLoggedIn())
    {
      if(username?.toLowerCase()==localStorage.getItem('username')?.toLowerCase() || username==null)
      {
        this.isUserOwnProfile=true;
        this.isUserLoggedIn=true;
      }
      else
      {
        this.isUserLoggedIn=true;
        this.isUserOwnProfile=false;
      }
    }
    else
    {
      this.isUserOwnProfile=false;
      this.isUserLoggedIn=false;
    }
  }
  private changeUsername(name: string): void {
    if(name!=null)
      this.isUserLoggedIn=true;
    else
      this.isUserLoggedIn=false;
  }
  createImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.userPhoto = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
  createFacebookImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.facebookImage = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
  createWhatsappImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.whatsappImage = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
  createTelegramImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.telegramImage = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
  createTelephoneImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.telephoneImage = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
  createGenderImageFromBlob(image: Blob) {
    let reader = new FileReader();
    reader.addEventListener("load", () => {
      this.genderImage = reader.result;
    }, false);

    if (image) {
      reader.readAsDataURL(image);
    }
  }
  getLoginImage() {
    this.isImageLoading = true;
      // console.log("http://localhost:5000"+this.userInfo.pictureUrl);
      this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+this.userInfo.pictureUrl).subscribe((data: Blob) => {
        this.createImageFromBlob(data);
        this.isImageLoading = false;

      }, (error: any) => {
        this.isImageLoading = false;
      });
  }

  getFacebookImage() {
    this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+"/images/facebook.png").subscribe((data: Blob) => {
      this.createFacebookImageFromBlob(data);
    }, (error: any) => {
      console.log(error);
    });
  }
  getTelephoneImage() {
    this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+"/images/telephone.png").subscribe((data: Blob) => {
      this.createTelephoneImageFromBlob(data);
    }, (error: any) => {
      console.log(error);
    });
  }
  getWhatsappImage() {
    this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+"/images/whatsapp.png").subscribe((data: Blob) => {
      this.createWhatsappImageFromBlob(data);
    }, (error: any) => {
      console.log(error);
    });
  }
  getGenderImage() {
    if(this.userInfo.gender?.toLowerCase()=="female")
    this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+"/images/female.png").subscribe((data: Blob) => {
      this.createGenderImageFromBlob(data);
    }, (error: any) => {
      console.log(error);
    });
    else
      this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+"/images/male.png").subscribe((data: Blob) => {
        this.createGenderImageFromBlob(data);
      }, (error: any) => {
        console.log(error);
      });
  }
  getTelegramImage() {
    this.staticFileServicesService.getPictureWithUrl("http://localhost:5000"+"/images/telegram.png").subscribe((data: Blob) => {
      this.createTelegramImageFromBlob(data);
    }, (error: any) => {
      console.log(error);
    });
  }
  getUserData() {
    this.usersService.getUserDataWithUsername(this.currnetUsername).subscribe((data: any) => {
      // console.log(data)
      this.userInfo=data;
      this.currentLastSeen=this.userInfo.lastSeen;
      this.wordPresentLastSeen=this.transform(this.currentLastSeen);
      this.secondsFromLastSeen=this.getSecondsFromDate(this.currentLastSeen);
      // console.log(this.userInfo.pictureUrl)
      this.getLoginImage();
      this.getUserRateToUser();
      this.getGenderImage();
    }, (error: any) => {
      console.log(error)
    });


  }
  getUserRateToUser(){
    this.usersService.getUserRateWithUsername(this.currnetUsername).subscribe((data: any) => {
      // console.log(data)
      this.userRate=data;
      this.totalRateCount= this.userRate.oneStarCount
        + this.userRate.twoStarCount
        + this.userRate.threeStarCount
        + this.userRate.fourStarCount
        + this.userRate.fiveStarCount;
      this.ratioOfTotalRate=
        this.userRate.oneStarCount
        + 2*this.userRate.twoStarCount
        + 3*this.userRate.threeStarCount
        + 4*this.userRate.fourStarCount
        + 5*this.userRate.fiveStarCount;
      if(this.totalRateCount==0) {
        this.ratioOfOneStar=0;
        this.ratioOfTwoStar=0;
        this.ratioOfThreeStar=0;
        this.ratioOfFourStar=0;
        this.ratioOfFiveStar=0;
        this.ratioOfTotalRate = 0;
        this.ratioWord="Not Rated";
        this.ratioColor="bg-secondary";

      }
      else {
        this.ratioOfOneStar=this.roundFloat(100*this.userRate.oneStarCount/ this.totalRateCount);
        this.ratioOfTwoStar=this.roundFloat(100*this.userRate.twoStarCount/ this.totalRateCount);
        this.ratioOfThreeStar=this.roundFloat(100*this.userRate.threeStarCount/ this.totalRateCount);
        this.ratioOfFourStar=this.roundFloat(100*this.userRate.fourStarCount/ this.totalRateCount);
        this.ratioOfFiveStar=this.roundFloat(100*this.userRate.fiveStarCount/ this.totalRateCount);
        this.ratioOfTotalRate = this.roundFloat(this.ratioOfTotalRate / this.totalRateCount);
        if(this.ratioOfTotalRate>=1.0) {
          this.ratioColor = "bg-danger";
          this.ratioWord="Poor";
        }
        if(this.ratioOfTotalRate>=2.0) {
          this.ratioColor = "bg-warning";
          this.ratioWord="Fair";
        }
        if(this.ratioOfTotalRate>=3.0) {
          this.ratioColor = "bg-info";
          this.ratioWord="Good";
        }
        if(this.ratioOfTotalRate>=4.0) {
          this.ratioColor = "bg-primary";
          this.ratioWord="Very Good";
        }
        if(this.ratioOfTotalRate>=5.0) {
          this.ratioColor = "bg-success";
          this.ratioWord="Excellent";
        }
      }
    }, (error: any) => {
      console.log(error)
    });
  }
  userPhotoHoverIn() {
    this.isPhotoHover=true;
  }
  userPhotoHoverOut(){
    this.isPhotoHover=false;
  }
  public uploadFile (event:any){
    const files=event.target.files
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    this.usersService.updateUserProfilePicture(fileToUpload)
      .subscribe((event :any) => {
          //  console.log("done upload");
           this.getUserData();
      }, (error: any) => {
        console.log(error)
      });
  }
  getSecondsFromDate(value: any, args?: any): any {
    if (value) {
      const seconds = Math.floor((+new Date() - +new Date(value)) / 1000);
      return seconds;
    }
  }
  transform(value: any, args?: any): any {
    if (value) {
      const seconds = Math.floor((+new Date() - +new Date(value)) / 1000);
      if (seconds <= 5) // less than 5 seconds ago will show as 'Just now'
        return 'Online';
      if (seconds < 29) // less than 30 seconds ago will show as 'Just now'
        return 'Just now';
      const intervals :any = {
        'year': 31536000,
        'month': 2592000,
        'week': 604800,
        'day': 86400,
        'hour': 3600,
        'minute': 60,
        'second': 1
      };
      let counter;
      for (const i in intervals) {
        counter = Math.floor(seconds / intervals[i]);
        if (counter > 0)
          if (counter === 1) {
            return counter + ' ' + i + ' ago'; // singular (1 day ago)
          } else {
            return counter + ' ' + i + 's ago'; // plural (2 days ago)
          }
      }
    }
    return value;
  }

  onRatingChange(event: any) {

    if(this.isSettingRate==false) {
      this.isSettingRate=true;
      this.newRateValue = event.target.value;
      // console.log(this.newRateValue);
      this.usersService.setRateUserToUser(this.currnetUsername, this.newRateValue).subscribe((event: any) => {
        this.getUserToUserRate();
        this.getUserRateToUser();
        this.isSettingRate=false;
      }, (error: any) => {
        console.log(error);
        this.isSettingRate=false;
      });
    }
  }
  roundFloat(number:any)
  {
    return Math.round((number + Number.EPSILON) * 100) / 100;
  }

}
