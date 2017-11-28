import { SessionObject } from "@allors/framework";
import { ObjectState } from './ObjectState.g';
export declare class PurchaseInvoiceState extends SessionObject implements ObjectState {
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
