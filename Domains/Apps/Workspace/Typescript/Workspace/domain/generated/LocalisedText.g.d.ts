import { SessionObject } from "@allors/framework";
import { Localised } from './Localised.g';
import { Locale } from './Locale.g';
export declare class LocalisedText extends SessionObject implements Localised {
    readonly CanReadText: boolean;
    readonly CanWriteText: boolean;
    Text: string;
    readonly CanReadLocale: boolean;
    readonly CanWriteLocale: boolean;
    Locale: Locale;
}
