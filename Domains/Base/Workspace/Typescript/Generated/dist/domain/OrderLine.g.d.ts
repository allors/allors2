import { SessionObject } from "@allors/base-domain";
import { OrderLineVersion } from './OrderLineVersion.g';
export declare class OrderLine extends SessionObject {
    readonly CanReadCurrentVersion: boolean;
    readonly CanWriteCurrentVersion: boolean;
    CurrentVersion: OrderLineVersion;
    readonly CanReadAllVersions: boolean;
    readonly CanWriteAllVersions: boolean;
    AllVersions: OrderLineVersion[];
    AddAllVersion(value: OrderLineVersion): void;
    RemoveAllVersion(value: OrderLineVersion): void;
}
