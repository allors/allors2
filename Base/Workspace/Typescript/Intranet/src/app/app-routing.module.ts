import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { ErrorComponent } from './error/error.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

// Objects
import * as CataloguesOverview from '../allors/material/base/objects/catalogue/list/catalogue-list.module';
import * as CarrierList from '../allors/material/base/objects/carrier/list/carrier-list.module';
import * as CategoriesOverview from '../allors/material/base/objects/productcategory/list/productcategory-list.module';
import * as CommunicationEventList from '../allors/material/base/objects/communicationevent/list/communicationevent-list.module';
import * as CommunicationEventWorkTask from '../allors/material/base/objects/communicationevent/worktask/communicationevent-worktask.module';
import * as GoodList from '../allors/material/base/objects/good/list/good-list.module';
import * as InventoryItemTransactionEdit from '../allors/material/base/objects/inventoryitemtransaction/edit/inventoryitemtransaction-edit.module';
import * as NonUnifiedGoodOverview from '../allors/material/base/objects/nonunifiedgood/overview/nonunifiedgood-overview.module';
import * as NonUnifiedPartOverview from '../allors/material/base/objects/nonunifiedpart/overview/nonunifiedpart-overview.module';
import * as OrganisationList from '../allors/material/base/objects/organisation/list/organisation-list.module';
import * as OrganisationOverview from '../allors/material/base/objects/organisation/overview/organisation-overview.module';
import * as PartList from '../allors/material/base/objects/part/list/part-list.module';
import * as PersonList from '../allors/material/base/objects/person/list/person-list.module';
import * as PersonOverview from '../allors/material/base/objects/person/overview/person-overview.module';
import * as PositionTypeList from '../allors/material/base/objects/positiontype/list/positiontype-list.module';
import * as PositionTypeRateList from '../allors/material/base/objects/positiontyperate/list/positiontyperate-list.module';
import * as ProductCharacteristicsOverview from '../allors/material/base/objects/serialiseditemcharacteristictype/list/serialiseditemcharacteristic-list.module';
import * as ProductQuotesOverview from '../allors/material/base/objects/productquote/list/productquote-list.module';
import * as ProductQuoteOverview from '../allors/material/base/objects/productquote/overview/productquote-overview.module';
import * as ProductTypesOverview from '../allors/material/base/objects/producttype/list/producttype-list.module';
import * as PurchaseInvoiceList from '../allors/material/base/objects/purchaseinvoice/list/purchaseinvoice-list.module';
import * as PurchaseInvoiceOverview from '../allors/material/base/objects/purchaseinvoice/overview/purchaseinvoice-overview.module';
import * as PurchaseInvoiceItemEdit from '../allors/material/base/objects/purchaseinvoiceitem/edit/purchaseinvoiceitem-edit.module';
import * as PurchaseOrderList from '../allors/material/base/objects/purchaseorder/list/purchaseorder-list.module';
import * as PurchaseOrderOverview from '../allors/material/base/objects/purchaseorder/overview/purchaseorder-overview.module';
import * as QuoteItemEdit from '../allors/material/base/objects/quoteitem/edit/quoteitem-edit.module';
import * as RepeatingSalesInvoiceEdit from '../allors/material/base/objects/repeatingsalesinvoice/edit/repeatingsalesinvoice-edit.module';
import * as RequestItemEdit from '../allors/material/base/objects/requestitem/edit/requestitem-edit.module';
import * as RequestsForQuoteList from '../allors/material/base/objects/requestforquote/list/requestforquote-list.module';
import * as RequestForQuoteOverview from '../allors/material/base/objects/requestforquote/overview/requestforquote-overview.module';
import * as SalesInvoiceList from '../allors/material/base/objects/salesinvoice/list/salesinvoice-list.module';
import * as SalesInvoiceItemEdit from '../allors/material/base/objects/salesinvoiceitem/edit/salesinvoiceitem-edit.module';
import * as SalesInvoiceOverview from '../allors/material/base/objects/salesinvoice/overview/salesinvoice-overview.module';
import * as SalesOrderList from '../allors/material/base/objects/salesorder/list/salesorder-list.module';
import * as SalesOrderOverview from '../allors/material/base/objects/salesorder/overview/salesorder-overview.module';
import * as SerialisedItemList from '../allors/material/base/objects/serialiseditem/list/serialiseditem-list.module';
import * as SerialisedItemOverview from '../allors/material/base/objects/serialiseditem/overview/serialiseditem-overview.module';
import * as ShipmentList from '../allors/material/base/objects/shipment/list/shipment-list.module';
import * as ShipmentOverview from '../allors/material/base/objects/shipment/overview/shipment-overview.module';
import * as UnifiedGoodList from '../allors/material/base/objects/unifiedgood/list/unifiedgood-list.module';
import * as UnifiedGoodOverview from '../allors/material/base/objects/unifiedgood/overview/unifiedgood-overview.module';
import * as WorkEffortList from '../allors/material/base/objects/workeffort/list/workeffort-list.module';
import * as WorkTaskOverview from '../allors/material/base/objects/worktask/overview/worktask-overview.module';
import * as TaskAssignmentList from '../allors/material/base/objects/taskassignment/list/taskassignment-list.module';

