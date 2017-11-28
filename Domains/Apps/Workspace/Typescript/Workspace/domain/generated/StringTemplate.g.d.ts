import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { Localised } from './Localised.g';
import { Locale } from './Locale.g';
export declare class StringTemplate extends SessionObject implements UniquelyIdentifiable, Localised {
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
    readonly CanReadLocale: boolean;
    readonly CanWriteLocale: boolean;
    Locale: Locale;
}
