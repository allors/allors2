import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { AllorsService } from "@allors";

import { LoginComponent } from "./auth/login.component";

import { FetchComponent } from "./fetch/fetch.component";
import { FormComponent } from "./form/form.component";
import { HomeComponent } from "./home/home.component";
import { QueryComponent } from "./query/query.component";

const routes: Routes = [
  {
    component: LoginComponent,
    path: "login",
  },
  {
    canActivate: [AllorsService],
    children: [
      {
        component: HomeComponent,
        path: "",
      },
      {
        component: FormComponent,
        path: "form",
      },
      {
        component: QueryComponent,
        path: "query",
      },
      {
        component: FetchComponent,
        path: "fetch/:id",
      },
    ],
    path: "",
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)],
})
export class AppRoutingModule {}
