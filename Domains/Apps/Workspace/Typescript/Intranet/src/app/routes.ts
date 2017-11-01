import { Routes } from "@angular/router";

import { AllorsService } from "@allors";

import * as orders from "../allors/intranet/apps/orders";
import * as relations from "../allors/intranet/apps/relations";
import * as common from "./common";

// tslint:disable:object-literal-sort-keys
export const routes: Routes = [
  { path: "login", component: common.LoginComponent },
  {
    canActivate: [AllorsService],
    path: "", component: common.MainComponent,
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
        path: "export",
        children: [
          { path: "people", component: relations.PeopleExportComponent },
        ],
      },
      {
        path: "organisation",
        children: [
          { path: "", component: relations.OrganisationComponent },
          { path: ":id", component: relations.OrganisationComponent },
          { path: ":id/contact", component: relations.OrganisationContactrelationshipAddComponent },
          { path: ":id/contact/:roleId", component: relations.OrganisationContactrelationshipEditComponent },
        ],
      },
      {
        path: "party",
        children: [
          { path: ":id/partycontactmechanism/emailaddress", component: relations.PartyContactMechanismEmailAddressAddComponent },
          { path: ":id/partycontactmechanism/emailaddress/:roleId", component: relations.PartyContactMechanismEmailAddressEditComponent },
          { path: ":id/partycontactmechanism/postaladdress", component: relations.PartyContactMechanismPostalAddressAddComponent },
          { path: ":id/partycontactmechanism/postaladdress/:roleId", component: relations.PartyContactMechanismPostalAddressEditComponent },
          { path: ":id/partycontactmechanism/telecommunicationsnumber", component: relations.PartyContactMechanismTelecommunicationsNumberAddComponent },
          { path: ":id/partycontactmechanism/telecommunicationsnumber/:roleId", component: relations.PartyContactMechanismTelecommunicationsNumberEditComponent },
          { path: ":id/partycontactmechanism/webaddress", component: relations.PartyContactMechanismAddWebAddressComponent },
          { path: ":id/partycontactmechanism/webaddres/:roleId", component: relations.PartyContactMechanismEditWebAddressComponent },

          { path: ":id/communicationevent/emailcommunication/:roleId", component: relations.PartyCommunicationEventEmailCommunicationComponent },
          { path: ":id/communicationevent/emailcommunication", component: relations.PartyCommunicationEventEmailCommunicationComponent },
          { path: ":id/communicationevent/facetofacecommunication/:roleId", component: relations.PartyCommunicationEventFaceToFaceCommunicationComponent },
          { path: ":id/communicationevent/facetofacecommunication", component: relations.PartyCommunicationEventFaceToFaceCommunicationComponent },
          { path: ":id/communicationevent/lettercorrespondence/:roleId", component: relations.PartyCommunicationEventLetterCorrespondenceComponent },
          { path: ":id/communicationevent/lettercorrespondence", component: relations.PartyCommunicationEventLetterCorrespondenceComponent },
          { path: ":id/communicationevent/phonecommunication/:roleId", component: relations.PartyCommunicationEventPhoneCommunicationComponent },
          { path: ":id/communicationevent/phonecommunication", component: relations.PartyCommunicationEventPhoneCommunicationComponent },
        ],
      },

      // Orders
      {
        path: "orders",
        component: orders.OverviewComponent, data: { type: "module", title: "Orders", icon: "share" },
        children: [
          { path: "", component: orders.OrdersOverviewComponent },
          { path: "requests", component: orders.RequestsOverviewComponent, data: { type: "page", title: "Requests", icon: "share" } },
          { path: "request/:id", component: orders.RequestOverviewComponent },
          { path: "productQuotes", component: orders.ProductQuotesOverviewComponent, data: { type: "page", title: "Quotes", icon: "share" } },
          { path: "productQuote/:id", component: orders.ProductQuoteOverviewComponent },
          { path: "salesOrders", component: orders.SalesOrdersOverviewComponent, data: { type: "page", title: "Orders", icon: "share" } },
          { path: "salesOrder/:id", component: orders.SalesOrderOverviewComponent },
        ],
      },
      {
        path: "request",
        children: [
          { path: "", component: orders.RequestEditComponent },
          { path: ":id", component: orders.RequestEditComponent },
          { path: ":id/item", component: orders.RequestItemEditComponent },
          { path: ":id/item/:itemId", component: orders.RequestItemEditComponent },
        ],
      },
      {
        path: "productQuote",
        children: [
          { path: "", component: orders.ProductQuoteEditComponent },
          { path: ":id", component: orders.ProductQuoteEditComponent },
          { path: ":id/item", component: orders.QuoteItemEditComponent },
          { path: ":id/item/:itemId", component: orders.QuoteItemEditComponent },
        ],
      },
      {
        path: "salesOrder",
        children: [
          { path: "", component: orders.SalesOrderEditComponent },
          { path: ":id", component: orders.SalesOrderEditComponent },
          { path: ":id/item", component: orders.SalesOrderItemEditComponent },
          { path: ":id/item/:itemId", component: orders.SalesOrderItemEditComponent },
        ],
      },

    ],
  },
];
// tslint:enable:object-literal-sort-keys
