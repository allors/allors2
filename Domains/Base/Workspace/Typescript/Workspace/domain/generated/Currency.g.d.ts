import { SessionObject } from "@allors/framework";
import { LocalisedText } from './LocalisedText.g';
export declare class Currency extends SessionObject {
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
