import { SessionObject } from "@allors/framework";
import { PriceableVersion } from './PriceableVersion.g';
import { AgreementTerm } from './AgreementTerm.g';
import { InvoiceVatRateItem } from './InvoiceVatRateItem.g';
import { InvoiceItem } from './InvoiceItem.g';
export interface InvoiceItemVersion extends SessionObject, PriceableVersion {
    InternalComment: string;
    InvoiceTerms: AgreementTerm[];
    AddInvoiceTerm(value: AgreementTerm): any;
    RemoveInvoiceTerm(value: AgreementTerm): any;
    TotalInvoiceAdjustment: number;
    InvoiceVatRateItems: InvoiceVatRateItem[];
    AddInvoiceVatRateItem(value: InvoiceVatRateItem): any;
    RemoveInvoiceVatRateItem(value: InvoiceVatRateItem): any;
    AdjustmentFor: InvoiceItem;
    Message: string;
    TotalInvoiceAdjustmentCustomerCurrency: number;
    AmountPaid: number;
    Quantity: number;
    Description: string;
}
