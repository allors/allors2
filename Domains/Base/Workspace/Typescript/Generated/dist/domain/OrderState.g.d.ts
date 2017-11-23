import { SessionObject } from "@allors/base-domain";
import { ObjectState } from './ObjectState.g';
export declare class OrderState extends SessionObject implements ObjectState {
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
