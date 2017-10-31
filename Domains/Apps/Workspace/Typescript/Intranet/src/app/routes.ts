import { Routes } from "@angular/router";

import { AllorsService } from "@allors";

import * as relations from "../allors/intranet/apps/relations";
import * as common from "./common";

// tslint:disable:object-literal-sort-keys
export const routes: Routes = [
  { path: "login", component: common.LoginComponent },
  {
    canActivate: [AllorsService],
    path: "",
    component: common.MainComponent,
    children: [
      {
        path: "", component: common.DashboardComponent, data: { type: "module", title: "Home", icon: "home" },
      },
      {
        path: "relations", component: relations.RelationsComponent, data: { type: "module", title: "Relations", icon: "dashboard" },
        children: [
          {
            path: "", component: relations.DashboardComponent,
          },
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
