export * from './invoice/invoices-overview.module';
export * from './invoice/invoice-overview.module';
export * from './invoice/invoice.module';
export * from './invoiceitem/invoiceitem.module';
export * from './salesterms/incoterm/incoterm.module';
export * from './salesterms/invoiceterm/invoiceterm.module';
export * from './salesterms/orderterm/orderterm.module';
export * from './repeatinginvoice/repeatinginvoice.module';

import { InvoiceOverviewModule } from './invoice/invoice-overview.module';
import { InvoiceComponent } from './invoice/invoice.component';
import { InvoiceModule } from './invoice/invoice.module';
import { InvoicesOverviewModule } from './invoice/invoices-overview.module';
import { InvoiceItemEditComponent } from './invoiceitem/invoiceitem.component';
import { InvoiceItemEditModule } from './invoiceitem/invoiceitem.module';
import { RepeatingInvoiceEditModule } from './repeatinginvoice/repeatinginvoice.module';
import { IncoTermEditModule } from './salesterms/incoterm/incoterm.module';
import { InvoiceTermEditModule } from './salesterms/invoiceterm/invoiceterm.module';
import { OrderTermEditModule } from './salesterms/orderterm/orderterm.module';

export const modules = [
  IncoTermEditModule,
  InvoiceModule,
  InvoiceOverviewModule,
  InvoicesOverviewModule,
  InvoiceItemEditModule,
  InvoiceTermEditModule,
  OrderTermEditModule,
  RepeatingInvoiceEditModule,
];
