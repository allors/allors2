import { SessionObject } from "@allors/framework";
import { PaymentMethod } from './PaymentMethod.g';
export declare class OwnCreditCard extends SessionObject implements PaymentMethod {
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
