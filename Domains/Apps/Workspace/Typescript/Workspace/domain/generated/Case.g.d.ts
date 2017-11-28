import { SessionObject } from "@allors/framework";
import { UniquelyIdentifiable } from './UniquelyIdentifiable.g';
import { CaseState } from './CaseState.g';
import { CaseVersion } from './CaseVersion.g';
export declare class Case extends SessionObject implements UniquelyIdentifiable {
    readonly CanReadCaseState: boolean;
    readonly CanWriteCaseState: boolean;
    CaseState: CaseState;
    readonly CanReadCurrentVersion: boolean;
    readonly CanWriteCurrentVersion: boolean;
    CurrentVersion: CaseVersion;
    readonly CanReadAllVersions: boolean;
    readonly CanWriteAllVersions: boolean;
    AllVersions: CaseVersion[];
    AddAllVersion(value: CaseVersion): void;
    RemoveAllVersion(value: CaseVersion): void;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
