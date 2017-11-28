import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
export declare class PositionReportingStructure extends SessionObject implements Commentable {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
