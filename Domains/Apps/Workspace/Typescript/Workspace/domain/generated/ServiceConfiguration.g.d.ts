import { SessionObject } from "@allors/framework";
import { InventoryItemConfiguration } from './InventoryItemConfiguration.g';
export declare class ServiceConfiguration extends SessionObject implements InventoryItemConfiguration {
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
