import { SessionObject } from "@allors/base-domain";
import { Version } from './Version.g';
export declare class OrderLineVersion extends SessionObject implements Version {
    readonly CanReadDerivationTimeStamp: boolean;
    readonly CanWriteDerivationTimeStamp: boolean;
    DerivationTimeStamp: Date;
}
