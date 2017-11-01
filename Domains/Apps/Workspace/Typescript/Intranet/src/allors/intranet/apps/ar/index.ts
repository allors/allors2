export * from "./overview.module";

export * from "./overview/accountsReveivables/arOverview.module";
export * from "./overview/invoice/invoiceOverview.module";
export * from "./overview/invoices/invoicesOverview.module";

export * from "./invoice/edit.module";
export * from "./invoiceItem/edit.module";

import { OverviewModule } from "./overview.module";

import { ArOverviewModule } from "./overview/accountsReveivables/arOverview.module";
import { InvoiceOverviewModule } from "./overview/invoice/invoiceOverview.module";
import { InvoicesOverviewModule } from "./overview/invoices/invoicesOverview.module";

import { InvoiceEditModule } from "./invoice/edit.module";
import { InvoiceItemEditModule } from "./invoiceItem/edit.module";

export const Modules = [
  // Overview
  OverviewModule,
  ArOverviewModule,
  InvoiceOverviewModule,
  InvoicesOverviewModule,

  InvoiceEditModule,
  InvoiceItemEditModule,
];
