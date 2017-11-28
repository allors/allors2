import { SessionObject } from "@allors/framework";
import { Period } from './Period.g';
import { Commentable } from './Commentable.g';
export declare class PartBillOfMaterialSubstitute extends SessionObject implements Period, Commentable {
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
