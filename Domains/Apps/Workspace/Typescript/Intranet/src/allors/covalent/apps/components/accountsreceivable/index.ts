export * from "./overview.module";

export * from "./dashboard/dashboard.module";

export * from "./invoice/invoices-overview.module";
export * from "./invoice/invoice-overview.module";
export * from "./invoice/invoice.module";
export * from "./invoice/invoice-print.module";
export * from "./invoiceitem/invoiceitem.module";
export * from "./salesterms/incoterm/incoterm.module";
export * from "./salesterms/invoiceterm/invoiceterm.module";
export * from "./salesterms/orderterm/orderterm.module";
export * from "./repeatinginvoice/repeatinginvoice.module";

import { OverviewModule } from "./overview.module";

import { DashboardModule } from "./dashboard/dashboard.module";

import { InvoiceOverviewModule } from "./invoice/invoice-overview.module";
import { InvoicePrintModule } from "./invoice/invoice-print.module";
import { InvoiceComponent } from "./invoice/invoice.component";
import { InvoiceModule } from "./invoice/invoice.module";
import { InvoicesOverviewModule } from "./invoice/invoices-overview.module";
import { InvoiceItemEditComponent } from "./invoiceitem/invoiceitem.component";
import { InvoiceItemEditModule } from "./invoiceitem/invoiceitem.module";
import { RepeatingInvoiceEditModule } from "./repeatinginvoice/repeatinginvoice.module";
import { IncoTermEditModule } from "./salesterms/incoterm/incoterm.module";
import { InvoiceTermEditModule } from "./salesterms/invoiceterm/invoiceterm.module";
import { OrderTermEditModule } from "./salesterms/orderterm/orderterm.module";

export const modules = [
  // Overview
  OverviewModule,
  DashboardModule,

  IncoTermEditModule,
  InvoiceModule,
  InvoiceOverviewModule,
  InvoicesOverviewModule,
  InvoicePrintModule,
  InvoiceItemEditModule,
  InvoiceTermEditModule,
  OrderTermEditModule,
  RepeatingInvoiceEditModule,
];
