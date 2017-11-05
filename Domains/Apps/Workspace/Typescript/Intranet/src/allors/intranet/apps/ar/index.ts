export * from "./overview.module";

export * from "./dashboard/dashboard.module";

export * from "./invoice/invoices-overview.module";
export * from "./invoice/invoice-overview.module";
export * from "./invoice/invoice.module";
export * from "./invoiceitem/invoiceitem.module";
export * from "./invoice/invoice-print.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { InvoiceOverviewModule } from "./invoice/invoice-overview.module";
import { InvoicePrintModule } from "./invoice/invoice-print.module";
import { InvoiceComponent } from "./invoice/invoice.component";
import { InvoiceModule } from "./invoice/invoice.module";
import { InvoicesOverviewModule } from "./invoice/invoices-overview.module";
import { InvoiceItemEditComponent } from "./invoiceitem/invoiceitem.component";
import { InvoiceItemEditModule } from "./invoiceitem/invoiceitem.module";

export const modules = [
  // Overview
  OverviewModule,
  DashboardModule,

  InvoiceModule, InvoiceOverviewModule, InvoicesOverviewModule, InvoicePrintModule, InvoiceItemEditModule,
];
