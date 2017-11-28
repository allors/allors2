import { SessionObject } from "@allors/framework";
import { Budget } from './Budget.g';
import { OperatingBudgetVersion } from './OperatingBudgetVersion.g';
import { BudgetState } from './BudgetState.g';
export declare class OperatingBudget extends SessionObject implements Budget {
    readonly CanReadCurrentVersion: boolean;
    readonly CanWriteCurrentVersion: boolean;
    CurrentVersion: OperatingBudgetVersion;
    readonly CanReadAllVersions: boolean;
    readonly CanWriteAllVersions: boolean;
    AllVersions: OperatingBudgetVersion[];
    AddAllVersion(value: OperatingBudgetVersion): void;
    RemoveAllVersion(value: OperatingBudgetVersion): void;
    readonly CanReadBudgetState: boolean;
    readonly CanWriteBudgetState: boolean;
    BudgetState: BudgetState;
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
    readonly CanReadUniqueId: boolean;
    readonly CanWriteUniqueId: boolean;
    UniqueId: string;
}
