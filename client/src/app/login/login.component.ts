import {Component, OnInit} from '@angular/core';
import {FormGroup, FormBuilder, Validators } from '@angular/forms';
import {CategoryServicesService} from "../Services/CategoryServices/category-services.service";
import {UsersService} from "../Services/UserServices/user-services.service";
import {Router} from '@angular/router';
import { StaticFileServicesService } from '../Services/StaticFileServices/staticfile-services.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginPageComponent implements OnInit {
  imageToShow: any;
  loginResult:any;
  showPassword = false;
  errorInLogin = false;
  emptyUsername = false;
  emptyPassword = false;
  errorLoginMessage="Invalid username/email or password";
  isImageLoading: boolean | any;
  loginForm = this.formBuilder.group({
    InputEmail: '',
    InputPassword: ''
  });

  constructor(
    private usersService: UsersService,
    private staticFileServicesService: StaticFileServicesService,
    private formBuilder: FormBuilder,
    private router: Router,
  ) {}
  onSubmit(): void {
    if (this.loginForm.value.InputEmail != "" && this.loginForm.value.InputPassword != "") {
      this.emptyUsername = false;
      this.emptyPassword = false;
      this.errorInLogin = false;
      this.usersService.login(this.loginForm.value.InputEmail, this.loginForm.value.InputPassword)
                        .subscribe(data => {
        this.loginResult = data;
        localStorage.setItem('username', this.loginResult.username);
        localStorage.setItem('email', this.loginResult.email);
        localStorage.setItem('token', this.loginResult.token);
        this.usersService.setUsername(this.loginResult.username);
        this.usersService.updateLastSeen().subscribe((data:any) => {
          console.log(data);
        }, (error: any) => {
          console.log(error);
        });
        this.router.navigateByUrl('/');

      }, error => {
        console.log(error);
        this.errorLoginMessage="Invalid username/email or password";
        this.errorInLogin = true;

      });
    }
    else
    {
      if (this.loginForm.value.InputEmail == "")
        this.emptyUsername = true;
      if (this.loginForm.value.InputPassword == "")
        this.emptyPassword = true;
      this.errorLoginMessage="Username/email or password can't be empty";
      this.errorInLogin = true;
    }
  }
  onUsernameChange(event: any)
  {
    if(event.target.value!="")
      this.emptyUsername = false;
  }
  onPasswordChange(event: any)
  {
    if(event.target.value!="")
      this.emptyPassword = false;
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

  getLoginImage() {
    this.isImageLoading = true;
    this.staticFileServicesService.getLoginImage().subscribe(data => {
      this.createImageFromBlob(data);
      this.isImageLoading = false;
    }, error => {
      this.isImageLoading = false;
    });
  }
  toggleShow() {
    this.showPassword = !this.showPassword;
  }
  ngOnInit(): void {
    this.getLoginImage();
  }
}
