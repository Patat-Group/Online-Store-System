import { Routes } from "@angular/router";
import { MainPageComponent } from "./main/main.component";
import { ProductComponent } from "./Product/product.component";

export const appRoutes: Routes =
  [
    { path: "home", component: MainPageComponent },
    { path: "products/:id", component: ProductComponent },
    { path: "**", component: MainPageComponent }
  ];
