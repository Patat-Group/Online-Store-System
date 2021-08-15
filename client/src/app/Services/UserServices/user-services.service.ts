import { Injectable, EventEmitter, Output } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Products } from 'src/app/Models/Products';
import {Login} from "../../Models/Login";
import { Register } from 'src/app/Models/Register';
import { UserInfo } from '../../Models/UserInfo';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  @Output() getLoggedInName: EventEmitter<any> = new EventEmitter();
  constructor(private http: HttpClient) {}
  baseUrl = 'http://localhost:5000/api/user/';
  loginUserUrl = this.baseUrl+"login/";
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
