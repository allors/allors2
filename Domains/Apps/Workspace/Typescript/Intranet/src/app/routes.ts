import { Routes } from "@angular/router";

import { AllorsService } from "@allors";

import * as ar from "../allors/intranet/apps/ar";
import * as catalogues from "../allors/intranet/apps/catalogues";
import * as orders from "../allors/intranet/apps/orders";
import * as relations from "../allors/intranet/apps/relations";
import * as workefforts from "../allors/intranet/apps/workefforts";
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
      {
        path: "communicationevent",
        children: [
          { path: ":id/worktask", component: relations.PartyCommunicationEventWorkTaskComponent },
          { path: ":id/worktask/:roleId", component: relations.PartyCommunicationEventWorkTaskComponent },
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

      // Catalogues
      {
        path: "catalogues", component: catalogues.OverviewComponent, data: { type: "module", title: "Catalogues", icon: "share" },
        children: [
          { path: "", component: catalogues.DashboardComponent },
          { path: "catalogues", component: catalogues.CataloguesOverviewComponent, data: { type: "page", title: "Catalogues", icon: "share" } },
          { path: "categories", component: catalogues.CategoriesOverviewComponent, data: { type: "page", title: "Categories", icon: "share" } },
          { path: "goods", component: catalogues.GoodsOverviewComponent, data: { type: "page", title: "Products", icon: "share" } },
          { path: "productCharacteristics", component: catalogues.ProductCharacteristicsOverviewComponent, data: { type: "page", title: "Product Characteristics", icon: "share" } },
          { path: "productTypes", component: catalogues.ProductTypesOverviewComponent, data: { type: "page", title: "Product Types", icon: "share" } },
        ],
      },
      {
        path: "catalogue",
        children: [
          { path: "", component: catalogues.CatalogueEditComponent },
          { path: ":id", component: catalogues.CatalogueEditComponent },
        ],
      },
      {
        path: "category",
        children: [
          { path: "", component: catalogues.CategoryEditComponent },
          { path: ":id", component: catalogues.CategoryEditComponent },
        ],
      },
      {
        path: "good",
        children: [
          { path: ":id", component: catalogues.GoodEditComponent },
        ],
      },
      {
        path: "nonSerialisedGood",
        children: [
          { path: "", component: catalogues.NonSerialisedGoodAddComponent },
        ],
      },
      {
        path: "serialisedGood",
        children: [
          { path: "", component: catalogues.SerialisedGoodAddComponent },
        ],
      },
      {
        path: "productCharacteristic",
        children: [
          { path: "", component: catalogues.ProductCharacteristicEditComponent },
          { path: ":id", component: catalogues.ProductCharacteristicEditComponent },
        ],
      },
      {
        path: "productType",
        children: [
          { path: "", component: catalogues.ProductTypeEditComponent },
          { path: ":id", component: catalogues.ProductTypeEditComponent },
        ],
      },

      // AR
      {
        path: "ar", component: ar.OverviewComponent, data: { type: "module", title: "Accounts Receivable", icon: "dashboard" },
        children: [
          { path: "", component: ar.DashboardComponent },
          { path: "invoices", component: ar.InvoicesOverviewComponent, data: { type: "page", title: "Invoices", icon: "attach_money" } },
          { path: "invoice/:id", component: ar.InvoiceOverviewComponent },
        ],
      },
      {
        path: "invoice",
        children: [
          { path: "", component: ar.InvoiceComponent },
          { path: ":id", component: ar.InvoiceComponent },
          { path: ":id/item", component: ar.InvoiceInvoiceItemComponent },
          { path: ":id/item/:itemId", component: ar.InvoiceInvoiceItemComponent },
        ],
      },

      // WorkEfforts
      {
        path: "workefforts", component: workefforts.OverviewComponent, data: { type: "module", title: "Work Efforts", icon: "work" },
        children: [
          { path: "", component: workefforts.WorkEffortsOverviewComponent },
          { path: "worktasks", component: workefforts.WorkTasksOverviewComponent, data: { type: "page", title: "Tasks", icon: "timer" } },
          { path: "worktask/:id", component: workefforts.WorkTaskOverviewComponent },
        ],
      },
      {
        path: "worktask",
        children: [
          { path: "", component: workefforts.WorkTaskEditComponent },
          { path: ":id", component: workefforts.WorkTaskEditComponent },
        ],
      },

    ],
  },
];
// tslint:enable:object-literal-sort-keys
