import { SessionObject } from "@allors/framework";
import { Language } from './Language.g';
import { Country } from './Country.g';
export declare class Locale extends SessionObject {
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadLanguage: boolean;
    readonly CanWriteLanguage: boolean;
    Language: Language;
    readonly CanReadCountry: boolean;
    readonly CanWriteCountry: boolean;
    Country: Country;
}
