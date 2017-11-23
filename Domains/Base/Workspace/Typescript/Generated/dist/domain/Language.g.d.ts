import { SessionObject } from "@allors/base-domain";
import { LocalisedText } from './LocalisedText.g';
export declare class Language extends SessionObject {
    readonly CanReadIsoCode: boolean;
    readonly CanWriteIsoCode: boolean;
    IsoCode: string;
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadNativeName: boolean;
    readonly CanWriteNativeName: boolean;
    NativeName: string;
    readonly CanReadLocalisedNames: boolean;
    readonly CanWriteLocalisedNames: boolean;
    LocalisedNames: LocalisedText[];
    AddLocalisedName(value: LocalisedText): void;
    RemoveLocalisedName(value: LocalisedText): void;
}
