import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { LoginComponent } from "./common/auth/login.component";
import { MainComponent } from "./common/main/main.component";

import * as relations from "../allors/covalent/custom/relations";

import { AuthorizationService } from "./common/auth/authorization.service";

const routes: Routes = [
  { path: "login", component: LoginComponent },
  { path: "", redirectTo: "/relations", pathMatch: "full" },
  {
    canActivate: [AuthorizationService],
    children: [
      {
        children: [
          {
            component: relations.DashboardComponent,
            path: "",
          }, {
            children: [
              { path: "", component: relations.OrganisationsComponent },
              { path: "add", component: relations.OrganisationComponent },
              { path: ":id/edit", component: relations.OrganisationComponent },
              { path: ":id/overview", component: relations.OrganisationOverviewComponent },
            ],
            path: "organisations",
          }, {
            children: [
              { path: "", component: relations.PeopleComponent },
              { path: "add", component: relations.PersonComponent },
              { path: ":id/edit", component: relations.PersonComponent },
              { path: ":id/overview", component: relations.PersonOverviewComponent },
            ],
            path: "people",
          },
        ],
        component: relations.RelationsComponent,
        path: "relations",
      },
    ],
    component: MainComponent,
    path: "",
  },
];

@NgModule({
  exports: [
    RouterModule,
  ],
  imports: [
    RouterModule.forRoot(routes, { useHash: true }),
  ],
})
export class AppRoutingModule { }
export const routedComponents: any[] = [
  LoginComponent,
  MainComponent,
];
