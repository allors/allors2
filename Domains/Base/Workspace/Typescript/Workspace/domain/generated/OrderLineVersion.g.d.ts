import { SessionObject } from "@allors/framework";
import { Version } from './Version.g';
export declare class OrderLineVersion extends SessionObject implements Version {
    readonly CanReadDerivationTimeStamp: boolean;
    readonly CanWriteDerivationTimeStamp: boolean;
    DerivationTimeStamp: Date;
}
