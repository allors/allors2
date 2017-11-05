export * from "./overview.module";

export * from "./dashboard/dashboard.module";

export * from "./invoice/invoices-overview.module";
export * from "./invoice/invoice-overview.module";
export * from "./invoice/invoice.module";
export * from "./invoice/invoiceItem/invoice-invoiceitem.module";
export * from "./invoice/invoice-print.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { InvoiceOverviewModule } from "./invoice/invoice-overview.module";
import { InvoicePrintModule } from "./invoice/invoice-print.module";
import { InvoiceModule } from "./invoice/invoice.module";
import { InvoiceInvoiceItemModule } from "./invoice/invoiceItem/invoice-invoiceitem.module";
import { InvoicesOverviewModule } from "./invoice/invoices-overview.module";

export const modules = [
  // Overview
  OverviewModule,
  DashboardModule,

  InvoiceModule, InvoiceOverviewModule, InvoicesOverviewModule, InvoicePrintModule, InvoiceInvoiceItemModule,
];
