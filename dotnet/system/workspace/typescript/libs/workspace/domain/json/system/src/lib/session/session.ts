import { PullResponse, PushRequest, PushResponse, SyncRequest } from '@allors/protocol/json/system';
import { IChangeSet, IInvokeResult, InvokeOptions, IObject, IPullResult, IPushResult, ISession, ISessionLifecycle, IStrategy, Method, Procedure, Pull } from '@allors/workspace/domain/system';
import { AssociationType, Class, Composite, Origin, RoleType } from '@allors/workspace/meta/system';
import { Database } from '../Database/Database';
import { DatabaseState } from '../Database/DatabaseState';
import { Strategy } from '../Strategy';
import { Workspace } from '../Workspace/Workspace';
import { WorkspaceState } from '../Workspace/WorkspaceState';
import { SessionState } from './SessionState';
import { ChangeSet } from '../ChangeSet';
import { EMPTY, Observable } from 'rxjs';

export class Session implements ISession {
  private readonly strategyByWorkspaceId: Map<number, Strategy>;
  private readonly strategiesByClass: Map<Class, Strategy[]>;

  private readonly existingDatabaseStrategies: Strategy[];
  private newDatabaseStrategies: Set<Strategy> | undefined;
  private workspaceStrategies: Set<Strategy> | undefined;

  private changedDatabaseStates: Set<DatabaseState> | undefined;
  private changedWorkspaceStates: Set<WorkspaceState> | undefined;

  private created: Set<Strategy> | undefined;
  private instantiated: Set<IStrategy> | undefined;

  constructor(public workspace: Workspace, public state: ISessionLifecycle) {
    this.database = this.workspace.database;

    this.strategyByWorkspaceId = new Map();
    this.strategiesByClass = new Map();
    this.existingDatabaseStrategies = [];

    this.sessionState = new SessionState();
    this.state.onInit(this);
  }

  database: Database;

  get hasDatabaseChanges(): boolean {
    return this.newDatabaseStrategies?.size > 0 || this.existingDatabaseStrategies.filter((v) => v.hasDatabaseChanges).length > 0;
  }

  sessionState: SessionState;

  invoke(methods: Method[], options?: InvokeOptions): Observable<IInvokeResult> {
    // var invokeRequest: InvokeRequest = {
    //   l = methods.map((v) => {
    //     i: v.object.id;
    //     v: v.object.strategy.databaseVersion;
    //     m = v.methodType.tag;
    //   }),
    //   o = options != null
    //     ? {
    //         c = options.continueOnError,
    //         i = options.isolated,
    //       }
    //     : undefined,
    // };

    // const invokeResponse = await this.database.invoke(invokeRequest);
    // return new InvokeResult(this, invokeResponse);

    // TODO:
    return EMPTY;
  }

  create(cls: Class): IObject {
    const workspaceId = this.database.identities.nextId();
    const strategy = new Strategy(this, cls, workspaceId);
    this.addStrategy(strategy);

    if (cls.origin == Origin.Database) {
      this.newDatabaseStrategies ??= new Set();
      this.newDatabaseStrategies.add(strategy);
    }

    if (cls.origin == Origin.Workspace) {
      this.workspaceStrategies ??= new Set();
      this.workspaceStrategies.add(strategy);
      // TODO: move to Push
      this.workspace.registerWorkspaceObject(cls, workspaceId);
    }

    this.created ??= new Set();
    this.created.add(strategy);

    return strategy.object;
  }

  getOne(id: number | IObject): IObject {
    if ((id as IObject).id) {
      return this.getStrategy((id as IObject).id)?.object;
    }

    return this.getStrategy(id as number)?.object;
  }

