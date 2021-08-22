import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Products } from 'src/app/Models/Products';
import {Login} from "../../Models/Login";
import { Register } from 'src/app/Models/Register';
import { UserInfo } from '../../Models/UserInfo';
import { interval } from 'rxjs'
@Injectable({
  providedIn: 'root'
})
export class UsersService {

  getUserToUserRateWithUrl(arg0: string) {
      throw new Error("Method not implemented.");
  }

  @Output() getLoggedInName: EventEmitter<any> = new EventEmitter();
  constructor(private http: HttpClient) {}
  baseUrl = 'http://localhost:5000/api/user/';
  baseRatingUrl='http://localhost:5000/api/rating/'
  getUserDataWithUsernameUrl = this.baseUrl;
  getUserRateWithUsernameUrl = this.baseRatingUrl+"details/";
  getUserToUserRateUrl = this.baseRatingUrl+"myrate/";
  setUserToUserRateUrl = this.baseRatingUrl;
  getUserRate=this.baseRatingUrl+'details/';
  loginUserUrl = this.baseUrl+"login/";
  updatePhotoUserUrl = this.baseUrl+"photo/";
  registerUserUrl = this.baseUrl+"register/";
  usernameChackExistUrl=this.baseUrl+"usernameExist/";
  emailChackExistUrl=this.baseUrl+"emailExist/";
  updateUserInfoUrl=this.baseUrl;
  public checkIfUserExist(username:string): Observable<String> {
    return this.http.get(this.usernameChackExistUrl+"?username="+username, {responseType: 'text'});
  }
  public checkIfEmailExist(email:string): Observable<String> {
    return this.http.get(this.emailChackExistUrl+"?email="+email.toLowerCase(), {responseType: 'text'});
  }
  public getUserData(): Observable<any> {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    return this.http.get(this.baseUrl,httpOptions);
  }
  public getUserDataWithUsername(username:string): Observable<any> {
    return this.http.get(this.getUserDataWithUsernameUrl+username);
  }
  public getUserToUserRate(username:string): Observable<any> {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    return this.http.get(this.getUserToUserRateUrl+username,httpOptions);
  }
  public setRateUserToUser(username:string,value :any): Observable<any> {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    var body:any = {};
    body["username"]=username;
    body["value"]=value;
    return this.http.put(this.setUserToUserRateUrl,body,httpOptions).pipe();
  }
  public updateLastSeen() :Observable<any>
  {
    var userInfoToUpdate:any = {};
    const currentDate =new Date().toLocaleString()
    userInfoToUpdate["lastSeen"]=currentDate.replace(",","");
    console.log(userInfoToUpdate);
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    return this.http.put<String>(this.updateUserInfoUrl, userInfoToUpdate,httpOptions).pipe();
  }
  public getUserRateWithUsername(username:string): Observable<any> {
    return this.http.get(this.getUserRateWithUsernameUrl+username);
  }

  public updateUserProfilePicture(newPicture: File): Observable<any> {
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    const formData = new FormData();
    formData.append('File', newPicture, newPicture.name);
    console.log(formData);
    return this.http.put(this.updatePhotoUserUrl,formData,httpOptions).pipe();
  }
  public updateUserInfo(userInfo:UserInfo): Observable<String>{
    var userInfoToUpdate:any = {};
    for (let [key, value] of Object.entries(userInfo)) {
      if(key=="gender")
        value=value[0].toUpperCase()+value.substring(1).toLowerCase();
      if(value!="")
      userInfoToUpdate[key.toString()]=value;
    }
    console.log(userInfoToUpdate);
    var headersObject = new HttpHeaders().set("Authorization", "Bearer " + localStorage.getItem('token'));
    const httpOptions = {
      headers: headersObject
    };
    return this.http.put<String>(this.updateUserInfoUrl, userInfoToUpdate,httpOptions).pipe();
  }
  public login(loginString: string, password: string): Observable<Login> {
    return this.http.post<Login>(this.loginUserUrl, {loginString: loginString, password: password}).pipe();
  }
  public register(username: string,email:string ,password: string): Observable<Register> {
    return this.http.post<Register>(this.registerUserUrl, {username: username, email: email,password:password}).pipe();
  }
  public isLoggedIn() {
    return localStorage.getItem('username')!=null;
  }
  public isLoggedOut() {
    return localStorage.getItem('username')==null;
  }
  public getCurrentUsername(){
    this.getLoggedInName.emit(localStorage.getItem('username'));
  }
  public setUsername(username: string) {
    this.getLoggedInName.emit(username);
  }

  public logout() {
    this.getLoggedInName.emit("");
    localStorage.removeItem('username');
    localStorage.removeItem('email');
    localStorage.removeItem('token');
  }


}
