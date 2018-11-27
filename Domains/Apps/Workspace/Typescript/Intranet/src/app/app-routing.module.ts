import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import { ids } from '../allors/meta/generated';
import { moduleData, pageListData, overviewData, editData, addData } from '../allors/angular';

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

import * as GoodEdit from 'src/allors/material/apps/objects/good/edit/good-edit.module';

import * as PartEdit from 'src/allors/material/apps/objects/part/edit/part-edit.module';
import * as PersonEdit from 'src/allors/material/apps/objects/person/edit/person-edit.module';

import * as OrganisationEdit from 'src/allors/material/apps/objects/organisation/edit/organisation-edit.module';
import * as EditSerialisedItem from 'src/allors/material/apps/objects/serialiseditem/edit/serialiseditem.module';

import * as EmailAddressEdit from 'src/allors/material/apps/objects/emailaddress/edit/emailaddress-edit.module';
import * as PostalAddressEdit from 'src/allors/material/apps/objects/postaladdress/edit/postaladdress-edit.module';
import * as TelecommunicationsNumberEdit from 'src/allors/material/apps/objects/telecommunicationsnumber/edit/telecommunicationsnumber-edit.module';
import * as EditWebAddress from 'src/allors/material/apps/objects/webaddress/edit/webaddress-edit.module';

import * as EditEmailCommunication from 'src/allors/material/apps/objects/emailcommunication/edit/emailcommunication.module';
import * as EditFaceToFaceCommunication from 'src/allors/material/apps/objects/facetofacecommunication/edit/facetofacecommunication.module';
import * as EditLetterCorrespondence from 'src/allors/material/apps/objects/lettercorrespondence/edit/lettercorrespondence.module';
import * as EditPhoneCommunication from 'src/allors/material/apps/objects/phonecommunication/edit/phonecommunication.module';
import * as EditEanIdentification from 'src/allors/material/apps/objects/eanidentification/edit/eanidentification.module';
import * as EditIsbnIdentification from 'src/allors/material/apps/objects/isbnidentification/edit/isbnidentification.module';
import * as EditManufacturerIdentification from 'src/allors/material/apps/objects/manufactureridentification/edit/manufactureridentification.module';
import * as EditPartNumber from 'src/allors/material/apps/objects/partnumber/edit/partnumber.module';
import * as EditProductNumber from 'src/allors/material/apps/objects/productnumber/edit/productnumber.module';
import * as EditSkuIdentification from 'src/allors/material/apps/objects/skuidentification/edit/skuidentification.module';
import * as EditUpcaIdentification from 'src/allors/material/apps/objects/upcaidentification/edit/upcaidentification.module';
import * as EditUpceIdentification from 'src/allors/material/apps/objects/upceidentification/edit/upceidentification.module';
import * as EditBaseprice from 'src/allors/material/apps/objects/baseprice/edit/baseprice.module';
import * as EditSupplierOffering from 'src/allors/material/apps/objects/supplieroffering/edit/supplieroffering.module';
import * as CommunicationEventWorkTask from 'src/allors/material/apps/objects/communicationevent/worktask/communicationevent-worktask.module';
import * as RequestsOverview from 'src/allors/material/apps/objects/request/list/request-list.module';
import * as RequestOverview from 'src/allors/material/apps/objects/request/overview/request-overview.module';
import * as ProductQuotesOverview from 'src/allors/material/apps/objects/productquote/list/productquote-list.module';
import * as ProductQuoteOverview from 'src/allors/material/apps/objects/productquote/overview/productquote-overview.module';
import * as SalesOrdersOverview from 'src/allors/material/apps/objects/salesorder/list/salesorder-list.module';
import * as SalesOrderOverview from 'src/allors/material/apps/objects/salesorder/overview/salesorder-overview.module';
import * as RequestEdit from 'src/allors/material/apps/objects/request/edit/request-edit.module';
import * as RequestItemEdit from 'src/allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import * as ProductQuoteEdit from 'src/allors/material/apps/objects/productquote/edit/productquote-edit.module';
import * as QuoteItemEdit from 'src/allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';
import * as SalesOrderEdit from 'src/allors/material/apps/objects/salesorder/edit/salesorder-edit.module';
import * as SalesOrderItemEdit from 'src/allors/material/apps/objects/salesorderitem/edit/salesorderitem-edit.module';
import * as GoodList from 'src/allors/material/apps/objects/good/list/good-list.module';
import * as GoodOverview from 'src/allors/material/apps/objects/good/overview/good-overview.module';
import * as PartList from 'src/allors/material/apps/objects/part/list/part-list.module';
import * as PartOverview from 'src/allors/material/apps/objects/part/overview/part-overview.module';
import * as CataloguesOverview from 'src/allors/material/apps/objects/catalogue/list/catalogue-list.module';
import * as CategoriesOverview from 'src/allors/material/apps/objects/category/list/category-list.module';
import * as ProductCharacteristicsOverview from 'src/allors/material/apps/objects/productcharacteristic/list/productcharacteristic-list.module';
import * as ProductTypesOverview from 'src/allors/material/apps/objects/producttype/list/producttype-list.module';
import * as Catalogue from 'src/allors/material/apps/objects/catalogue/edit/catalogue-edit.module';
import * as Category from 'src/allors/material/apps/objects/category/edit/category-edit.module';
import * as ProductCharacteristic from 'src/allors/material/apps/objects/productcharacteristic/edit/productcharacteristic-edit.module';
import * as ProductType from 'src/allors/material/apps/objects/producttype/edit/producttype-edit.module';
import * as WorkTaskList from 'src/allors/material/apps/objects/worktask/list/worktasks-list.module';
import * as WorkTaskOverview from 'src/allors/material/apps/objects/worktask/overview/worktask-overview.module';
import * as WorkTaskEdit from 'src/allors/material/apps/objects/worktask/edit/worktask-edit.module';