  getMany(id: number[] | IObject[]): IObject[] {
    return id.map((v) => this.getOne(v));
  }
  getAll(objectType: Composite): IObject[] {
    const result: IObject[] = [];

    for (const cls of objectType.classes) {
      switch (cls.origin) {
        case Origin.Workspace:
          if (this.workspace.workspaceIdsByWorkspaceClass.has(cls)) {
            const ids = this.workspace.workspaceIdsByWorkspaceClass.get(cls);
            if (ids !== undefined) {
              for (const id of ids) {
                let strategy = this.strategyByWorkspaceId.get(id);
                if (strategy) {
                  result.push(strategy.object);
                } else {
                  strategy = this.instantiateWorkspaceObject(id);
                  result.push(strategy.object);
                }
              }
            }
          }
          break;
        default:
          if (this.strategiesByClass.has(cls)) {
            const strategies = this.strategiesByClass.get(cls);
            if (strategies !== undefined) {
              for (const strategy of strategies) {
                result.push(strategy.object);
              }
            }
          }
          break;
      }
    }

    return result;
  }

  pull(procedureOrPulls: Procedure | Pull[], pulls?: Pull[]): Observable<IPullResult> {
    // var pullRequest = new PullRequest
    // {
    //   // TODO: visitor
    //     l: pulls.map(v => v.ToJson())
    // };

    // var pullResponse = await this.database.pull(pullRequest);
    // return await this.onPull(pullResponse);

    // var pullRequest = new PullRequest();
    // {
    //   (p = procedure.ToJson()), (l = pulls.map((v) => v.ToJson()).ToArray());
    // }

    // var pullResponse = await this.database.pull(pullRequest);
    // return await this.onPull(pullResponse);

    // TODO:
    return EMPTY;
  }

  reset(): void {
    for (const value of this.strategyByWorkspaceId.values()) {
      value.reset();
    }
  }

  push(): Observable<IPushResult> {
    // const pushRequest = this.pushRequest();
    // const pushResponse = await this.database.push(pushRequest);
    // if (!pushResponse.hasErrors) {
    //   this.pushResponse(pushResponse);

    //   var objects = pushRequest.o.map((v) => v.i);
    //   if (pushResponse.n != null) {
    //     objects = objects.Union(pushResponse.n.Select((v) => v.DatabaseId)).ToArray();
    //   }

    //   var syncRequests: SyncRequest = {
    //     o = objects,
    //   };

    //   await this.sync(syncRequests);

    //   if (this.workspaceStrategies != null) {
    //     for (const workspaceStrategy of this.workspaceStrategies) {
    //       workspaceStrategy.workspacePush();
    //     }
    //   }

    //   this.reset();
    // }

    // return new PushResult(this, pushResponse);

    // TODO:
    return EMPTY;
  }

  checkpoint(): IChangeSet {
    const changeSet = new ChangeSet(this, this.created, this.instantiated, this.sessionState.checkpoint());

    if (this.changedWorkspaceStates != null) {
      for (const changed of this.changedWorkspaceStates) {
        changed.checkpoint(changeSet);
      }
    }

    if (this.changedDatabaseStates != null) {
      for (const changed of this.changedDatabaseStates) {
        changed.checkpoint(changeSet);
      }
    }

    this.created = null;
    this.instantiated = null;

    return changeSet;
  }

  getStrategy(identity: number): Strategy {
    if (identity > 0) {
      return this.strategyByWorkspaceId.get(identity);
    } else if (identity < 0) {
      return this.instantiateWorkspaceObject(identity).strategy;
    } else {
      return null;
    }
  }

  getRole(association: Strategy, roleType: RoleType): unknown {
    let role = this.sessionState.getRole(association, roleType);
    if (roleType.objectType.isUnit) {
      return role;
    }

    if (roleType.isOne) {
      return this.get<IObject>(role);
    }

    var ids = role as [];
    return ids?.map((v) => this.getOne<IObject>(v)) ?? [];
  }

  getCompositeAssociation(role: Strategy, associationType: AssociationType): IObject {
    const roleType = associationType.roleType;

    for (const association of this.getAll(associationType.objectType)) {
      if (!association.canRead(roleType)) {
        continue;
      }

      if (association.IiAssociationForRole(roleType, role)) {
        return association.Object;
      }
    }
  }

  getCompositesAssociation<T>(role: Strategy, associationType: AssociationType): IObject[] {
    var roleType = associationType.roleType;

    for (const association of this.getMany(associationType.objectType)) {
      if (!association.canRead(roleType)) {
        continue;
      }

      if (association.IiAssociationForRole(roleType, role)) {
        return association.Object;
      }
    }
  }

