import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
export declare class CostCenterCategory extends SessionObject implements UniquelyIdentifiable {
    readonly CanReadParent: boolean;
    readonly CanWriteParent: boolean;
    Parent: CostCenterCategory;
    readonly CanReadAncestors: boolean;
    readonly Ancestors: CostCenterCategory[];
    readonly CanReadChildren: boolean;
    readonly Children: CostCenterCategory[];
    readonly CanReadDescription: boolean;
    readonly CanWriteDescription: boolean;
    Description: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
