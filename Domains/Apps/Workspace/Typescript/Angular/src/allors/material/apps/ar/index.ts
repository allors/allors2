// Overview
import { OverviewComponent } from "./overview.component";
import { ArOverviewComponent } from "./overview/accountsReveivables/arOverview.component";
import { InvoiceOverviewComponent } from "./overview/invoice/invoiceOverview.component";
import { InvoicesOverviewComponent } from "./overview/invoices/invoicesOverview.component";

import { InvoiceEditComponent } from "./invoice/edit.component";
import { InvoiceItemEditComponent } from "./invoiceItem/edit.component";

export const AR: any[] = [
];

export const AR_ROUTING: any[] = [
  // Overview
  ArOverviewComponent,
  OverviewComponent,
  InvoiceOverviewComponent,
  InvoicesOverviewComponent,

  InvoiceEditComponent,
  InvoiceItemEditComponent,
];

export {
  // Overview
  ArOverviewComponent,
  OverviewComponent,
  InvoiceOverviewComponent,
  InvoicesOverviewComponent,

  InvoiceEditComponent,
  InvoiceItemEditComponent,
};
