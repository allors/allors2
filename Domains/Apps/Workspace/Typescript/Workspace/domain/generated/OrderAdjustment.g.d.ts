import { SessionObject } from "@allors/framework";
import { VatRate } from './VatRate.g';
export interface OrderAdjustment extends SessionObject {
    Amount: number;
    VatRate: VatRate;
    Percentage: number;
}
