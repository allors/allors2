import { SessionObject, Method } from "@allors/framework";
import { Enumeration } from './Enumeration.g';
import { IUnitOfMeasure } from './IUnitOfMeasure.g';
import { LocalisedText } from './LocalisedText.g';
export declare class TimeFrequency extends SessionObject implements Enumeration, IUnitOfMeasure {
    readonly CanReadLocalisedNames: boolean;
    readonly CanWriteLocalisedNames: boolean;
    LocalisedNames: LocalisedText[];
    AddLocalisedName(value: LocalisedText): void;
    RemoveLocalisedName(value: LocalisedText): void;
    readonly CanReadName: boolean;
    readonly CanWriteName: boolean;
    Name: string;
    readonly CanReadIsActive: boolean;
    readonly CanWriteIsActive: boolean;
    IsActive: boolean;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
