import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { SharedModule } from "./shared/shared.module";

import { AllorsService } from "@allors";

import { LoginComponent } from "./common/auth/login.component";
import { DashboardComponent } from "./common/dashboard/dashboard.component";
import { MainComponent } from "./common/main/main.component";

import * as relations from "../allors/intranet/apps/relations";

// tslint:disable:object-literal-sort-keys
const routes: Routes = [
  { path: "login", component: LoginComponent },
  {
    canActivate: [AllorsService],
    path: "",
    component: MainComponent,
    children: [
      {
        path: "", component: DashboardComponent, data: { type: "module", title: "Home", icon: "home" },
      },
      {
        path: "relations", component: relations.RelationsComponent, data: { type: "module", title: "Relations", icon: "dashboard" },
        children: [
          {
            path: "people",
            children: [
              { path: "", component: relations.PeopleComponent, data: { type: "page", title: "People", icon: "people" } },
              { path: "add", component: relations.PersonComponent },
              { path: ":id/edit", component: relations.PersonComponent },
              { path: ":id/overview", component: relations.PersonOverviewComponent },
            ],
          },
        ],
      },
    ],
  },
];
// tslint:enable:object-literal-sort-keys

@NgModule({
  exports: [
    RouterModule,
    SharedModule,
  ],
  imports: [
    RouterModule.forRoot(routes, { useHash: true }),
    SharedModule,
  ],
})
export class AppRoutingModule { }
export const routedComponents: any[] = [
  LoginComponent,
  DashboardComponent,
  MainComponent,
];
