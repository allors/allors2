// Overview
import { OverviewComponent } from "./overview.component";

import { OrdersOverviewComponent } from "./overview/orders/ordersOverview.component";
import { ProductQuoteOverviewComponent } from "./overview/productQuote/productQuoteOverview.component";
import { ProductQuotesOverviewComponent } from "./overview/productQuotes/productQuotesOverview.component";
import { RequestOverviewComponent } from "./overview/request/requestOverview.component";
import { RequestsOverviewComponent } from "./overview/requests/requestsOverview.component";
import { SalesOrderOverviewComponent } from "./overview/salesOrder/salesOrderOverview.component";
import { SalesOrdersOverviewComponent } from "./overview/salesOrders/salesOrdersOverview.component";

import { ProductQuoteEditComponent } from "./productQuote/edit.component";
import { QuoteItemEditComponent } from "./quoteItem/edit.component";
import { RequestEditComponent } from "./request/edit.component";
import { RequestItemEditComponent } from "./requestItem/edit.component";
import { SalesOrderEditComponent } from "./salesOrder/edit.component";
import { SalesOrderItemEditComponent } from "./salesOrderItem/edit.component";

export const ORDERS: any[] = [
];

export const ORDERS_ROUTING: any[] = [
  // Overview
  OverviewComponent,

  OrdersOverviewComponent,
  ProductQuoteOverviewComponent,
  ProductQuotesOverviewComponent,
  QuoteItemEditComponent,
  RequestsOverviewComponent,
  RequestOverviewComponent,
  SalesOrderOverviewComponent,
  SalesOrdersOverviewComponent,

  ProductQuoteEditComponent,
  RequestEditComponent,
  RequestItemEditComponent,
  SalesOrderEditComponent,
  SalesOrderItemEditComponent,
];

export {
  // Overview
  OverviewComponent,
  OrdersOverviewComponent,
  ProductQuoteOverviewComponent,
  ProductQuotesOverviewComponent,
  RequestsOverviewComponent,
  RequestOverviewComponent,
  SalesOrderOverviewComponent,
  SalesOrdersOverviewComponent,

  ProductQuoteEditComponent,
  RequestEditComponent,
  QuoteItemEditComponent,
  RequestItemEditComponent,
  SalesOrderEditComponent,
  SalesOrderItemEditComponent,
};
