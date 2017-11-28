import { SessionObject, Method } from "@allors/framework";
import { Deletable } from './Deletable.g';
export declare class SalesChannelRevenue extends SessionObject implements Deletable {
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
