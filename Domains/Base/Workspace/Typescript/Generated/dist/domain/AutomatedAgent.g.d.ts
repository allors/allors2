import { SessionObject } from "@allors/base-domain";
import { User } from './User.g';
import { TaskList } from './TaskList.g';
import { NotificationList } from './NotificationList.g';
import { Locale } from './Locale.g';
export declare class AutomatedAgent extends SessionObject implements User {
    readonly CanReadUserName: boolean;
    readonly CanWriteUserName: boolean;
    UserName: string;
    readonly CanReadNormalizedUserName: boolean;
    readonly CanWriteNormalizedUserName: boolean;
    NormalizedUserName: string;
    readonly CanReadUserEmail: boolean;
    readonly CanWriteUserEmail: boolean;
    UserEmail: string;
    readonly CanReadUserEmailConfirmed: boolean;
    readonly CanWriteUserEmailConfirmed: boolean;
    UserEmailConfirmed: boolean;
    readonly CanReadTaskList: boolean;
    readonly TaskList: TaskList;
    readonly CanReadNotificationList: boolean;
    readonly CanWriteNotificationList: boolean;
    NotificationList: NotificationList;
    readonly CanReadLocale: boolean;
    readonly CanWriteLocale: boolean;
    Locale: Locale;
}
