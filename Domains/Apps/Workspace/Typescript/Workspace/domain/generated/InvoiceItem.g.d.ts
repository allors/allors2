import { SessionObject, Method } from "@allors/framework";
import { Priceable } from './Priceable.g';
import { Deletable } from './Deletable.g';
import { AgreementTerm } from './AgreementTerm.g';
import { InvoiceVatRateItem } from './InvoiceVatRateItem.g';
export interface InvoiceItem extends SessionObject, Priceable, Deletable {
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
    CanExecuteDelete: boolean;
    Delete: Method;
}
