import { SessionObject } from "@allors/framework";
import { Version } from './Version.g';
import { VatRate } from './VatRate.g';
export interface OrderAdjustmentVersion extends SessionObject, Version {
    Amount: number;
    VatRate: VatRate;
    Percentage: number;
}
