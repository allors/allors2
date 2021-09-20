import { ISession, IWorkspace, IWorkspaceLifecycle } from '@allors/workspace/domain/system';
import { Class, MetaPopulation, RelationType } from '@allors/workspace/meta/system';
import { Client } from '../Database/Client';
import { Database } from '../Database/Database';
import { Session } from '../Session/Session';
import { Identities } from '../Identities';
import { ObjectFactory } from '../ObjectFactory';
import { WorkspaceObject } from './WorkspaceObject';

export class Workspace implements IWorkspace {
  database: Database;

  objectFactory: ObjectFactory;

  workspaceClassByWorkspaceId: Map<number, Class>;

  workspaceIdsByWorkspaceClass: Map<Class, Set<number>>;

  private readonly objectById: Map<number, WorkspaceObject>;

  constructor(public name: string, public metaPopulation: MetaPopulation, public lifecycle: IWorkspaceLifecycle, private client: Client) {
    this.objectFactory = new ObjectFactory(this.metaPopulation);
    this.database = new Database(this.metaPopulation, client, new Identities());

    this.workspaceClassByWorkspaceId = new Map();
    this.workspaceIdsByWorkspaceClass = new Map();

    this.objectById = new Map();

    this.lifecycle.onInit(this);
  }

  createSession(): ISession {
    return new Session(this, this.lifecycle.createSessionContext());
  }

  get(identity: number): WorkspaceObject | undefined {
    return this.objectById.get(identity);
  }

  registerWorkspaceObject(cls: Class, workspaceId: number): void {
    this.workspaceClassByWorkspaceId.set(workspaceId, cls);

    let ids = this.workspaceIdsByWorkspaceClass.get(cls);
    if (ids === undefined) {
      ids = new Set();
      this.workspaceIdsByWorkspaceClass.set(cls, ids);
    }

    ids.add(workspaceId);
  }

  push(identity: number, cls: Class, version: number, changedRoleByRoleType: Map<RelationType, unknown> | undefined): void {
    const originalWorkspaceObject = this.objectById.get(identity);
    if (!originalWorkspaceObject) {
      this.objectById.set(identity, new WorkspaceObject(this.database, identity, cls, ++version, changedRoleByRoleType));
    } else {
      this.objectById.set(identity, WorkspaceObject.fromOriginal(originalWorkspaceObject, changedRoleByRoleType));
    }
  }
}
