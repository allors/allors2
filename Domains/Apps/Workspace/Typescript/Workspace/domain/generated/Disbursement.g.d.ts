import { SessionObject } from "@allors/framework";
import { Payment } from './Payment.g';
export declare class Disbursement extends SessionObject implements Payment {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
