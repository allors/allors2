// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
import { SessionObject, Method } from "../../framework";

import { User } from './User.g';
import { Localised } from './Localised.g';
import { TaskList } from './TaskList.g';
import { NotificationList } from './NotificationList.g';
import { Locale } from './Locale.g';
import { Singleton } from './Singleton.g';
import { TaskAssignment } from './TaskAssignment.g';

export class AutomatedAgent extends SessionObject implements User {
    get CanReadUserName(): boolean {
        return this.canRead('UserName');
    }

    get CanWriteUserName(): boolean {
        return this.canWrite('UserName');
    }

    get UserName(): string {
        return this.get('UserName');
    }

    set UserName(value: string) {
        this.set('UserName', value);
    }

    get CanReadNormalizedUserName(): boolean {
        return this.canRead('NormalizedUserName');
    }

    get CanWriteNormalizedUserName(): boolean {
        return this.canWrite('NormalizedUserName');
    }

    get NormalizedUserName(): string {
        return this.get('NormalizedUserName');
    }

    set NormalizedUserName(value: string) {
        this.set('NormalizedUserName', value);
    }

    get CanReadUserEmail(): boolean {
        return this.canRead('UserEmail');
    }

    get CanWriteUserEmail(): boolean {
        return this.canWrite('UserEmail');
    }

    get UserEmail(): string {
        return this.get('UserEmail');
    }

    set UserEmail(value: string) {
        this.set('UserEmail', value);
    }

    get CanReadUserEmailConfirmed(): boolean {
        return this.canRead('UserEmailConfirmed');
    }

    get CanWriteUserEmailConfirmed(): boolean {
        return this.canWrite('UserEmailConfirmed');
    }

    get UserEmailConfirmed(): boolean {
        return this.get('UserEmailConfirmed');
    }

    set UserEmailConfirmed(value: boolean) {
        this.set('UserEmailConfirmed', value);
    }

    get CanReadTaskList(): boolean {
        return this.canRead('TaskList');
    }

    get TaskList(): TaskList {
        return this.get('TaskList');
    }


    get CanReadNotificationList(): boolean {
        return this.canRead('NotificationList');
    }

    get CanWriteNotificationList(): boolean {
        return this.canWrite('NotificationList');
    }

    get NotificationList(): NotificationList {
        return this.get('NotificationList');
    }

    set NotificationList(value: NotificationList) {
        this.set('NotificationList', value);
    }

    get CanReadLocale(): boolean {
        return this.canRead('Locale');
    }

    get CanWriteLocale(): boolean {
        return this.canWrite('Locale');
    }

    get Locale(): Locale {
        return this.get('Locale');
    }

    set Locale(value: Locale) {
        this.set('Locale', value);
    }


}
