import { ISessionLifecycle } from "./ISessionLifecycle";
import { IWorkspace } from "../IWorkspace";

export interface IWorkspaceLifecycle {
  onInit(workspace: IWorkspace): void;

  createSessionContext(): ISessionLifecycle;
}
