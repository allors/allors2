import { SessionObject } from "@allors/base-domain";
import { OrderVersion } from './OrderVersion.g';
export declare class Order extends SessionObject {
    readonly CanReadCurrentVersion: boolean;
    readonly CanWriteCurrentVersion: boolean;
    CurrentVersion: OrderVersion;
    readonly CanReadAllVersions: boolean;
    readonly CanWriteAllVersions: boolean;
    AllVersions: OrderVersion[];
    AddAllVersion(value: OrderVersion): void;
    RemoveAllVersion(value: OrderVersion): void;
}
