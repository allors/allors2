import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { Commentable } from './Commentable.g';
import { PartSpecificationState } from './PartSpecificationState.g';
import { PartSpecificationVersion } from './PartSpecificationVersion.g';
export declare class PartSpecification extends SessionObject implements UniquelyIdentifiable, Commentable {
    readonly CanReadPartSpecificationState: boolean;
    readonly CanWritePartSpecificationState: boolean;
    PartSpecificationState: PartSpecificationState;
    readonly CanReadCurrentVersion: boolean;
    readonly CanWriteCurrentVersion: boolean;
    CurrentVersion: PartSpecificationVersion;
    readonly CanReadAllVersions: boolean;
    readonly CanWriteAllVersions: boolean;
    AllVersions: PartSpecificationVersion[];
    AddAllVersion(value: PartSpecificationVersion): void;
    RemoveAllVersion(value: PartSpecificationVersion): void;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
}
