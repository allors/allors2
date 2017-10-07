import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { AuthenticationService } from "../allors/angular";
import { LoginComponent } from "./auth/login.component";

import { DashboardComponent } from "./dashboard/dashboard.component";
import { FormComponent } from "./form/form.component";

const routes: Routes = [
  { path: "login", component: LoginComponent },
  {
    canActivate: [AuthenticationService],
    children: [{
      component: DashboardComponent,
      path: "",
    },
    {
      component: FormComponent,
      path: "form",
    }],
    path: "",
  },
];

@NgModule({
  exports: [RouterModule],
  imports: [RouterModule.forRoot(routes)],
})
export class AppRoutingModule { }
