import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
export declare class PerformanceNote extends SessionObject implements Commentable {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
