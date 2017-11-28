import { SessionObject } from "@allors/framework";
import { OrderAdjustmentVersion } from './OrderAdjustmentVersion.g';
import { VatRate } from './VatRate.g';
export declare class DiscountAdjustmentVersion extends SessionObject implements OrderAdjustmentVersion {
    readonly CanReadAmount: boolean;
    readonly CanWriteAmount: boolean;
    Amount: number;
    readonly CanReadVatRate: boolean;
    readonly CanWriteVatRate: boolean;
    VatRate: VatRate;
    readonly CanReadPercentage: boolean;
    readonly CanWritePercentage: boolean;
    Percentage: number;
    readonly CanReadDerivationTimeStamp: boolean;
    readonly CanWriteDerivationTimeStamp: boolean;
    DerivationTimeStamp: Date;
}
