import { SessionObject, Method } from "@allors/base-domain";
import { Deletable } from './Deletable.g';
import { User } from './User.g';
import { Task } from './Task.g';
export declare class TaskAssignment extends SessionObject implements Deletable {
    readonly CanReadUser: boolean;
    readonly CanWriteUser: boolean;
    User: User;
    readonly CanReadTask: boolean;
    readonly CanWriteTask: boolean;
    Task: Task;
    readonly CanExecuteDelete: boolean;
    readonly Delete: Method;
}
