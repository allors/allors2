import { SessionObject } from "@allors/framework";
import { ServiceEntry } from './ServiceEntry.g';
export declare class TimeEntry extends SessionObject implements ServiceEntry {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
