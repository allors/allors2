import { SessionObject, Method } from "@allors/base-domain";
import { Deletable } from './Deletable.g';
import { TaskAssignment } from './TaskAssignment.g';
export declare class TaskList extends SessionObject implements Deletable {
    readonly CanReadTaskAssignments: boolean;
    readonly TaskAssignments: TaskAssignment[];
    readonly CanReadOpenTaskAssignments: boolean;
    readonly OpenTaskAssignments: TaskAssignment[];
    readonly CanReadCount: boolean;
    readonly Count: number;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
