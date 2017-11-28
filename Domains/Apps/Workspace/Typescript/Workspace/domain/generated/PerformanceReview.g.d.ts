import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
import { Period } from './Period.g';
export declare class PerformanceReview extends SessionObject implements Commentable, Period {
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
