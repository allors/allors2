import { SessionObject } from "@allors/framework";
import { VatRate } from './VatRate.g';
export declare class InvoiceVatRateItem extends SessionObject {
    readonly CanReadBaseAmount: boolean;
    readonly CanWriteBaseAmount: boolean;
    BaseAmount: number;
    readonly CanReadVatRates: boolean;
    readonly CanWriteVatRates: boolean;
    VatRates: VatRate[];
    AddVatRate(value: VatRate): void;
    RemoveVatRate(value: VatRate): void;
    readonly CanReadVatAmount: boolean;
    readonly CanWriteVatAmount: boolean;
    VatAmount: number;
}
