import { SessionObject } from "@allors/framework";
import { PartBillOfMaterial } from './PartBillOfMaterial.g';
export declare class EngineeringBom extends SessionObject implements PartBillOfMaterial {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
}
