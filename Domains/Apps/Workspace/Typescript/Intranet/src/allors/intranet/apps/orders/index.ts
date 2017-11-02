export * from "./overview.module";

export * from "./order/orders-overview.module";
export * from "./productquote/productquote-overview.module";
export * from "./productquote/productquotes-overview.module";
export * from "./request/request-overview.module";
export * from "./request/requests-overview.module";
export * from "./salesorder/salesorder-overview.module";
export * from "./salesorder/salesorders-overview.module";

export * from "./productquote/productquote.module";
export * from "./quoteitem/quoteitem.module";
export * from "./request/request.module";
export * from "./requestitem/requestitem.module";
export * from "./salesorder/salesorder.module";
export * from "./salesorderitem/salesorderitem.module";

import { OverviewModule } from "./overview.module";

import { OrdersOverviewModule } from "./order/orders-overview.module";
import { ProductQuoteOverviewModule } from "./productquote/productquote-overview.module";
import { ProductQuotesOverviewModule } from "./productquote/productquotes-overview.module";
import { RequestOverviewModule } from "./request/request-overview.module";
import { RequestsOverviewModule } from "./request/requests-overview.module";
import { SalesOrderOverviewModule } from "./salesorder/salesorder-overview.module";
import { SalesOrdersOverviewModule } from "./salesorder/salesorders-overview.module";

import { ProductQuoteEditModule } from "./productquote/productquote.module";
import { QuoteItemEditModule } from "./quoteitem/quoteitem.module";
import { RequestEditModule } from "./request/request.module";
import { RequestItemEditModule } from "./requestitem/requestitem.module";
import { SalesOrderEditModule } from "./salesorder/salesorder.module";
import { SalesOrderItemEditModule } from "./salesorderitem/salesorderitem.module";

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
