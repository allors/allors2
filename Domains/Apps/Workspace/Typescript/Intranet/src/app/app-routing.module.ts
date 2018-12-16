import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { ids } from '../allors/meta/generated';
import { moduleData, listData, overviewData, editData, addData } from '../allors/angular';

import * as PurchaseInvoiceList from '../allors/material/apps/objects/purchaseinvoice/list/purchaseinvoice-list.module';
import * as PurchaseInvoiceOverview from '../allors/material/apps/objects/purchaseinvoice/overview/purchaseinvoice-overview.module';
import * as PurchaseInvoiceEdit from '../allors/material/apps/objects/purchaseinvoice/edit/purchaseinvoice-edit.module';
import * as PurchaseInvoiceItemEdit from '../allors/material/apps/objects/purchaseinvoiceItem/edit/purchaseinvoiceitem-edit.module';

import * as SalesInvoiceList from '../allors/material/apps/objects/salesinvoice/list/salesinvoice-list.module';
import * as SalesInvoiceOverview from '../allors/material/apps/objects/salesinvoice/overview/salesinvoice-overview.module';
import * as SalesInvoiceEdit from '../allors/material/apps/objects/salesinvoice/edit/salesinvoice-edit.module';
import * as SalesInvoiceItemEdit from '../allors/material/apps/objects/salesinvoiceitem/edit/salesinvoiceitem-edit.module';
import * as RepeatingSalesInvoiceEdit from '../allors/material/apps/objects/repeatingsalesinvoice/edit/repeatingsalesinvoice-edit.module';

import * as IncoTermEdit from '../allors/material/apps/objects/incoterm/edit/incoterm-edit.module';
import * as InvoiceTermEdit from '../allors/material/apps/objects/invoiceterm/edit/invoiceterm-edit.module';
import * as OrderTermEdit from '../allors/material/apps/objects/orderterm/edit/orderterm-edit.module';

import * as PersonList from 'src/allors/material/apps/objects/person/list/person-list.module';
import * as PersonOverview from 'src/allors/material/apps/objects/person/overview/person-overview.module';

import * as OrganisationList from 'src/allors/material/apps/objects/organisation/list/organisation-list.module';
import * as OrganisationOverview from 'src/allors/material/apps/objects/organisation/overview/organisation-overview.module';

import * as CommunicationEventList from 'src/allors/material/apps/objects/communicationevent/list/communicationevent-list.module';

import * as EmailCommunicationOverview from 'src/allors/material/apps/objects/emailcommunication/overview/emailcommunication-overview.module';
import * as FaceToFaceCommunicationOverview from 'src/allors/material/apps/objects/facetofacecommunication/overview/facetofacecommunication-overview.module';
import * as LetterCorrespondenceOverview from 'src/allors/material/apps/objects/lettercorrespondence/overview/lettercorrespondence-overview.module';
import * as PhoneCommunicationOverview from 'src/allors/material/apps/objects/phonecommunication/overview/phonecommunication-overview.module';

import * as EditSerialisedItem from 'src/allors/material/apps/objects/serialiseditem/edit/serialiseditem.module';

import * as EmailAddressEdit from 'src/allors/material/apps/objects/emailaddress/edit/emailaddress-edit.module';
import * as PostalAddressEdit from 'src/allors/material/apps/objects/postaladdress/edit/postaladdress-edit.module';
import * as TelecommunicationsNumberEdit from 'src/allors/material/apps/objects/telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';
import * as EditWebAddress from 'src/allors/material/apps/objects/webaddress/edit/webaddress-edit.module';

import * as InventoryItemTransactionEdit from 'src/allors/material/apps/objects/inventoryitemtransaction/edit/inventoryitemtransaction-edit.module';
import * as NonSerialisedInventoryItemEdit from 'src/allors/material/apps/objects/nonserialisedinventoryitem/edit/nonserialisedinventoryitem-edit.module';

