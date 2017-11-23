import { SessionObject } from "@allors/base-domain";
import { Currency } from './Currency.g';
import { LocalisedText } from './LocalisedText.g';
export declare class Country extends SessionObject {
    readonly CanReadCurrency: boolean;
    readonly CanWriteCurrency: boolean;
    Currency: Currency;
    readonly CanReadIsoCode: boolean;
    readonly CanWriteIsoCode: boolean;
    IsoCode: string;
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadLocalisedNames: boolean;
    readonly CanWriteLocalisedNames: boolean;
    LocalisedNames: LocalisedText[];
    AddLocalisedName(value: LocalisedText): void;
    RemoveLocalisedName(value: LocalisedText): void;
}
