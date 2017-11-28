import { SessionObject } from "@allors/framework";
import { OrderAdjustment } from './OrderAdjustment.g';
import { VatRate } from './VatRate.g';
export declare class Fee extends SessionObject implements OrderAdjustment {
    readonly CanReadAmount: boolean;
    readonly CanWriteAmount: boolean;
    Amount: number;
    readonly CanReadVatRate: boolean;
    readonly CanWriteVatRate: boolean;
    VatRate: VatRate;
    readonly CanReadPercentage: boolean;
    readonly CanWritePercentage: boolean;
    Percentage: number;
}
