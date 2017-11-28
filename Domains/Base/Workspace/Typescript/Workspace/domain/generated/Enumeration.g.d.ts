import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { LocalisedText } from './LocalisedText.g';
export interface Enumeration extends SessionObject, UniquelyIdentifiable {
    LocalisedNames: LocalisedText[];
    AddLocalisedName(value: LocalisedText): any;
    RemoveLocalisedName(value: LocalisedText): any;
    Name: string;
    IsActive: boolean;
}
