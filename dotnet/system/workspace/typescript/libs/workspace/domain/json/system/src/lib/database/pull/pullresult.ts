import { IObject, IPullResult, ISession, IWorkspace, UnitTypes } from '@allors/workspace/domain/system';
import { PullResponse } from '@allors/protocol/json/system';
import { Result } from '../Result';

export class PullResult extends Result implements IPullResult {
  objects: Map<string, IObject>;

  collections: Map<string, IObject[]>;

  values: Map<string, UnitTypes>;

  workspace: IWorkspace;

  constructor(session: ISession, response: PullResponse) {
    super(session, response);

    this.workspace = session.workspace;

    this.objects = new Map(Object.keys(response.o).map((v) => [v, session.getOne<IObject>(response.o[v])]));
    this.collections = new Map(Object.keys(response.c).map((v) => [v, response.c[v].map((w) => session.getOne<IObject>(w))]));
    this.values = new Map(Object.keys(response.v).map((v) => [v, response.v[v]]));
  }
}
