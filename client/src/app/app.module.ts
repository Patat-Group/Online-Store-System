import { appRoutes } from './routes';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavBarComponent } from './nav-bar/nav-bar.component'
import { MainPageComponent } from './main/main.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { FooterComponent } from './footer/footer.component';
import { ProductComponent } from './Product/product.component';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { LoginPageComponent } from './login/login.component';
import { CategoriesNavBarComponent } from './categories-nav-bar/categories-nav-bar.component';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { MatSelectModule } from '@angular/material/select';
import { MatSliderModule } from '@angular/material/slider';
import { MatDialogModule } from '@angular/material/dialog';
import { RegisterComponent } from './register/register.component';
import { UserProfileComponent } from './user-profile/user-profile.component';
import { UserSettingComponent } from './user-setting/user-setting.component';
import { UserChangePasswordComponent } from './user-change-password/user-change-password.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { ProductsService } from './Services/ProductServices/products.service';
import { AddProductComponent } from './add-product/add-product.component';

@NgModule({
  imports: [
    NgbModule,
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    BrowserAnimationsModule,
    HttpClientModule,
    ReactiveFormsModule,
    MatMenuModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    ScrollingModule,
    NgxPaginationModule,
    MatSelectModule,
    MatSliderModule,
    MatDialogModule,
    RouterModule.forRoot(appRoutes),
    PaginationModule.forRoot(),
  ],
  declarations: [
    AppComponent,
    NavBarComponent,
    FooterComponent,  
    ProductComponent,
    ProductDetailsComponent,
    CategoriesNavBarComponent,
    LoginPageComponent,
    RegisterComponent,
    UserProfileComponent,
    UserSettingComponent,
    UserChangePasswordComponent,
    ProductComponent,
    MainPageComponent,
    AddProductComponent
  ],
  exports: [
    MatButtonModule,
    MatMenuModule,
    MatIconModule,
    MatCardModule,
    ScrollingModule,
    MatSelectModule,
    MatSliderModule,
    MatDialogModule,
  ],
  providers: [  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