const modules = [

  CommunicationEventList.CommunicationEventsOverviewModule,

  EmailCommunicationOverview.EmailCommunicationOverviewModule,
  FaceToFaceCommunicationOverview.FaceToFaceCommunicationOverviewModule,
  LetterCorrespondenceOverview.LetterCorrespondenceOverviewModule,
  PhoneCommunicationOverview.PhoneCommunicationOverviewModule,

  EditSerialisedItem.SerialisedItemModule,

  EditWebAddress.EditWebAddressModule,

  EmailAddressEdit.EmailAddressdModule,

  GoodEdit.GoodEditModule,

  IncoTermEdit.IncoTermEditModule,

  InvoiceTermEdit.InvoiceTermEditModule,

  OrderTermEdit.OrderTermEditModule,

  OrganisationList.OrganisationsOverviewModule,
  OrganisationOverview.OrganisationOverviewModule,
  OrganisationEdit.OrganisationModule,

  PartEdit.PartEditModule,

  PersonList.PersonListModule,
  PersonOverview.PersonOverviewModule,
  PersonEdit.PersonModule,

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


  EditEmailCommunication.EmailCommunicationModule,
  EditFaceToFaceCommunication.FaceToFaceCommunicationModule,
  EditLetterCorrespondence.LetterCorrespondenceModule,
  EditPhoneCommunication.PhoneCommunicationModule,
  EditEanIdentification.EanIdentificationModule,
  EditIsbnIdentification.IsbnIdentificationModule,
  EditManufacturerIdentification.ManufacturerIdentificationModule,
  EditPartNumber.PartNumberModule,
  EditProductNumber.ProductNumberModule,
  EditSkuIdentification.SkuIdentificationModule,
  EditUpcaIdentification.UpcaIdentificationModule,
  EditUpceIdentification.UpceIdentificationModule,
  EditBaseprice.BasepriceModule,
  EditSupplierOffering.SupplierOfferingModule,
  CommunicationEventWorkTask.CommunicationEventWorkTaskModule,
  RequestsOverview.RequestsOverviewModule,
  RequestOverview.RequestOverviewModule,
  ProductQuotesOverview.ProductQuotesOverviewModule,
  ProductQuoteOverview.ProductQuoteOverviewModule,
  SalesOrdersOverview.SalesOrdersOverviewModule,
  SalesOrderOverview.SalesOrderOverviewModule,
  RequestEdit.RequestEditModule,
  RequestItemEdit.RequestItemEditModule,
  ProductQuoteEdit.ProductQuoteEditModule,
  QuoteItemEdit.QuoteItemEditModule,
  SalesOrderEdit.SalesOrderEditModule,
  SalesOrderItemEdit.SalesOrderItemEditModule,
  GoodList.GoodListModule,
  GoodOverview.GoodOverviewModule,
  PartList.PartListModule,
  PartOverview.PartOverviewModule,
  CataloguesOverview.CataloguesOverviewModule,
  CategoriesOverview.CategoriesOverviewModule,
  ProductCharacteristicsOverview.ProductCharacteristicsOverviewModule,
  ProductTypesOverview.ProductTypesOverviewModule,
  Catalogue.CatalogueModule,
  Category.CategoryModule,
  ProductCharacteristic.ProductCharacteristicModule,
  ProductType.ProductTypeModule,
  WorkTaskList.WorkTasksOverviewModule,
  WorkTaskOverview.WorkTaskOverviewModule,
  WorkTaskEdit.WorkTaskEditModule,
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
          { path: 'people', data: pageListData({ id: ids.Person, icon: 'people' }), component: PersonList.PersonListComponent, },
          { path: 'person/:id', data: overviewData({ id: ids.Person }), component: PersonOverview.PersonOverviewComponent },
          { path: 'organisations', data: pageListData({ id: ids.Organisation, icon: 'business' }), component: OrganisationList.OrganisationListComponent },
          { path: 'organisation/:id', data: overviewData({ id: ids.Organisation }), component: OrganisationOverview.OrganisationOverviewComponent },
          { path: 'communicationevents', data: pageListData({ id: ids.CommunicationEvent, icon: 'share' }), component: CommunicationEventList.CommunicationEventListComponent },
          { path: 'emailcommunication/:id', data: overviewData({ id: ids.EmailCommunication }), component: EmailCommunicationOverview.EmailCommunicationOverviewComponent },
          { path: 'facetofacecommunication/:id', data: overviewData({ id: ids.FaceToFaceCommunication }), component: FaceToFaceCommunicationOverview.FaceToFaceCommunicationOverviewComponent },
          { path: 'lettercorrespondence/:id', data: overviewData({ id: ids.LetterCorrespondence }), component: LetterCorrespondenceOverview.LetterCorrespondenceComponent },
          { path: 'phonecommunicationevent/:id', data: overviewData({ id: ids.PhoneCommunication }), component: PhoneCommunicationOverview.PhoneCommunicationOverviewComponent },
        ],
      },

      // Orders
      {
        path: 'orders', data: moduleData({ title: 'Sales', icon: 'shopping_cart' }),
        children: [
          { path: 'requests', data: pageListData({ id: ids.Request, icon: 'share' }), component: RequestsOverview.RequestsOverviewComponent },
          { path: 'request/:id', data: overviewData({ id: ids.Request }), component: RequestOverview.RequestOverviewComponent },
          { path: 'productQuotes', data: pageListData({ id: ids.Quote, icon: 'share' }), component: ProductQuotesOverview.ProductQuotesOverviewComponent },
          { path: 'productQuote/:id', data: overviewData({ id: ids.Quote }), component: ProductQuoteOverview.ProductQuoteOverviewComponent },
          { path: 'salesOrders', data: pageListData({ id: ids.SalesOrder, icon: 'share' }), component: SalesOrdersOverview.SalesOrdersOverviewComponent },
          { path: 'salesOrder/:id', data: overviewData({ id: ids.SalesOrder }), component: SalesOrderOverview.SalesOrderOverviewComponent },
        ],
      },

      // Catalogues
      {
        path: 'catalogues', data: moduleData({ title: 'Products', icon: 'build' }),
        children: [
          { path: 'goods', data: pageListData({ id: ids.Good }), component: GoodList.GoodListComponent },
          { path: 'good/:id', data: overviewData({ id: ids.Good }), component: GoodOverview.GoodOverviewComponent },
          { path: 'parts', data: pageListData({ id: ids.Part }), component: PartList.PartListComponent },
          { path: 'part/:id', data: overviewData({ id: ids.Part }), component: PartOverview.PartOverviewComponent },
          { path: 'catalogues', data: pageListData({ id: ids.Catalogue }), component: CataloguesOverview.CataloguesOverviewComponent },
          { path: 'categories', data: pageListData({ id: ids.ProductCategory }), component: CategoriesOverview.CategoriesOverviewComponent },
          { path: 'productCharacteristics', data: pageListData({ id: ids.SerialisedItemCharacteristicType }), component: ProductCharacteristicsOverview.ProductCharacteristicsOverviewComponent },
          { path: 'productTypes', data: pageListData({ id: ids.ProductType }), component: ProductTypesOverview.ProductTypesOverviewComponent },
        ],
      },

      // Accounting
      {
        path: 'accounting', data: moduleData({ title: 'Accounting', icon: 'payment' }),
        children: [
          { path: 'purchaseinvoices', data: pageListData({ id: ids.PurchaseInvoice, icon: 'attach_money' }), component: PurchaseInvoiceList.PurchaseInvoiceListComponent },
          { path: 'purchaseinvoice/:id', data: overviewData({ id: ids.PurchaseInvoice }), component: PurchaseInvoiceOverview.PurchaseInvoiceOverviewComponent },
          { path: 'salesinvoices', data: pageListData({ id: ids.SalesInvoice, icon: 'attach_money' }), component: SalesInvoiceList.SalesInvoiceListComponent },
          { path: 'salesinvoice/:id', data: overviewData({ id: ids.SalesInvoice }), component: SalesInvoiceOverview.SalesInvoiceOverviewComponent },
        ],
      },

      // WorkEfforts
      {
        path: 'workefforts', data: moduleData({ title: 'Work Efforts', icon: 'work' }),
        children: [
          { path: 'worktasks', data: pageListData({ id: ids.WorkTask, icon: 'timer' }), component: WorkTaskList.WorkTaskListComponent },
          { path: 'worktask/:id', data: overviewData({ id: ids.WorkTask }), component: WorkTaskOverview.WorkTaskOverviewComponent },
        ],
      },


      { path: 'good', data: addData({ id: ids.Good }), component: GoodEdit.GoodEditComponent },
      { path: 'good/:id', data: editData({ id: ids.Good }), component: GoodEdit.GoodEditComponent },

      { path: 'part', data: addData({ id: ids.Part }), component: PartEdit.PartEditComponent },
      { path: 'part/:id', data: editData({ id: ids.Part }), component: PartEdit.PartEditComponent },

      { path: 'person', data: addData({ id: ids.Person }), component: PersonEdit.PersonEditComponent },
      { path: 'person/:id', data: editData({ id: ids.Person }), component: PersonEdit.PersonEditComponent },

      { path: 'organisation', data: addData({ id: ids.Organisation }), component: OrganisationEdit.OrganisationEditComponent },
      { path: 'organisation/:id', data: editData({ id: ids.Organisation }), component: OrganisationEdit.OrganisationEditComponent },

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

      { path: 'emailcommunication', data: addData({ id: ids.EmailCommunication }), component: EditEmailCommunication.EditEmailCommunicationComponent },
      { path: 'emailcommunication/:id', data: editData({ id: ids.EmailCommunication }), component: EditEmailCommunication.EditEmailCommunicationComponent },
      { path: 'facetofacecommunication', data: addData({ id: ids.FaceToFaceCommunication }), component: EditFaceToFaceCommunication.EditFaceToFaceCommunicationComponent },
      { path: 'facetofacecommunication/:id', data: editData({ id: ids.FaceToFaceCommunication }), component: EditFaceToFaceCommunication.EditFaceToFaceCommunicationComponent },
      { path: 'lettercorrespondence', data: addData({ id: ids.LetterCorrespondence }), component: EditLetterCorrespondence.EditLetterCorrespondenceComponent },
      { path: 'lettercorrespondence/:id', data: editData({ id: ids.LetterCorrespondence }), component: EditLetterCorrespondence.EditLetterCorrespondenceComponent },
      { path: 'phonecommunication', data: addData({ id: ids.PhoneCommunication }), component: EditPhoneCommunication.EditPhoneCommunicationComponent },
      { path: 'phonecommunication/:id', data: editData({ id: ids.PhoneCommunication }), component: EditPhoneCommunication.EditPhoneCommunicationComponent },

      { path: 'eanidentification', data: addData({ id: ids.EanIdentification }), component: EditEanIdentification.EditEanIdentificationComponent },
      { path: 'eanidentification/:id', data: editData({ id: ids.EanIdentification }), component: EditEanIdentification.EditEanIdentificationComponent },
      { path: 'isbnidentification', data: addData({ id: ids.IsbnIdentification }), component: EditIsbnIdentification.EditIsbnIdentificationComponent },
      { path: 'isbnidentification/:id', data: editData({ id: ids.IsbnIdentification }), component: EditIsbnIdentification.EditIsbnIdentificationComponent },
      { path: 'manufactureridentification', data: addData({ id: ids.ManufacturerIdentification }), component: EditManufacturerIdentification.EditManufacturerIdentificationComponent },
      { path: 'manufactureridentification/:id', data: editData({ id: ids.ManufacturerIdentification }), component: EditManufacturerIdentification.EditManufacturerIdentificationComponent },
      { path: 'partnumber', data: addData({ id: ids.PartNumber }), component: EditPartNumber.EditPartNumberComponent },
      { path: 'partnumber/:id', data: editData({ id: ids.PartNumber }), component: EditPartNumber.EditPartNumberComponent },
      { path: 'productnumber', data: addData({ id: ids.ProductNumber }), component: EditProductNumber.EditProductNumberComponent },
      { path: 'productnumber/:id', data: editData({ id: ids.ProductNumber }), component: EditProductNumber.EditProductNumberComponent },
      { path: 'skuidentification', data: addData({ id: ids.SkuIdentification }), component: EditSkuIdentification.EditSkuIdentificationComponent },
      { path: 'skuidentification/:id', data: editData({ id: ids.SkuIdentification }), component: EditSkuIdentification.EditSkuIdentificationComponent },
      { path: 'upcaidentification', data: addData({ id: ids.UpcaIdentification }), component: EditUpcaIdentification.EditUpcaIdentificationComponent },
      { path: 'upcaidentification/:id', data: editData({ id: ids.UpcaIdentification }), component: EditUpcaIdentification.EditUpcaIdentificationComponent },
      { path: 'upceidentification', data: addData({ id: ids.UpceIdentification }), component: EditUpceIdentification.EditUpceIdentificationComponent },
      { path: 'upceidentification/:id', data: editData({ id: ids.UpceIdentification }), component: EditUpceIdentification.EditUpceIdentificationComponent },

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
          { path: '', component: RequestEdit.RequestEditComponent },
          { path: ':id', component: RequestEdit.RequestEditComponent },
          { path: ':id/item', component: RequestItemEdit.RequestItemEditComponent },
          { path: ':id/item/:itemId', component: RequestItemEdit.RequestItemEditComponent },
        ],
      },
      {
        path: 'productQuote',
        children: [
          { path: '', component: ProductQuoteEdit.ProductQuoteEditComponent },
          { path: ':id', component: ProductQuoteEdit.ProductQuoteEditComponent },
          { path: ':id/item', component: QuoteItemEdit.QuoteItemEditComponent },
          { path: ':id/item/:itemId', component: QuoteItemEdit.QuoteItemEditComponent },
        ],
      },
      {
        path: 'salesOrder',
        children: [
          { path: '', component: SalesOrderEdit.SalesOrderEditComponent },
          { path: ':id', component: SalesOrderEdit.SalesOrderEditComponent },
          { path: ':id/item', component: SalesOrderItemEdit.SalesOrderItemEditComponent },
          { path: ':id/item/:itemId', component: SalesOrderItemEdit.SalesOrderItemEditComponent },
          { path: ':id/incoterm', component: IncoTermEdit.IncoTermEditComponent },
          { path: ':id/incoterm/:termId', component: IncoTermEdit.IncoTermEditComponent },
          { path: ':id/invoiceterm', component: InvoiceTermEdit.InvoiceTermEditComponent },
          { path: ':id/invoiceterm/:termId', component: InvoiceTermEdit.InvoiceTermEditComponent },
          { path: ':id/orderterm', component: OrderTermEdit.OrderTermEditComponent },
          { path: ':id/orderterm/:termId', component: OrderTermEdit.OrderTermEditComponent },
        ],
      },


      {
        path: 'catalogue',
        children: [
          { path: '', data: addData({ id: ids.Catalogue }), component: Catalogue.CatalogueComponent },
          { path: ':id', data: editData({ id: ids.Catalogue }), component: Catalogue.CatalogueComponent },
        ],
      },
      {
        path: 'category',
        children: [
          { path: '', data: addData({ id: ids.ProductCategory }), component: Category.CategoryComponent },
          { path: ':id', data: editData({ id: ids.ProductCategory }), component: Category.CategoryComponent },
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


      { path: 'worktask', data: addData({ id: ids.WorkTask }), component: WorkTaskEdit.WorkTaskEditComponent },
      { path: 'worktask/:id', data: editData({ id: ids.WorkTask }), component: WorkTaskEdit.WorkTaskEditComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule, modules],
})
export class AppRoutingModule { }
