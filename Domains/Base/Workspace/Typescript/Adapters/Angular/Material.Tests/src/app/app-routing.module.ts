import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { AllorsService } from "./allors.service";

import { LoginComponent } from "./auth/login.component";
import { DashboardComponent } from "./dashboard/dashboard.component";
import { FormComponent } from "./form/form.component";

const routes: Routes = [
  { path: "login", component: LoginComponent },
  {
    canActivate: [AllorsService],
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