import * as EditBaseprice from 'src/allors/material/apps/objects/baseprice/edit/baseprice.module';
import * as EditSupplierOffering from 'src/allors/material/apps/objects/supplieroffering/edit/supplieroffering.module';
import * as CommunicationEventWorkTask from 'src/allors/material/apps/objects/communicationevent/worktask/communicationevent-worktask.module';
import * as RequestsForQuoteList from 'src/allors/material/apps/objects/requestforquote/list/requestforquote-list.module';
import * as RequestForQuoteOverview from 'src/allors/material/apps/objects/requestforquote/overview/requestforquote-overview.module';
import * as ProductQuotesOverview from 'src/allors/material/apps/objects/productquote/list/productquote-list.module';
import * as ProductQuoteOverview from 'src/allors/material/apps/objects/productquote/overview/productquote-overview.module';
import * as SalesOrdersOverview from 'src/allors/material/apps/objects/salesorder/list/salesorder-list.module';
import * as SalesOrderOverview from 'src/allors/material/apps/objects/salesorder/overview/salesorder-overview.module';
import * as RequestItemEdit from 'src/allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import * as QuoteItemEdit from 'src/allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';
import * as SalesOrderItemEdit from 'src/allors/material/apps/objects/salesorderitem/edit/salesorderitem-edit.module';
import * as GoodList from 'src/allors/material/apps/objects/good/list/good-list.module';
import * as GoodOverview from 'src/allors/material/apps/objects/good/overview/good-overview.module';
import * as PartList from 'src/allors/material/apps/objects/part/list/part-list.module';
import * as PartOverview from 'src/allors/material/apps/objects/part/overview/part-overview.module';
import * as CataloguesOverview from 'src/allors/material/apps/objects/catalogue/list/catalogue-list.module';
import * as CategoriesOverview from 'src/allors/material/apps/objects/productcategory/list/productcategory-list.module';
import * as ProductCharacteristicsOverview from 'src/allors/material/apps/objects/productcharacteristic/list/productcharacteristic-list.module';
import * as ProductTypesOverview from 'src/allors/material/apps/objects/producttype/list/producttype-list.module';
import * as ProductCharacteristic from 'src/allors/material/apps/objects/productcharacteristic/edit/productcharacteristic-edit.module';
import * as ProductType from 'src/allors/material/apps/objects/producttype/edit/producttype-edit.module';
import * as WorkEffortList from 'src/allors/material/apps/objects/workeffort/list/workeffort-list.module';
import * as WorkTaskOverview from 'src/allors/material/apps/objects/worktask/overview/worktask-overview.module';

