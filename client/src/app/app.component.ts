import { Component, OnInit } from '@angular/core';
import { interval } from "rxjs";
import { UsersService } from "../app/Services/UserServices/user-services.service";
import construct = Reflect.construct;
import { StaticFileServicesService } from "./Services/StaticFileServices/staticfile-services.service";
import { FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Ptata';
  constructor(
    private usersService: UsersService,
  ) { }
  ngOnInit(): void {
    const counter = interval(4000);

    const updateLastSeen = counter
      .subscribe((event: any) => {
        if (this.usersService.isLoggedIn()) {
          this.usersService.updateLastSeen().subscribe((data: any) => {
            // console.log(data);
          }, (error: any) => {
            console.log(error);
          });
        }
      }, (error: any) => {
        console.log(error);
      });
  }

}
