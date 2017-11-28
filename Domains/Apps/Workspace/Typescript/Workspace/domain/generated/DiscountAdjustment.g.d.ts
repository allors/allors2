import { SessionObject } from "@allors/framework";
import { OrderAdjustment } from './OrderAdjustment.g';
import { DiscountAdjustmentVersion } from './DiscountAdjustmentVersion.g';
import { VatRate } from './VatRate.g';
export declare class DiscountAdjustment extends SessionObject implements OrderAdjustment {
    readonly CanReadCurrentVersion: boolean;
    readonly CanWriteCurrentVersion: boolean;
    CurrentVersion: DiscountAdjustmentVersion;
    readonly CanReadAllVersions: boolean;
    readonly CanWriteAllVersions: boolean;
    AllVersions: DiscountAdjustmentVersion[];
    AddAllVersion(value: DiscountAdjustmentVersion): void;
    RemoveAllVersion(value: DiscountAdjustmentVersion): void;
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
