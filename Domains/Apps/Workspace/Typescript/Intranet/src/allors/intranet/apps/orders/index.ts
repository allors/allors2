export * from "./overview.module";

export * from "./overview/orders/ordersOverview.module";
export * from "./overview/productQuote/productQuoteOverview.module";
export * from "./overview/productQuotes/productQuotesOverview.module";
export * from "./overview/request/requestOverview.module";
export * from "./overview/requests/requestsOverview.module";
export * from "./overview/salesOrder/salesOrderOverview.module";
export * from "./overview/salesOrders/salesOrdersOverview.module";

export * from "./productQuote/edit.module";
export * from "./quoteItem/edit.module";
export * from "./request/edit.module";
export * from "./requestItem/edit.module";
export * from "./salesOrder/edit.module";
export * from "./salesOrderItem/edit.module";

import { OverviewModule } from "./overview.module";

import { OrdersOverviewModule } from "./overview/orders/ordersOverview.module";
import { ProductQuoteOverviewModule } from "./overview/productQuote/productQuoteOverview.module";
import { ProductQuotesOverviewModule } from "./overview/productQuotes/productQuotesOverview.module";
import { RequestOverviewModule } from "./overview/request/requestOverview.module";
import { RequestsOverviewModule } from "./overview/requests/requestsOverview.module";
import { SalesOrderOverviewModule } from "./overview/salesOrder/salesOrderOverview.module";
import { SalesOrdersOverviewModule } from "./overview/salesOrders/salesOrdersOverview.module";

import { ProductQuoteEditModule } from "./productQuote/edit.module";
import { QuoteItemEditModule } from "./quoteItem/edit.module";
import { RequestEditModule } from "./request/edit.module";
import { RequestItemEditModule } from "./requestItem/edit.module";
import { SalesOrderEditModule } from "./salesOrder/edit.module";
import { SalesOrderItemEditModule } from "./salesOrderItem/edit.module";

export const Modules = [
  // Overview
  OverviewModule,

  OrdersOverviewModule,
  ProductQuoteOverviewModule,
  ProductQuotesOverviewModule,
  RequestsOverviewModule,
  RequestOverviewModule,
  SalesOrderOverviewModule,
  SalesOrdersOverviewModule,

  ProductQuoteEditModule,
  RequestEditModule,
  QuoteItemEditModule,
  RequestItemEditModule,
  SalesOrderEditModule,
  SalesOrderItemEditModule,
];