  pushRequest(): PushRequest {
    return {
      n: this.newDatabaseStrategies?.map((v) => v.databasePushNew()),
      o: this.existingDatabaseStrategies.filter((v) => v.hasDatabaseChanges).map((v) => v.databasePushExisting()),
    };
  }

  pushResponse(pushResponse: PushResponse): void {
    if (pushResponse.n.length > 0) {
      for (const pushResponseNewObject of pushResponse.n) {
        var workspaceId = pushResponseNewObject.w;
        var databaseId = pushResponseNewObject.d;

        var strategy = this.strategyByWorkspaceId[workspaceId];

        this.newDatabaseStrategies.remove(strategy);
        this.removeStrategy(strategy);

        var databaseObject = this.database.pushResponse(databaseId, strategy.Class);
        strategy.databasePushResponse(databaseObject);

        this.existingDatabaseStrategies.add(strategy);
        this.addStrategy(strategy);
      }
    }

    if (this.newDatabaseStrategies?.length > 0) {
      throw new Error('Not all new objects received ids');
    }

    this.newDatabaseStrategies = null;
  }

  onDatabaseChange(state: DatabaseState): void {
    this.changedDatabaseStates ??= new Set();
    this.changedDatabaseStates.add(state);
  }

  onWorkspaceChange(state: WorkspaceState): void {
    this.changedWorkspaceStates ??= new Set();
    _ = this.changedWorkspaceStates.add(state);
  }

  instantiateDatabaseObject(identity: number): Strategy {
    var databaseObject = this.database.get(identity);
    var strategy = new Strategy(this, databaseObject);
    this.existingDatabaseStrategies.add(strategy);
    this.addStrategy(strategy);
    this.onInstantiate(strategy);

    return strategy;
  }

  instantiateWorkspaceObject(identity: number): Strategy {
    const cls = this.workspace.workspaceClassByWorkspaceId.get(identity);
    if (!cls) {
      return null;
    }

    var strategy = new Strategy(this, cls, identity);
    this.workspaceStrategies ??= new Set();
    this.workspaceStrategies.add(strategy);
    this.addStrategy(strategy);
    this.onInstantiate(strategy);

    return strategy;
  }

  onPull(pullResponse: PullResponse): Observable<IPullResult> {
    var syncRequest = this.database.diff(pullResponse);
    if (syncRequest.o.length > 0) {
      await this.sync(syncRequest);
    }

    for (const v in pullResponse.p) {
      if (!this.strategyByWorkspaceId.has(v.id)) {
        this.instantiateDatabaseObject(v.id);
      }
    }

    return new PullResult(this, pullResponse);
  }

  sync(syncRequest: SyncRequest): Observable<unknown> {
    var syncResponse = await this.database.sync(syncRequest);
    var securityRequest = this.database.syncResponse(syncResponse);

    if (securityRequest != null) {
      var securityResponse = await this.database.security(securityRequest);
      securityRequest = this.database.securityResponse(securityResponse);

      if (securityRequest != null) {
        securityResponse = await this.database.security(securityRequest);
        this.database.securityResponse(securityResponse);
      }
    }
  }

  onInstantiate(strategy: Strategy): void {
    this.instantiated ??= new Set();
    this.instantiated.add(strategy);
  }

  get(objectType: Composite): Strategy[] {
    var classes = new Set(objectType.classes);
    return this.strategyByWorkspaceId.filter((v) => classes.has(v.Value.Class)).Select((v) => v.Value);
  }

  addStrategy(strategy: Strategy): void {
    this.strategyByWorkspaceId.add(strategy.Id, strategy);

    var cls = strategy.class;

    let strategies: [] = this.strategiesByClass.get(cls);
    if (!strategies) {
      strategies = [strategy];
    } else {
      // TODO: Set
      strategies.add(strategies, strategy);
    }

    this.strategiesByClass[cls] = strategies;
  }

  removeStrategy(strategy: Strategy): void {
    this.strategyByWorkspaceId.delete(strategy.id);
    var cls = strategy.class;
    // TODO: Set
    this.strategiesByClass.get(cls)?.delete(strategy.id);
  }
}
