import { SessionObject, Method } from "@allors/framework";
import { IUnitOfMeasure } from './IUnitOfMeasure.g';
import { Enumeration } from './Enumeration.g';
import { LocalisedText } from './LocalisedText.g';
export declare class UnitOfMeasure extends SessionObject implements IUnitOfMeasure, Enumeration {
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
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
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
