import { SessionObject } from "@allors/framework";
import { TermType } from './TermType.g';
export declare class OrderTerm extends SessionObject {
    readonly CanReadTermValue: boolean;
    readonly CanWriteTermValue: boolean;
    TermValue: string;
    readonly CanReadTermType: boolean;
    readonly CanWriteTermType: boolean;
    TermType: TermType;
}
