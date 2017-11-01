export * from "./overview.module";

export * from "./dashboard/dashboard.module";

export * from "./invoices/invoices-overview.module";

export * from "./invoices/invoice/invoice-overview.module";
export * from "./invoices/invoice/invoice.module";
export * from "./invoices/invoice/invoiceItem/invoice-invoiceitem.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { InvoiceOverviewModule } from "./invoices/invoice/invoice-overview.module";

import { InvoiceModule } from "./invoices/invoice/invoice.module";
import { InvoiceInvoiceItemModule } from "./invoices/invoice/invoiceItem/invoice-invoiceitem.module";
import { InvoicesOverviewModule } from "./invoices/invoices-overview.module";

export const modules = [
  // Overview
  OverviewModule,
  DashboardModule,

  InvoiceModule,
  InvoiceOverviewModule,
  InvoicesOverviewModule,
  InvoiceInvoiceItemModule,
];
