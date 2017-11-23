import { SessionObject } from "@allors/base-domain";
export declare class Notification extends SessionObject {
    readonly CanReadConfirmed: boolean;
    readonly CanWriteConfirmed: boolean;
    Confirmed: boolean;
    readonly CanReadTitle: boolean;
    readonly CanWriteTitle: boolean;
    Title: string;
    readonly CanReadDescription: boolean;
    readonly CanWriteDescription: boolean;
    Description: string;
    readonly CanReadDateCreated: boolean;
    readonly DateCreated: Date;
}
