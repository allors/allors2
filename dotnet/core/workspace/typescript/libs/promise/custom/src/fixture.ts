import { MetaPopulation } from '@allors/meta/system';
import { Meta, PullFactory, TreeFactory, FetchFactory, data } from '@allors/meta/generated';
import { Workspace } from '@allors/domain/system';

import '@allors/meta/core';
import { extend as extendDomain } from '@allors/domain/custom';

import { Context, AxiosHttp, Database } from '@allors/promise/core';

export class Fixture {

  readonly FULL_POPULATION = 'full';

  metaPopulation: MetaPopulation;
  m: Meta;
  ctx: Context;

  pull: PullFactory;
  tree: TreeFactory;
  fetch: FetchFactory;

  readonly x = new Object();

  private http: AxiosHttp;

  async init(population?: string) {

    this.http = new AxiosHttp({ baseURL: 'http://localhost:5000/allors/' });

    this.metaPopulation = new MetaPopulation(data);
    this.m = this.metaPopulation as Meta;
    const workspace = new Workspace(this.metaPopulation);
    extendDomain(workspace);

    await this.login('administrator');
    await this.http.get('Test/Setup', { population });
    const database = new Database(this.http);
    this.ctx = new Context(database, workspace);

    this.tree = new TreeFactory(this.m);
    this.fetch = new FetchFactory(this.m);
    this.pull = new PullFactory(this.m);

  }

  async login(user: string) {
    await this.http.login('TestAuthentication/Token', user);
  }
}
