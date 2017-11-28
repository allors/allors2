import { SessionObject } from "@allors/framework";
import { IUnitOfMeasure } from './IUnitOfMeasure.g';
import { LocalisedText } from './LocalisedText.g';
export declare class Currency extends SessionObject implements IUnitOfMeasure {
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
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
