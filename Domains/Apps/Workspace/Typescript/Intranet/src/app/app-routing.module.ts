import { ChangeDetectorRef, NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { LoginComponent } from "./common/auth/login.component";
import { DashboardComponent } from "./common/dashboard/dashboard.component";
import { MainComponent } from "./common/main/main.component";

import { AllorsService } from "@allors";

import * as ar from "@appsIntranet/ar";
import * as catalogues from "@appsIntranet/catalogues";
import * as orders from "@appsIntranet/orders";
import * as relations from "@appsIntranet/relations";
import * as workefforts from "@appsIntranet/workefforts";

import { AR_ROUTING } from "@appsIntranet/ar";
import { CATALOGUES_ROUTING } from "@appsIntranet/catalogues";
import { ORDERS_ROUTING } from "@appsIntranet/orders";
import { RELATIONS_ROUTING } from "@appsIntranet/relations";
import { WORKEFFORTS_ROUTING } from "@appsIntranet/workefforts";

const routes: Routes = [
  { path: "login", component: LoginComponent },
  {
    canActivate: [AllorsService],
    children: [
      {
        component: DashboardComponent, data: { type: "module", title: "Home", icon: "home" },
        path: "",
      },
      {
        children: [
          { path: "", component: ar.ArOverviewComponent },
          { path: "invoices", component: ar.InvoicesOverviewComponent, data: { type: "page", title: "Invoices", icon: "attach_money" } },
          { path: "invoice/:id", component: ar.InvoiceOverviewComponent },
        ],
        component: ar.OverviewComponent, data: { type: "module", title: "Accounts Receivable", icon: "dashboard" },
        path: "ar",
      },
      {
        children: [
          { path: "", component: ar.InvoiceEditComponent },
          { path: ":id", component: ar.InvoiceEditComponent },
          { path: ":id/item", component: ar.InvoiceItemEditComponent },
          { path: ":id/item/:itemId", component: ar.InvoiceItemEditComponent },
        ],
        path: "invoice",
      },
      {
        children: [
          { path: "", component: relations.RelationsOverviewComponent },
          { path: "communicationevents", component: relations.CommunicationEventsOverviewComponent, data: { type: "page", title: "Communications", icon: "share" } },
          { path: "organisations", component: relations.OrganisationsOverviewComponent, data: { type: "page", title: "Organisations", icon: "business" } },
          { path: "organisation/:id", component: relations.OrganisationOverviewComponent },
          { path: "people", component: relations.PeopleOverviewComponent, data: { type: "page", title: "People", icon: "people" } },
          { path: "person/:id", component: relations.PersonOverviewComponent },
          { path: "party/:id/communicationevent/:roleId", component: relations.PartyCommunicationEventOverviewComponent },
        ],
        component: relations.OverviewComponent, data: { type: "module", title: "Relations", icon: "dashboard" },
        path: "relations",
      },
      {
        children: [
          { path: "", component: relations.OrganisationEditComponent },
          { path: ":id", component: relations.OrganisationEditComponent },
          { path: ":id/contact", component: relations.OrganisationContactrelationshipAddComponent },
          { path: ":id/contact/:roleId", component: relations.OrganisationContactrelationshipEditComponent },
        ],
        path: "organisation",
      },
      {
        children: [
          { path: "", component: relations.PersonEditComponent },
          { path: ":id", component: relations.PersonEditComponent },
        ],
        path: "person",
      },
      {
        children: [
          { path: ":id/worktask", component: relations.PartyCommunicationEventAddWorkTaskComponent },
          { path: ":id/worktask/:roleId", component: relations.PartyCommunicationEventAddWorkTaskComponent },
        ],
        path: "communicationevent",
      },
      {
        children: [
          { path: ":id/partycontactmechanism/emailaddress", component: relations.PartyContactMechanismAddEmailAddressComponent },
          { path: ":id/partycontactmechanism/emailaddress/:roleId", component: relations.PartyContactMechanismEditEmailAddressComponent },
          { path: ":id/partycontactmechanism/postaladdress", component: relations.PartyContactMechanismAddPostalAddressComponent },
          { path: ":id/partycontactmechanism/postaladdress/:roleId", component: relations.PartyContactMechanismEditPostalAddressComponent },
          { path: ":id/partycontactmechanism/telecommunicationsnumber", component: relations.PartyContactMechanismAddTelecommunicationsNumberComponent },
          { path: ":id/partycontactmechanism/telecommunicationsnumber/:roleId", component: relations.PartyContactMechanismEditTelecommunicationsNumberComponent },
          { path: ":id/partycontactmechanism/webaddress", component: relations.PartyContactMechanismAddWebAddressComponent },
          { path: ":id/partycontactmechanism/webaddres/:roleId", component: relations.PartyContactMechanismEditWebAddressComponent },

          { path: ":id/communicationevent/emailcommunication/:roleId", component: relations.PartyCommunicationEventEditEmailCommunicationComponent },
          { path: ":id/communicationevent/emailcommunication", component: relations.PartyCommunicationEventEditEmailCommunicationComponent },
          { path: ":id/communicationevent/facetofacecommunication/:roleId", component: relations.PartyCommunicationEventEditFaceToFaceCommunicationComponent },
          { path: ":id/communicationevent/facetofacecommunication", component: relations.PartyCommunicationEventEditFaceToFaceCommunicationComponent },
          { path: ":id/communicationevent/lettercorrespondence/:roleId", component: relations.PartyCommunicationEventEditLetterCorrespondenceComponent },
          { path: ":id/communicationevent/lettercorrespondence", component: relations.PartyCommunicationEventEditLetterCorrespondenceComponent },
          { path: ":id/communicationevent/phonecommunication/:roleId", component: relations.PartyCommunicationEventEditPhoneCommunicationComponent },
          { path: ":id/communicationevent/phonecommunication", component: relations.PartyCommunicationEventEditPhoneCommunicationComponent },
        ],
        path: "party",
      },
      {
        children: [
          { path: "", component: workefforts.WorkEffortsOverviewComponent },
          { path: "worktasks", component: workefforts.WorkTasksOverviewComponent, data: { type: "page", title: "Tasks", icon: "timer" } },
          { path: "worktask/:id", component: workefforts.WorkTaskOverviewComponent },
        ],
        component: workefforts.OverviewComponent, data: { type: "module", title: "Work Efforts", icon: "work" },
        path: "workefforts",
      },
      {
        children: [
          { path: "", component: workefforts.WorkTaskEditComponent },
          { path: ":id", component: workefforts.WorkTaskEditComponent },
        ],
        path: "worktask",
      },
      {
        children: [
          { path: "people", component: relations.ExportPeopleComponent },
        ],
        path: "export",
      },
      {
        children: [
          { path: "", component: orders.OrdersOverviewComponent },
          { path: "requests", component: orders.RequestsOverviewComponent, data: { type: "page", title: "Requests", icon: "share" } },
          { path: "request/:id", component: orders.RequestOverviewComponent },
          { path: "productQuotes", component: orders.ProductQuotesOverviewComponent, data: { type: "page", title: "Quotes", icon: "share" } },
          { path: "productQuote/:id", component: orders.ProductQuoteOverviewComponent },
          { path: "salesOrders", component: orders.SalesOrdersOverviewComponent, data: { type: "page", title: "Orders", icon: "share" } },
          { path: "salesOrder/:id", component: orders.SalesOrderOverviewComponent },
        ],
        component: orders.OverviewComponent, data: { type: "module", title: "Orders", icon: "share" },
        path: "orders",
      },
      {
        children: [
          { path: "", component: orders.RequestEditComponent },
          { path: ":id", component: orders.RequestEditComponent },
          { path: ":id/item", component: orders.RequestItemEditComponent },
          { path: ":id/item/:itemId", component: orders.RequestItemEditComponent },
        ],
        path: "request",
      },
      {
        children: [
          { path: "", component: orders.ProductQuoteEditComponent },
          { path: ":id", component: orders.ProductQuoteEditComponent },
          { path: ":id/item", component: orders.QuoteItemEditComponent },
          { path: ":id/item/:itemId", component: orders.QuoteItemEditComponent },
        ],
        path: "productQuote",
      },
      {
        children: [
          { path: "", component: orders.SalesOrderEditComponent },
          { path: ":id", component: orders.SalesOrderEditComponent },
          { path: ":id/item", component: orders.SalesOrderItemEditComponent },
          { path: ":id/item/:itemId", component: orders.SalesOrderItemEditComponent },
        ],
        path: "salesOrder",
      },
      {
        children: [
          { path: "", component: catalogues.DashboardComponent },
          { path: "catalogues", component: catalogues.CataloguesOverviewComponent, data: { type: "page", title: "Catalogues", icon: "share" } },
          { path: "categories", component: catalogues.CategoriesOverviewComponent, data: { type: "page", title: "Categories", icon: "share" } },
          { path: "goods", component: catalogues.GoodsOverviewComponent, data: { type: "page", title: "Products", icon: "share" } },
          { path: "productCharacteristics", component: catalogues.ProductCharacteristicsOverviewComponent, data: { type: "page", title: "Product Characteristics", icon: "share" } },
          { path: "productTypes", component: catalogues.ProductTypesOverviewComponent, data: { type: "page", title: "Product Types", icon: "share" } },
        ],
        component: catalogues.OverviewComponent, data: { type: "module", title: "Catalogues", icon: "share" },
        path: "catalogues",
      },
      {
        children: [
          { path: "", component: catalogues.CatalogueEditComponent },
          { path: ":id", component: catalogues.CatalogueEditComponent },
        ],
        path: "catalogue",
      },
      {
        children: [
          { path: "", component: catalogues.CategoryEditComponent },
          { path: ":id", component: catalogues.CategoryEditComponent },
        ],
        path: "category",
      },
      {
        children: [
          { path: ":id", component: catalogues.GoodEditComponent },
        ],
        path: "good",
      },
      {
        children: [
          { path: "", component: catalogues.NonSerialisedGoodAddComponent },
        ],
        path: "nonSerialisedGood",
      },
      {
        children: [
          { path: "", component: catalogues.SerialisedGoodAddComponent },
        ],
        path: "serialisedGood",
      },
      {
        children: [
          { path: "", component: catalogues.ProductCharacteristicEditComponent },
          { path: ":id", component: catalogues.ProductCharacteristicEditComponent },
        ],
        path: "productCharacteristic",
      },
      {
        children: [
          { path: "", component: catalogues.ProductTypeEditComponent },
          { path: ":id", component: catalogues.ProductTypeEditComponent },
        ],
        path: "productType",
      },
    ],
    component: MainComponent,
    path: "",
  },
];

@NgModule({
  declarations: [
  ],
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
  DashboardComponent,
  AR_ROUTING,
  RELATIONS_ROUTING,
  WORKEFFORTS_ROUTING,
  ORDERS_ROUTING,
  CATALOGUES_ROUTING,
];
