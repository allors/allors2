import { SessionObject } from "@allors/framework";
import { Localised } from './Localised.g';
import { TaskList } from './TaskList.g';
import { NotificationList } from './NotificationList.g';
export interface User extends SessionObject, Localised {
    UserName: string;
    NormalizedUserName: string;
    UserEmail: string;
    UserEmailConfirmed: boolean;
    TaskList: TaskList;
    NotificationList: NotificationList;
}
