import { SessionObject, Method } from "@allors/framework";
import { Period } from './Period.g';
import { Commentable } from './Commentable.g';
import { Deletable } from './Deletable.g';
import { Person } from './Person.g';
import { WorkEffort } from './WorkEffort.g';
export declare class WorkEffortAssignment extends SessionObject implements Period, Commentable, Deletable {
    readonly CanReadProfessional: boolean;
    readonly CanWriteProfessional: boolean;
    Professional: Person;
    readonly CanReadAssignment: boolean;
    readonly CanWriteAssignment: boolean;
    Assignment: WorkEffort;
    readonly CanReadFromDate: boolean;
    readonly CanWriteFromDate: boolean;
    FromDate: Date;
    readonly CanReadThroughDate: boolean;
    readonly CanWriteThroughDate: boolean;
    ThroughDate: Date;
    readonly CanReadComment: boolean;
    readonly CanWriteComment: boolean;
    Comment: string;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
