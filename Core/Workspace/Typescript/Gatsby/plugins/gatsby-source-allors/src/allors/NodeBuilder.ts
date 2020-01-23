import { MetaPopulation, Workspace, PullRequest } from "./framework";
import { Database, Context } from "./promise";
import { AxiosHttp } from "./promise/core/http/AxiosHttp";
import { data, Meta, PullFactory } from "./meta";
import { domain } from './domain';
import { NodeMapper } from "./gatsby/NodeMapper";

import { NodePluginArgs, PluginOptions } from "gatsby"

export class NodeBuilder {

  http: AxiosHttp;
  m: Meta;
  workspace: Workspace;
  mapper: NodeMapper;
  login: string;
  user: string;
  password: string;

  constructor(private metaPopulation: MetaPopulation, args: NodePluginArgs, { plugins, ...options }: PluginOptions) {
    const url = options["url"] as string
    this.login = options["login"] as string;
    this.user = options["user"] as string;
    this.password = options["password"] as string;

    this.http = new AxiosHttp(url);

    this.m = this.metaPopulation as Meta;

    this.workspace = new Workspace(this.metaPopulation);
    domain.apply(this.workspace);

    this.mapper = new NodeMapper(args, options);
  }

  public async build() {
    // Fetch objects
    await this.http.login(this.login, this.user, this.password);

    const database = new Database(this.http);
    const ctx = new Context(database, this.workspace);
    const pull = new PullFactory(this.m);

    const pulls = [
      pull.Organisation({
        include: {
          Owner: {},
          Employees: {},
        }
      }),
      pull.Person(),
      pull.Media(),
    ];

    ctx.session.reset();

    const loaded = await ctx
      .load(new PullRequest({ pulls }));

    // Map to Nodes
    this.mapper.map(loaded);
  }
}
