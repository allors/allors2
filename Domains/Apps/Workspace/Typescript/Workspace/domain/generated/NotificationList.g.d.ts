import { SessionObject, Method } from "@allors/framework";
import { Deletable } from './Deletable.g';
import { Notification } from './Notification.g';
export declare class NotificationList extends SessionObject implements Deletable {
    readonly CanReadUnconfirmedNotifications: boolean;
    readonly UnconfirmedNotifications: Notification[];
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
