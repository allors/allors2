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
      // Relations
      {
        path: "relations", component: relations.OverviewComponent, data: { type: "module", title: "Relations", icon: "dashboard" },
        children: [
          { path: "", component: relations.DashboardComponent },
          { path: "people", component: relations.PeopleOverviewComponent, data: { type: "page", title: "People", icon: "people" }},
          { path: "person/:id", component: relations.PersonOverviewComponent },
          { path: "organisations", component: relations.OrganisationsOverviewComponent, data: { type: "page", title: "Organisations", icon: "business" } },
          { path: "organisation/:id", component: relations.OrganisationOverviewComponent },
          { path: "communicationevents", component: relations.CommunicationEventsOverviewComponent, data: { type: "page", title: "Communications", icon: "share" } },
          { path: "party/:id/communicationevent/:roleId", component: relations.CommunicationEventOverviewComponent },
        ],
      },
      {
        path: "person",
        children: [
          { path: "", component: relations.PersonComponent },
          { path: ":id", component: relations.PersonComponent },
        ],
      },
      {
        children: [
          { path: "", component: relations.OrganisationComponent },
          { path: ":id", component: relations.OrganisationComponent },
          { path: ":id/contact", component: relations.OrganisationContactrelationshipAddComponent },
          { path: ":id/contact/:roleId", component: relations.OrganisationContactrelationshipEditComponent },
        ],
        path: "organisation",
      },
    ],
  },
];
// tslint:enable:object-literal-sort-keys
