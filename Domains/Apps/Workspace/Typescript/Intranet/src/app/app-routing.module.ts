import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AuthorizationService } from './auth/authorization.service';
import { LoginComponent } from './auth/login.component';
import { MainComponent } from './main/main.component';
import { DashboardComponent } from './dashboard/dashboard.component';

import * as CataloguesOverview from 'src/allors/material/apps/objects/catalogue/list/catalogue-list.module';
import * as CategoriesOverview from 'src/allors/material/apps/objects/productcategory/list/productcategory-list.module';
import * as CommunicationEventList from 'src/allors/material/apps/objects/communicationevent/list/communicationevent-list.module';
import * as CommunicationEventWorkTask from 'src/allors/material/apps/objects/communicationevent/worktask/communicationevent-worktask.module';
import * as GoodList from 'src/allors/material/apps/objects/good/list/good-list.module';
import * as InventoryItemTransactionEdit from 'src/allors/material/apps/objects/inventoryitemtransaction/edit/inventoryitemtransaction-edit.module';
import * as NonUnifiedGoodOverview from 'src/allors/material/apps/objects/nonunifiedgood/overview/nonunifiedgood-overview.module';
import * as NonUnifiedPartOverview from 'src/allors/material/apps/objects/nonunifiedpart/overview/nonunifiedpart-overview.module';
import * as OrganisationList from 'src/allors/material/apps/objects/organisation/list/organisation-list.module';
import * as OrganisationOverview from 'src/allors/material/apps/objects/organisation/overview/organisation-overview.module';
import * as PartList from 'src/allors/material/apps/objects/part/list/part-list.module';
import * as PersonList from 'src/allors/material/apps/objects/person/list/person-list.module';
import * as PersonOverview from 'src/allors/material/apps/objects/person/overview/person-overview.module';
import * as PositionTypeList from 'src/allors/material/apps/objects/positiontype/list/positiontype-list.module';
import * as PositionTypeRateList from 'src/allors/material/apps/objects/positiontyperate/list/positiontyperate-list.module';
import * as ProductCharacteristicsOverview from 'src/allors/material/apps/objects/serialiseditemcharacteristictype/list/serialiseditemcharacteristic-list.module';
import * as ProductQuotesOverview from 'src/allors/material/apps/objects/productquote/list/productquote-list.module';
import * as ProductQuoteOverview from 'src/allors/material/apps/objects/productquote/overview/productquote-overview.module';
import * as ProductTypesOverview from 'src/allors/material/apps/objects/producttype/list/producttype-list.module';
import * as PurchaseInvoiceList from '../allors/material/apps/objects/purchaseinvoice/list/purchaseinvoice-list.module';
import * as PurchaseInvoiceOverview from '../allors/material/apps/objects/purchaseinvoice/overview/purchaseinvoice-overview.module';
import * as PurchaseInvoiceItemEdit from '../allors/material/apps/objects/purchaseinvoiceItem/edit/purchaseinvoiceitem-edit.module';
import * as PurchaseOrderList from 'src/allors/material/apps/objects/purchaseorder/list/purchaseorder-list.module';
import * as PurchaseOrderOverview from 'src/allors/material/apps/objects/purchaseorder/overview/purchaseorder-overview.module';
import * as QuoteItemEdit from 'src/allors/material/apps/objects/quoteitem/edit/quoteitem-edit.module';
import * as RepeatingSalesInvoiceEdit from '../allors/material/apps/objects/repeatingsalesinvoice/edit/repeatingsalesinvoice-edit.module';
import * as RequestItemEdit from 'src/allors/material/apps/objects/requestitem/edit/requestitem-edit.module';
import * as RequestsForQuoteList from 'src/allors/material/apps/objects/requestforquote/list/requestforquote-list.module';
import * as RequestForQuoteOverview from 'src/allors/material/apps/objects/requestforquote/overview/requestforquote-overview.module';
import * as SalesInvoiceList from '../allors/material/apps/objects/salesinvoice/list/salesinvoice-list.module';
import * as SalesInvoiceItemEdit from '../allors/material/apps/objects/salesinvoiceitem/edit/salesinvoiceitem-edit.module';
import * as SalesInvoiceOverview from '../allors/material/apps/objects/salesinvoice/overview/salesinvoice-overview.module';
import * as SalesOrderList from 'src/allors/material/apps/objects/salesorder/list/salesorder-list.module';
import * as SalesOrderOverview from 'src/allors/material/apps/objects/salesorder/overview/salesorder-overview.module';
import * as SerialisedItemList from 'src/allors/material/apps/objects/serialiseditem/list/serialiseditem-list.module';
import * as SerialisedItemOverview from 'src/allors/material/apps/objects/serialiseditem/overview/serialiseditem-overview.module';
import * as UnifiedGoodList from 'src/allors/material/apps/objects/unifiedgood/list/unifiedgood-list.module';
import * as UnifiedGoodOverview from 'src/allors/material/apps/objects/unifiedgood/overview/unifiedgood-overview.module';
import * as WorkEffortList from 'src/allors/material/apps/objects/workeffort/list/workeffort-list.module';
import * as WorkTaskOverview from 'src/allors/material/apps/objects/worktask/overview/worktask-overview.module';
import * as TaskAssignmentList from 'src/allors/material/apps/objects/taskassignment/list/taskassignment-list.module';

const modules = [

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
  ProductQuotesOverview.ProductQuotesOverviewModule,
  ProductQuoteOverview.ProductQuoteOverviewModule,
  RequestItemEdit.RequestItemEditModule,
  QuoteItemEdit.QuoteItemEditModule,
  GoodList.GoodListModule,
  NonUnifiedGoodOverview.NonUnifiedGoodOverviewModule,
  PartList.PartListModule,
  NonUnifiedPartOverview.NonUnifiedPartOverviewModule,
  CataloguesOverview.CataloguesOverviewModule,
  CategoriesOverview.ProductCategoriesOverviewModule,
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
  UnifiedGoodList.UnifiedGoodListModule,
  UnifiedGoodOverview.UnifiedGoodOverviewModule,
  WorkEffortList.WorkEffortListModule,
  WorkTaskOverview.WorkTaskDetailModule,

  TaskAssignmentList.TaskListModule,
];

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  {
    canActivate: [AuthorizationService],
    path: '', component: MainComponent,
    children: [
      {
        path: '', component: DashboardComponent,
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
          { path: 'productquotes', component: ProductQuotesOverview.ProductQuotesOverviewComponent },
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
          { path: 'catalogues', component: CataloguesOverview.CataloguesOverviewComponent },
          { path: 'productcategories', component: CategoriesOverview.ProductCategoriesOverviewComponent },
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