const modules = [

  CarrierList.CarrierListModule,
  CommunicationEventList.CommunicationEventListModule,
  CommunicationEventWorkTask.CommunicationEventWorkTaskModule,
  InventoryItemTransactionEdit.InventoryItemTransactionEditModule,
  OrganisationList.OrganisationListModule,
  OrganisationOverview.OrganisationOverviewModule,
  PersonList.PersonListModule,
  PersonOverview.PersonOverviewModule,
  PurchaseInvoiceList.PurchaseInvoiceListModule,
  PurchaseInvoiceOverview.PurchaseInvoiceOverviewModule,
  PurchaseInvoiceItemEdit.PurchaseInvoiceItemEditModule,
  RepeatingSalesInvoiceEdit.RepeatingSalesInvoiceEditModule,
  RequestsForQuoteList.RequestForQuoteListModule,
  RequestForQuoteOverview.RequestForQuoteOverviewModule,
  ProductQuotesOverview.ProductQuotesListModule,
  ProductQuoteOverview.ProductQuoteOverviewModule,
  RequestItemEdit.RequestItemEditModule,
  QuoteItemEdit.QuoteItemEditModule,
  GoodList.GoodListModule,
  NonUnifiedGoodOverview.NonUnifiedGoodOverviewModule,
  PartList.PartListModule,
  NonUnifiedPartOverview.NonUnifiedPartOverviewModule,
  CataloguesOverview.CataloguesListModule,
  CategoriesOverview.ProductCategoryListModule,
  PositionTypeList.PositionTypesOverviewModule,
  PositionTypeRateList.PositionTypeRatesOverviewModule,
  ProductCharacteristicsOverview.SerialisedItemCharacteristicListModule,
  ProductTypesOverview.ProductTypesOverviewModule,
  PurchaseOrderList.PurchaseOrderListModule,
  PurchaseOrderOverview.PurchaseOrderOverviewModule,
  SalesInvoiceList.SalesInvoiceListModule,
  SalesInvoiceOverview.SalesInvoiceOverviewModule,
  SalesInvoiceItemEdit.SalesInvoiceItemEditModule,
  SalesOrderList.SalesOrderListModule,
  SalesOrderOverview.SalesOrderOverviewModule,
  SerialisedItemList.SerialisedItemListModule,
  SerialisedItemOverview.SerialisedItemOverviewModule,
  ShipmentList.ShipmentListModule,
  ShipmentOverview.ShipmentOverviewModule,
  TaskAssignmentList.TaskListModule,
  UnifiedGoodList.UnifiedGoodListModule,
  UnifiedGoodOverview.UnifiedGoodOverviewModule,
  WorkEffortList.WorkEffortListModule,
  WorkTaskOverview.WorkTaskDetailModule,
];

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'error', component: ErrorComponent },
  {
    canActivate: [AuthorizationService],
    path: '', component: MainComponent,
    children: [
      {
        path: '', component: DashboardComponent, pathMatch: 'full',
      },
      {
        path: 'contacts',
        children: [
          { path: 'people', component: PersonList.PersonListComponent, },
          { path: 'person/:id', component: PersonOverview.PersonOverviewComponent },
          { path: 'organisations', component: OrganisationList.OrganisationListComponent },
          { path: 'organisation/:id', component: OrganisationOverview.OrganisationOverviewComponent },
          { path: 'communicationevents', component: CommunicationEventList.CommunicationEventListComponent },
        ],
      },

      {
        path: 'sales',
        children: [
          { path: 'requestsforquote', component: RequestsForQuoteList.RequestForQuoteListComponent },
          { path: 'requestforquote/:id', component: RequestForQuoteOverview.RequestForQuoteOverviewComponent },
          { path: 'productquotes', component: ProductQuotesOverview.ProductQuoteListComponent },
          { path: 'productquote/:id', component: ProductQuoteOverview.ProductQuoteOverviewComponent },
          { path: 'salesorders', component: SalesOrderList.SalesOrderListComponent },
          { path: 'salesorder/:id', component: SalesOrderOverview.SalesOrderOverviewComponent },
          { path: 'salesinvoices', component: SalesInvoiceList.SalesInvoiceListComponent },
          { path: 'salesinvoice/:id', component: SalesInvoiceOverview.SalesInvoiceOverviewComponent },
        ],
      },

      {
        path: 'products',
        children: [
          { path: 'goods', component: GoodList.GoodListComponent },
          { path: 'nonunifiedgood/:id', component: NonUnifiedGoodOverview.NonUnifiedGoodOverviewComponent },
          { path: 'parts', component: PartList.PartListComponent },
          { path: 'nonunifiedpart/:id', component: NonUnifiedPartOverview.NonUnifiedPartOverviewComponent },
          { path: 'catalogues', component: CataloguesOverview.CataloguesListComponent },
          { path: 'productcategories', component: CategoriesOverview.ProductCategoryListComponent },
          { path: 'serialiseditemcharacteristics', component: ProductCharacteristicsOverview.SerialisedItemCharacteristicListComponent },
          { path: 'producttypes', component: ProductTypesOverview.ProductTypesOverviewComponent },
          { path: 'serialiseditems', component: SerialisedItemList.SerialisedItemListComponent },
          { path: 'serialisedItem/:id', component: SerialisedItemOverview.SerialisedItemOverviewComponent},
          { path: 'unifiedgoods', component: UnifiedGoodList.UnifiedGoodListComponent },
          { path: 'unifiedgood/:id', component: UnifiedGoodOverview.UnifiedGoodOverviewComponent },
        ],
      },

      {
        path: 'purchasing',
        children: [
          { path: 'purchaseorders', component: PurchaseOrderList.PurchaseOrderListComponent },
          { path: 'purchaseorder/:id', component: PurchaseOrderOverview.PurchaseOrderOverviewComponent },
          { path: 'purchaseinvoices', component: PurchaseInvoiceList.PurchaseInvoiceListComponent },
          { path: 'purchaseinvoice/:id', component: PurchaseInvoiceOverview.PurchasInvoiceOverviewComponent },
        ],
      },

      {
        path: 'shipment',
        children: [
          { path: 'shipments', component: ShipmentList.ShipmentListComponent },
          { path: 'shipment/:id', component: ShipmentOverview.ShipmentOverviewComponent },
          { path: 'carriers', component: CarrierList.CarrierListComponent },
        ],
      },

      {
        path: 'workefforts',
        children: [
          { path: 'workefforts', component: WorkEffortList.WorkEffortListComponent },
          { path: 'worktask/:id', component: WorkTaskOverview.WorkTaskOverviewComponent },
        ],
      },

      {
        path: 'humanresource',
        children: [
          { path: 'positiontypes', component: PositionTypeList.PositionTypesOverviewComponent },
          { path: 'positiontyperates', component: PositionTypeRateList.PositionTypeRatesOverviewComponent },
        ],
      },

      {
        path: 'workflow',
        children: [
          { path: 'taskassignments', component: TaskAssignmentList.TaskAssignmentListComponent },
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
