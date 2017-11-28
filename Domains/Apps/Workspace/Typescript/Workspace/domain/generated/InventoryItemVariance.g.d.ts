import { SessionObject } from "@allors/framework";
import { Commentable } from './Commentable.g';
import { VarianceReason } from './VarianceReason.g';
export declare class InventoryItemVariance extends SessionObject implements Commentable {
    readonly CanReadQuantity: boolean;
    readonly CanWriteQuantity: boolean;
    Quantity: number;
    readonly CanReadInventoryDate: boolean;
    readonly CanWriteInventoryDate: boolean;
    InventoryDate: Date;
    readonly CanReadReason: boolean;
    readonly CanWriteReason: boolean;
    Reason: VarianceReason;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
