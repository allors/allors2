import { MetaPopulation } from "@allors/workspace/meta/system";
import { IObjectFactory } from "./IObjectFactory";
import { ISession } from "./ISession";
import { IWorkspaceLifecycle } from "./state/IWorkspaceLifecycle";

export interface IWorkspace {
  name: string;

  metaPopulation: MetaPopulation;

  objectFactory: IObjectFactory;

  lifecycle: IWorkspaceLifecycle;

  createSession(): ISession;
}
