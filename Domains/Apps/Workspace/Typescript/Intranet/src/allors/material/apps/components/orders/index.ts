export * from './productquote/overview/productquote-overview.module';
export * from './productquote/list/productquote-list.module';
export * from './request/overview/request-overview.module';
export * from './request/list/request-list.module';
export * from './salesorder/salesorder-overview.module';
export * from './salesorder/salesorders-overview.module';

export * from './productquote/edit/productquote-edit.module';
export * from './quoteitem/quoteitem.module';
export * from './request/edit/request-edit.module';
export * from './requestitem/requestitem.module';
export * from './salesorder/salesorder.module';
export * from './salesorderitem/salesorderitem.module';
export * from './salesterms/incoterm/incoterm.module';
export * from './salesterms/invoiceterm/invoiceterm.module';
export * from './salesterms/orderterm/orderterm.module';

import { ProductQuoteOverviewModule } from './productquote/overview/productquote-overview.module';
import { ProductQuotesOverviewModule } from './productquote/list/productquote-list.module';
import { RequestOverviewModule } from './request/overview/request-overview.module';
import { RequestsOverviewModule } from './request/list/request-list.module';
import { SalesOrderOverviewModule } from './salesorder/salesorder-overview.module';
import { SalesOrdersOverviewModule } from './salesorder/salesorders-overview.module';

import { ProductQuoteEditModule } from './productquote/edit/productquote-edit.module';
import { QuoteItemEditModule } from './quoteitem/quoteitem.module';
import { RequestEditModule } from './request/edit/request-edit.module';
import { RequestItemEditModule } from './requestitem/requestitem.module';
import { SalesOrderEditModule } from './salesorder/salesorder.module';
import { SalesOrderItemEditModule } from './salesorderitem/salesorderitem.module';
import { IncoTermEditModule } from './salesterms/incoterm/incoterm.module';
import { InvoiceTermEditModule } from './salesterms/invoiceterm/invoiceterm.module';
import { OrderTermEditModule } from './salesterms/orderterm/orderterm.module';

export const Modules = [
  IncoTermEditModule,
  InvoiceTermEditModule,
  OrderTermEditModule,
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