const modules = [

  CommunicationEventList.CommunicationEventListModule,

  EmailCommunicationOverview.EmailCommunicationOverviewModule,
  FaceToFaceCommunicationOverview.FaceToFaceCommunicationOverviewModule,
  LetterCorrespondenceOverview.LetterCorrespondenceOverviewModule,
  PhoneCommunicationOverview.PhoneCommunicationOverviewModule,

  EditSerialisedItem.SerialisedItemModule,

  EditWebAddress.EditWebAddressModule,

  EmailAddressEdit.EmailAddressdModule,

  IncoTermEdit.IncoTermEditModule,

  InventoryItemTransactionEdit.InventoryItemTransactionEditModule,
  NonSerialisedInventoryItemEdit.NonSerialisedInventoryItemEditModule,

  InvoiceTermEdit.InvoiceTermEditModule,

  OrderTermEdit.OrderTermEditModule,

  OrganisationList.OrganisationListModule,
  OrganisationOverview.OrganisationOverviewModule,

  PersonList.PersonListModule,
  PersonOverview.PersonOverviewModule,

  PostalAddressEdit.PostalAddressModule,

  PurchaseInvoiceList.PurchaseInvoiceListModule,
  PurchaseInvoiceOverview.PurchaseInvoiceOverviewModule,
  PurchaseInvoiceEdit.PurchaseInvoiceEditModule,
  PurchaseInvoiceItemEdit.PurchaseInvoiceItemEditModule,

  RepeatingSalesInvoiceEdit.RepeatingSalesInvoiceEditModule,

  SalesInvoiceList.SalesInvoiceListModule,
  SalesInvoiceOverview.SalesInvoiceOverviewModule,
  SalesInvoiceEdit.SalesInvoiceEditModule,
  SalesInvoiceItemEdit.SalesInvoiceItemEditModule,

  TelecommunicationsNumberEdit.TelecommunicationsNumberModule,

  EditBaseprice.BasepriceModule,
  EditSupplierOffering.SupplierOfferingModule,
  CommunicationEventWorkTask.CommunicationEventWorkTaskModule,
  RequestsForQuoteList.RequestForQuoteListModule,
  RequestForQuoteOverview.RequestForQuoteOverviewModule,
  ProductQuotesOverview.ProductQuotesOverviewModule,
  ProductQuoteOverview.ProductQuoteOverviewModule,
  SalesOrdersOverview.SalesOrdersOverviewModule,
  SalesOrderOverview.SalesOrderOverviewModule,
  RequestItemEdit.RequestItemEditModule,
  QuoteItemEdit.QuoteItemEditModule,
  SalesOrderItemEdit.SalesOrderItemEditModule,
  GoodList.GoodListModule,
  GoodOverview.GoodOverviewModule,
  PartList.PartListModule,
  PartOverview.PartOverviewModule,
  CataloguesOverview.CataloguesOverviewModule,
  CategoriesOverview.ProductCategoriesOverviewModule,
  ProductCharacteristicsOverview.ProductCharacteristicsOverviewModule,
  ProductTypesOverview.ProductTypesOverviewModule,
  ProductCharacteristic.ProductCharacteristicModule,
  ProductType.ProductTypeModule,
  WorkEffortList.WorkEffortListModule,
  WorkTaskOverview.WorkTaskDetailModule,
];

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
        path: 'relations', data: moduleData({ title: 'Contacts', icon: 'contacts' }),
        children: [
          { path: 'people', data: listData({ id: ids.Person, icon: 'people' }), component: PersonList.PersonListComponent, },
          { path: 'person/:id', data: overviewData({ id: ids.Person }), component: PersonOverview.PersonOverviewComponent },
          { path: 'organisations', data: listData({ id: ids.Organisation, icon: 'business' }), component: OrganisationList.OrganisationListComponent },
          { path: 'organisation/:id', data: overviewData({ id: ids.Organisation }), component: OrganisationOverview.OrganisationOverviewComponent },
          { path: 'communicationevents', data: listData({ id: ids.CommunicationEvent, icon: 'share' }), component: CommunicationEventList.CommunicationEventListComponent },
          { path: 'emailcommunication/:id', data: overviewData({ id: ids.EmailCommunication }), component: EmailCommunicationOverview.EmailCommunicationOverviewComponent },
          { path: 'facetofacecommunication/:id', data: overviewData({ id: ids.FaceToFaceCommunication }), component: FaceToFaceCommunicationOverview.FaceToFaceCommunicationOverviewComponent },
          { path: 'lettercorrespondence/:id', data: overviewData({ id: ids.LetterCorrespondence }), component: LetterCorrespondenceOverview.LetterCorrespondenceOverviewComponent },
          { path: 'phonecommunicationevent/:id', data: overviewData({ id: ids.PhoneCommunication }), component: PhoneCommunicationOverview.PhoneCommunicationOverviewComponent },
        ],
      },

      // Orders
      {
        path: 'orders', data: moduleData({ title: 'Sales', icon: 'shopping_cart' }),
        children: [
          { path: 'requests', data: listData({ id: ids.Request, icon: 'share' }), component: RequestsForQuoteList.RequestForQuoteListComponent },
          { path: 'request/:id', data: overviewData({ id: ids.Request }), component: RequestForQuoteOverview.RequestForQuoteOverviewComponent },
          { path: 'productQuotes', data: listData({ id: ids.Quote, icon: 'share' }), component: ProductQuotesOverview.ProductQuotesOverviewComponent },
          { path: 'productQuote/:id', data: overviewData({ id: ids.Quote }), component: ProductQuoteOverview.ProductQuoteOverviewComponent },
          { path: 'salesOrders', data: listData({ id: ids.SalesOrder, icon: 'share' }), component: SalesOrdersOverview.SalesOrdersOverviewComponent },
          { path: 'salesOrder/:id', data: overviewData({ id: ids.SalesOrder }), component: SalesOrderOverview.SalesOrderOverviewComponent },
        ],
      },

      // Catalogues
      {
        path: 'catalogues', data: moduleData({ title: 'Products', icon: 'build' }),
        children: [
          { path: 'goods', data: listData({ id: ids.Good }), component: GoodList.GoodListComponent },
          { path: 'good/:id', data: overviewData({ id: ids.Good }), component: GoodOverview.GoodOverviewComponent },
          { path: 'parts', data: listData({ id: ids.Part }), component: PartList.PartListComponent },
          { path: 'part/:id', data: overviewData({ id: ids.Part }), component: PartOverview.PartOverviewComponent },
          { path: 'catalogues', data: listData({ id: ids.Catalogue }), component: CataloguesOverview.CataloguesOverviewComponent },
          { path: 'categories', data: listData({ id: ids.ProductCategory }), component: CategoriesOverview.ProductCategoriesOverviewComponent },
          { path: 'productCharacteristics', data: listData({ id: ids.SerialisedItemCharacteristicType }), component: ProductCharacteristicsOverview.ProductCharacteristicsOverviewComponent },
          { path: 'productTypes', data: listData({ id: ids.ProductType }), component: ProductTypesOverview.ProductTypesOverviewComponent },
        ],
      },

      // Accounting
      {
        path: 'accounting', data: moduleData({ title: 'Accounting', icon: 'payment' }),
        children: [
          { path: 'purchaseinvoices', data: listData({ id: ids.PurchaseInvoice, icon: 'attach_money' }), component: PurchaseInvoiceList.PurchaseInvoiceListComponent },
          { path: 'purchaseinvoice/:id', data: overviewData({ id: ids.PurchaseInvoice }), component: PurchaseInvoiceOverview.PurchaseInvoiceOverviewComponent },
          { path: 'salesinvoices', data: listData({ id: ids.SalesInvoice, icon: 'attach_money' }), component: SalesInvoiceList.SalesInvoiceListComponent },
          { path: 'salesinvoice/:id', data: overviewData({ id: ids.SalesInvoice }), component: SalesInvoiceOverview.SalesInvoiceOverviewComponent },
        ],
      },

      // WorkEfforts
      {
        path: 'workefforts', data: moduleData({ title: 'Work Efforts', icon: 'work' }),
        children: [
          { path: 'workefforts', data: listData({ id: ids.WorkEffort, icon: 'timer' }), component: WorkEffortList.WorkEffortListComponent },
          { path: 'worktask/:id', data: overviewData({ id: ids.WorkTask }), component: WorkTaskOverview.WorkTaskOverviewComponent },
        ],
      },

      { path: 'inventoryitemtransaction', data: addData({ id: ids.InventoryItemTransaction }), component: InventoryItemTransactionEdit.InventoryItemTransactionEditComponent },
      { path: 'nonserialisedinventoryitem', data: addData({ id: ids.NonSerialisedInventoryItem }), component: NonSerialisedInventoryItemEdit.NonSerialisedInventoryItemEditComponent },

      { path: 'serialiseditem', data: addData({ id: ids.SerialisedItem }), component: EditSerialisedItem.EditSerialisedItemComponent },
      { path: 'serialiseditem/:id', data: editData({ id: ids.SerialisedItem }), component: EditSerialisedItem.EditSerialisedItemComponent },

      { path: 'emailaddress', data: addData({ id: ids.EmailAddress }), component: EmailAddressEdit.EmailAddressEditComponent },
      { path: 'emailaddress/:id', data: editData({ id: ids.EmailAddress }), component: EmailAddressEdit.EmailAddressEditComponent },
      { path: 'postaladdress', data: addData({ id: ids.PostalAddress }), component: PostalAddressEdit.PostalAddressEditComponent },
      { path: 'postaladdress/:id', data: editData({ id: ids.PostalAddress }), component: PostalAddressEdit.PostalAddressEditComponent },
      { path: 'telecommunicationsnumber', data: addData({ id: ids.TelecommunicationsNumber }), component: TelecommunicationsNumberEdit.TelecommunicationsNumberEditComponent },
      { path: 'telecommunicationsnumber/:id', data: editData({ id: ids.TelecommunicationsNumber }), component: TelecommunicationsNumberEdit.TelecommunicationsNumberEditComponent },
      { path: 'webaddress', data: addData({ id: ids.WebAddress }), component: EditWebAddress.EditWebAddressComponent },
      { path: 'webaddress/:id', data: editData({ id: ids.WebAddress }), component: EditWebAddress.EditWebAddressComponent },

      { path: 'baseprice', data: addData({ id: ids.BasePrice }), component: EditBaseprice.EditBasepriceComponent },
      { path: 'baseprice/:id', data: editData({ id: ids.BasePrice }), component: EditBaseprice.EditBasepriceComponent },

      { path: 'supplieroffering', data: addData({ id: ids.SupplierOffering }), component: EditSupplierOffering.EditSupplierOfferingComponent },
      { path: 'supplieroffering/:id', data: editData({ id: ids.SupplierOffering }), component: EditSupplierOffering.EditSupplierOfferingComponent },

      {
        path: 'communicationevent',
        children: [
          { path: ':id/worktask', component: CommunicationEventWorkTask.CommunicationEventWorkTaskComponent },
          { path: ':id/worktask/:roleId', component: CommunicationEventWorkTask.CommunicationEventWorkTaskComponent },
        ],
      },


      {
        path: 'request',
        children: [
          { path: ':id/item', component: RequestItemEdit.RequestItemEditComponent },
          { path: ':id/item/:itemId', component: RequestItemEdit.RequestItemEditComponent },
        ],
      },
      {
        path: 'productCharacteristic',
        children: [
          { path: '', data: addData({ id: ids.SerialisedItemCharacteristicType }), component: ProductCharacteristic.ProductCharacteristicComponent },
          { path: ':id', data: editData({ id: ids.SerialisedItemCharacteristicType }), component: ProductCharacteristic.ProductCharacteristicComponent },
        ],
      },
      {
        path: 'productType',
        children: [
          { path: '', data: addData({ id: ids.ProductType }), component: ProductType.ProductTypeComponent },
          { path: ':id', data: editData({ id: ids.ProductType }), component: ProductType.ProductTypeComponent },
        ],
      },


      {
        path: 'purchaseinvoice',
        children: [
          { path: '', component: PurchaseInvoiceEdit.PurchaseInvoiceEditComponent },
          { path: ':id', component: PurchaseInvoiceEdit.PurchaseInvoiceEditComponent },
          { path: ':id/item', component: PurchaseInvoiceItemEdit.PurchaseInvoiceItemEditComponent },
          { path: ':id/item/:itemId', component: PurchaseInvoiceItemEdit.PurchaseInvoiceItemEditComponent },
        ],
      },
      {
        path: 'salesinvoice',
        children: [
          { path: '', component: SalesInvoiceEdit.SalesInvoiceEditComponent },
          { path: ':id', component: SalesInvoiceEdit.SalesInvoiceEditComponent },
          { path: ':id/item', component: SalesInvoiceEdit.SalesInvoiceEditComponent },
          { path: ':id/repeat', component: RepeatingSalesInvoiceEdit.RepeatingSalesInvoiceEditComponent },
          { path: ':id/repeat/:repeatingInvoiceId', component: RepeatingSalesInvoiceEdit.RepeatingSalesInvoiceEditComponent },
          { path: ':id/item/:itemId', component: SalesInvoiceItemEdit.SalesInvoiceItemEditComponent },

          { path: ':id/incoterm', component: IncoTermEdit.IncoTermEditComponent },
          { path: ':id/incoterm/:termId', component: IncoTermEdit.IncoTermEditComponent },
          { path: ':id/invoiceterm', component: InvoiceTermEdit.InvoiceTermEditComponent },
          { path: ':id/invoiceterm/:termId', component: InvoiceTermEdit.InvoiceTermEditComponent },
          { path: ':id/orderterm', component: OrderTermEdit.OrderTermEditComponent },
          { path: ':id/orderterm/:termId', component: OrderTermEdit.OrderTermEditComponent },
        ],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule, modules],
})
export class AppRoutingModule { }
