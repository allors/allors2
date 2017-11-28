import { SessionObject } from "@allors/framework";
import { Locale } from './Locale.g';
import { User } from './User.g';
export declare class Singleton extends SessionObject {
    readonly CanReadDefaultLocale: boolean;
    readonly CanWriteDefaultLocale: boolean;
    DefaultLocale: Locale;
    readonly CanReadLocales: boolean;
    readonly CanWriteLocales: boolean;
    Locales: Locale[];
    AddLocale(value: Locale): void;
    RemoveLocale(value: Locale): void;
    readonly CanReadGuest: boolean;
    readonly CanWriteGuest: boolean;
    Guest: User;
}
