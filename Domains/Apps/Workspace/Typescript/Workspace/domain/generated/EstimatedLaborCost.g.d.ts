import { SessionObject } from "@allors/framework";
import { EstimatedProductCost } from './EstimatedProductCost.g';
export declare class EstimatedLaborCost extends SessionObject implements EstimatedProductCost {
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
}
