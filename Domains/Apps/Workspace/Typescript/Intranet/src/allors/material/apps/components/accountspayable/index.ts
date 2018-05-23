export * from './invoice/invoices-overview.module';
export * from './invoice/invoice-overview.module';
export * from './invoice/invoice.module';
export * from './invoiceitem/invoiceitem.module';

import { InvoiceOverviewModule } from './invoice/invoice-overview.module';
import { InvoiceComponent } from './invoice/invoice.component';
import { InvoiceModule } from './invoice/invoice.module';
import { InvoicesOverviewModule } from './invoice/invoices-overview.module';
import { InvoiceItemEditComponent } from './invoiceitem/invoiceitem.component';
import { InvoiceItemEditModule } from './invoiceitem/invoiceitem.module';

export const modules = [
  InvoiceModule,
  InvoiceOverviewModule,
  InvoicesOverviewModule,
  InvoiceItemEditModule,
];
