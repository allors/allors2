import { SessionObject } from "@allors/framework";
import { Period } from './Period.g';
export declare class Deployment extends SessionObject implements Period {
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
}
