import { SessionObject } from "@allors/framework";
import { DeploymentUsage } from './DeploymentUsage.g';
export declare class TimePeriodUsage extends SessionObject implements DeploymentUsage {
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
