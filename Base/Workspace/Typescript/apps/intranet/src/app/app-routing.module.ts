import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import {
  LoginComponent,
  MainComponent,
  DashboardComponent,
  PersonListComponent,
  PersonOverviewComponent,
  OrganisationListComponent,
  OrganisationOverviewComponent,
  CommunicationEventListComponent,
  RequestForQuoteListComponent,
  RequestForQuoteOverviewComponent,
  ProductQuoteListComponent,
  ProductQuoteOverviewComponent,
  SalesOrderListComponent,
  SalesOrderOverviewComponent,
  SalesInvoiceListComponent,
  SalesInvoiceOverviewComponent,
  GoodListComponent,
  NonUnifiedGoodOverviewComponent,
  PartListComponent,
  NonUnifiedPartOverviewComponent,
  CataloguesListComponent,
  ProductCategoryListComponent,
  SerialisedItemCharacteristicListComponent,
  ProductTypesOverviewComponent,
  SerialisedItemListComponent,
  SerialisedItemOverviewComponent,
  UnifiedGoodListComponent,
  UnifiedGoodOverviewComponent,
  PurchaseOrderListComponent,
  PurchaseOrderOverviewComponent,
  PurchaseInvoiceListComponent,
  ShipmentListComponent,
  CustomerShipmentOverviewComponent,
  PurchaseShipmentOverviewComponent,
  CarrierListComponent,
  WorkEffortListComponent,
  WorkTaskOverviewComponent,
  PositionTypesOverviewComponent,
  PositionTypeRatesOverviewComponent,
  TaskAssignmentListComponent,
  PurchaseInvoiceOverviewComponent,
} from '@allors/angular/material/module';

import { AuthorizationService, ErrorComponent } from '@allors/angular/material/custom';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'error', component: ErrorComponent },
  {
    canActivate: [AuthorizationService],
    path: '',
    component: MainComponent,
    children: [
      {
        path: '',
        component: DashboardComponent,
        pathMatch: 'full',
      },
      {
        path: 'contacts',
        children: [
          { path: 'people', component: PersonListComponent },
          { path: 'person/:id', component: PersonOverviewComponent },
          { path: 'organisations', component: OrganisationListComponent },
          { path: 'organisation/:id', component: OrganisationOverviewComponent },
          { path: 'communicationevents', component: CommunicationEventListComponent },
        ],
      },

      {
        path: 'sales',
        children: [
          { path: 'requestsforquote', component: RequestForQuoteListComponent },
          { path: 'requestforquote/:id', component: RequestForQuoteOverviewComponent },
          { path: 'productquotes', component: ProductQuoteListComponent },
          { path: 'productquote/:id', component: ProductQuoteOverviewComponent },
          { path: 'salesorders', component: SalesOrderListComponent },
          { path: 'salesorder/:id', component: SalesOrderOverviewComponent },
          { path: 'salesinvoices', component: SalesInvoiceListComponent },
          { path: 'salesinvoice/:id', component: SalesInvoiceOverviewComponent },
        ],
      },

      {
        path: 'products',
        children: [
          { path: 'goods', component: GoodListComponent },
          { path: 'nonunifiedgood/:id', component: NonUnifiedGoodOverviewComponent },
          { path: 'parts', component: PartListComponent },
          { path: 'nonunifiedpart/:id', component: NonUnifiedPartOverviewComponent },
          { path: 'catalogues', component: CataloguesListComponent },
          { path: 'productcategories', component: ProductCategoryListComponent },
          { path: 'serialiseditemcharacteristics', component: SerialisedItemCharacteristicListComponent },
          { path: 'producttypes', component: ProductTypesOverviewComponent },
          { path: 'serialiseditems', component: SerialisedItemListComponent },
          { path: 'serialisedItem/:id', component: SerialisedItemOverviewComponent },
          { path: 'unifiedgoods', component: UnifiedGoodListComponent },
          { path: 'unifiedgood/:id', component: UnifiedGoodOverviewComponent },
        ],
      },

      {
        path: 'purchasing',
        children: [
          { path: 'purchaseorders', component: PurchaseOrderListComponent },
          { path: 'purchaseorder/:id', component: PurchaseOrderOverviewComponent },
          { path: 'purchaseinvoices', component: PurchaseInvoiceListComponent },
          { path: 'purchaseinvoice/:id', component: PurchaseInvoiceOverviewComponent },
        ],
      },

      {
        path: 'shipment',
        children: [
          { path: 'shipments', component: ShipmentListComponent },
          { path: 'customershipment/:id', component: CustomerShipmentOverviewComponent },
          { path: 'purchaseshipment/:id', component: PurchaseShipmentOverviewComponent },
          { path: 'carriers', component: CarrierListComponent },
        ],
      },

      {
        path: 'workefforts',
        children: [
          { path: 'workefforts', component: WorkEffortListComponent },
          { path: 'worktask/:id', component: WorkTaskOverviewComponent },
        ],
      },

      {
        path: 'humanresource',
        children: [
          { path: 'positiontypes', component: PositionTypesOverviewComponent },
          { path: 'positiontyperates', component: PositionTypeRatesOverviewComponent },
        ],
      },

      {
        path: 'workflow',
        children: [{ path: 'taskassignments', component: TaskAssignmentListComponent }],
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
