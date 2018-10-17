import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import * as ap from '../allors/material/apps/components/accountspayable';
import * as ar from '../allors/material/apps/components/accountsreceivable';
import * as catalogues from '../allors/material/apps/components/catalogues';
import * as orders from '../allors/material/apps/components/orders';
import * as relations from '../allors/material/apps/components/relations';
import * as workefforts from '../allors/material/apps/components/workefforts';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { ids } from '../allors/meta/generated';
import { moduleData, pageListData, overviewData, editData, addData } from '../allors/angular';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    canActivate: [AuthorizationService],
    path: '', component: MainComponent,
    children: [
      {
        path: '', data: moduleData({ title: 'Home', icon: 'home' }), component: DashboardComponent,
      },
      // Relations
      {
        path: 'relations', data: moduleData({ title: 'Relations', icon: 'dashboard' }),
        children: [
          { path: 'people', data: pageListData({ id: ids.Person, icon: 'people' }), component: relations.PersonListComponent, },
          { path: 'person/:id', data: overviewData({ id: ids.Person }), component: relations.PersonOverviewComponent },
          { path: 'organisations', data: pageListData({ id: ids.Organisation, icon: 'business' }), component: relations.OrganisationListComponent },
          { path: 'organisation/:id', data: overviewData({ id: ids.Organisation }), component: relations.OrganisationOverviewComponent },
          { path: 'communicationevents', data: pageListData({ id: ids.CommunicationEvent, icon: 'share' }), component: relations.CommunicationEventsOverviewComponent },
          { path: 'party/:id/communicationevent/:roleId', data: overviewData({ id: ids.CommunicationEvent }), component: relations.CommunicationEventOverviewComponent },
        ],
      },
      { path: 'person', data: addData({ id: ids.Person }), component: relations.PersonEditComponent },
      { path: 'person/:id', data: editData({ id: ids.Person }), component: relations.PersonEditComponent },
      { path: 'organisation', data: addData({ id: ids.Person }), component: relations.OrganisationEditComponent },
      { path: 'organisation/:id', data: editData({ id: ids.Person }), component: relations.OrganisationEditComponent },
      { path: 'emailaddress', data: addData({ id: ids.EmailAddress }), component: relations.EmailAddressEditComponent },
      { path: 'emailaddress/:id', data: editData({ id: ids.EmailAddress }), component: relations.EmailAddressEditComponent },

      {
        path: 'party',
        children: [
          { path: ':id/partycontactmechanism/postaladdress', component: relations.PartyContactMechanismPostalAddressAddComponent },
          { path: ':id/partycontactmechanism/postaladdress/:roleId', component: relations.PartyContactMechanismPostalAddressEditComponent },
          { path: ':id/partycontactmechanism/telecommunicationsnumber', component: relations.PartyContactMechanismTelecommunicationsNumberAddComponent },
          { path: ':id/partycontactmechanism/telecommunicationsnumber/:roleId', component: relations.PartyContactMechanismTelecommunicationsNumberEditComponent },
          { path: ':id/partycontactmechanism/webaddress', component: relations.PartyContactMechanismAddWebAddressComponent },
          { path: ':id/partycontactmechanism/webaddress/:roleId', component: relations.PartyContactMechanismEditWebAddressComponent },

          { path: ':id/communicationevent/emailcommunication/:roleId', component: relations.PartyCommunicationEventEmailCommunicationComponent },
          { path: ':id/communicationevent/emailcommunication', component: relations.PartyCommunicationEventEmailCommunicationComponent },
          { path: ':id/communicationevent/facetofacecommunication/:roleId', component: relations.PartyCommunicationEventFaceToFaceCommunicationComponent },
          { path: ':id/communicationevent/facetofacecommunication', component: relations.PartyCommunicationEventFaceToFaceCommunicationComponent },
          { path: ':id/communicationevent/lettercorrespondence/:roleId', component: relations.PartyCommunicationEventLetterCorrespondenceComponent },
          { path: ':id/communicationevent/lettercorrespondence', component: relations.PartyCommunicationEventLetterCorrespondenceComponent },
          { path: ':id/communicationevent/phonecommunication/:roleId', component: relations.PartyCommunicationEventPhoneCommunicationComponent },
          { path: ':id/communicationevent/phonecommunication', component: relations.PartyCommunicationEventPhoneCommunicationComponent },
        ],
      },
      {
        path: 'communicationevent',
        children: [
          { path: ':id/worktask', component: relations.CommunicationEventWorkTaskComponent },
          { path: ':id/worktask/:roleId', component: relations.CommunicationEventWorkTaskComponent },
        ],
      },

      // Orders
      {
        path: 'orders', data: moduleData({ title: 'Orders', icon: 'share' }),
        children: [
          { path: 'requests', data: pageListData({ id: ids.Request, icon: 'share' }), component: orders.RequestsOverviewComponent },
          { path: 'request/:id', component: orders.RequestOverviewComponent },
          { path: 'productQuotes', data: pageListData({ id: ids.Quote, icon: 'share' }), component: orders.ProductQuotesOverviewComponent },
          { path: 'productQuote/:id', component: orders.ProductQuoteOverviewComponent },
          { path: 'salesOrders', data: pageListData({ id: ids.SalesOrder, icon: 'share' }), component: orders.SalesOrdersOverviewComponent },
          { path: 'salesOrder/:id', component: orders.SalesOrderOverviewComponent },
        ],
      },
      {
        path: 'request',
        children: [
          { path: '', component: orders.RequestEditComponent },
          { path: ':id', component: orders.RequestEditComponent },
          { path: ':id/item', component: orders.RequestItemEditComponent },
          { path: ':id/item/:itemId', component: orders.RequestItemEditComponent },
        ],
      },
      {
        path: 'productQuote',
        children: [
          { path: '', component: orders.ProductQuoteEditComponent },
          { path: ':id', component: orders.ProductQuoteEditComponent },
          { path: ':id/item', component: orders.QuoteItemEditComponent },
          { path: ':id/item/:itemId', component: orders.QuoteItemEditComponent },
        ],
      },
      {
        path: 'salesOrder',
        children: [
          { path: '', component: orders.SalesOrderEditComponent },
          { path: ':id', component: orders.SalesOrderEditComponent },
          { path: ':id/item', component: orders.SalesOrderItemEditComponent },
          { path: ':id/item/:itemId', component: orders.SalesOrderItemEditComponent },
          { path: ':id/incoterm', component: orders.IncoTermEditComponent },
          { path: ':id/incoterm/:termId', component: orders.IncoTermEditComponent },
          { path: ':id/invoiceterm', component: orders.InvoiceTermEditComponent },
          { path: ':id/invoiceterm/:termId', component: orders.InvoiceTermEditComponent },
          { path: ':id/orderterm', component: orders.OrderTermEditComponent },
          { path: ':id/orderterm/:termId', component: orders.OrderTermEditComponent },
        ],
      },

      // Catalogues
      {
        path: 'catalogues', data: moduleData({ title: 'Catalogues', icon: 'share' }),
        children: [
          { path: 'catalogues', data: pageListData({ id: ids.Catalogue, icon: 'share' }), component: catalogues.CataloguesOverviewComponent },
          { path: 'categories', data: pageListData({ id: ids.ProductCategory, icon: 'share' }), component: catalogues.CategoriesOverviewComponent },
          { path: 'goods', data: pageListData({ id: ids.Product, icon: 'share' }), component: catalogues.GoodsOverviewComponent },
          { path: 'productCharacteristics', data: pageListData({ id: ids.SerialisedInventoryItemCharacteristicType, icon: 'share' }), component: catalogues.ProductCharacteristicsOverviewComponent },
          { path: 'productTypes', data: pageListData({ id: ids.ProductType, icon: 'share' }), component: catalogues.ProductTypesOverviewComponent },
        ],
      },
      {
        path: 'catalogue',
        children: [
          { path: '', component: catalogues.CatalogueComponent },
          { path: ':id', component: catalogues.CatalogueComponent },
        ],
      },
      {
        path: 'category',
        children: [
          { path: '', component: catalogues.CategoryComponent },
          { path: ':id', component: catalogues.CategoryComponent },
        ],
      },
      {
        path: 'nonSerialisedGood',
        children: [
          { path: ':id', component: catalogues.NonSerialisedGoodComponent },
          { path: '', component: catalogues.NonSerialisedGoodComponent },
        ],
      },
      {
        path: 'serialisedGood',
        children: [
          { path: ':id', component: catalogues.SerialisedGoodComponent },
          { path: '', component: catalogues.SerialisedGoodComponent },
        ],
      },
      {
        path: 'productCharacteristic',
        children: [
          { path: '', component: catalogues.ProductCharacteristicComponent },
          { path: ':id', component: catalogues.ProductCharacteristicComponent },
        ],
      },
      {
        path: 'productType',
        children: [
          { path: '', component: catalogues.ProductTypeComponent },
          { path: ':id', component: catalogues.ProductTypeComponent },
        ],
      },

      // Accounts Payable
      {
        path: 'accountspayable', data: moduleData({ title: 'Accounts Payable', icon: 'dashboard' }),
        children: [
          { path: 'invoices', data: pageListData({ id: ids.PurchaseInvoice, icon: 'attach_money' }), component: ap.InvoicesOverviewComponent },
          { path: 'invoice/:id', component: ap.InvoiceOverviewComponent },
        ],
      },
      {
        path: 'purchaseinvoice',
        children: [
          { path: '', component: ap.InvoiceComponent },
          { path: ':id', component: ap.InvoiceComponent },
          { path: ':id/item', component: ap.InvoiceItemEditComponent },
          { path: ':id/item/:itemId', component: ap.InvoiceItemEditComponent },
        ],
      },

      // Accounts Receivable
      {
        path: 'accountsreceivable', data: moduleData({ title: 'Accounts Receivable', icon: 'dashboard' }),
        children: [
          { path: 'invoices', data: pageListData({ id: ids.SalesInvoice, icon: 'attach_money' }), component: ar.InvoicesOverviewComponent },
          { path: 'invoice/:id', component: ar.InvoiceOverviewComponent },
        ],
      },
      {
        path: 'salesinvoice',
        children: [
          { path: '', component: ar.InvoiceComponent },
          { path: ':id', component: ar.InvoiceComponent },
          { path: ':id/item', component: ar.InvoiceItemEditComponent },
          { path: ':id/repeat', component: ar.RepeatingInvoiceEditComponent },
          { path: ':id/repeat/:repeatingInvoiceId', component: ar.RepeatingInvoiceEditComponent },
          { path: ':id/item/:itemId', component: ar.InvoiceItemEditComponent },
          { path: ':id/incoterm', component: ar.IncoTermEditComponent },
          { path: ':id/incoterm/:termId', component: ar.IncoTermEditComponent },
          { path: ':id/invoiceterm', component: ar.InvoiceTermEditComponent },
          { path: ':id/invoiceterm/:termId', component: ar.InvoiceTermEditComponent },
          { path: ':id/orderterm', component: ar.OrderTermEditComponent },
          { path: ':id/orderterm/:termId', component: ar.OrderTermEditComponent },
        ],
      },

      // WorkEfforts
      {
        path: 'workefforts', data: moduleData({ title: 'Work Efforts', icon: 'work' }),
        children: [
          { path: '', component: workefforts.WorkEffortsOverviewComponent },
          { path: 'worktasks', data: pageListData({ id: ids.Task, icon: 'timer' }), component: workefforts.WorkTasksOverviewComponent },
          { path: 'worktask/:id', component: workefforts.WorkTaskOverviewComponent },
        ],
      },
      {
        path: 'worktask',
        children: [
          { path: '', component: workefforts.WorkTaskEditComponent },
          { path: ':id', component: workefforts.WorkTaskEditComponent },
        ],
      },

    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
