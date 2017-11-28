import { SessionObject } from "@allors/framework";
import { Document } from './Document.g';
export declare class BillOfLading extends SessionObject implements Document {
    readonly CanReadPrintContent: boolean;
    readonly CanWritePrintContent: boolean;
    PrintContent: string;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
