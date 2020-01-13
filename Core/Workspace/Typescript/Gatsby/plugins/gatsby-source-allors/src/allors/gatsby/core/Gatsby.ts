import { MetaPopulation, Workspace, Pull, Filter, PullRequest } from "../../framework";
import { Database, Context } from "../../promise";
import { AxiosHttp } from "../../promise/core/http/AxiosHttp";
import { data, Meta, TreeFactory, FetchFactory, PullFactory } from "../../meta";
import { domain, Person } from '../../domain';
import { Mapper } from "./Mapper";

import { NodePluginArgs } from "gatsby"

export class Gatsby {

  http: AxiosHttp;
  metaPopulation: MetaPopulation;
  m: Meta;
  workspace: Workspace;
  mapper: Mapper;

  login: string;
  user: string;
  password: string;

  constructor(args: NodePluginArgs, { plugins, ...options }) {
    const url = options["url"]
    this.http = new AxiosHttp(url);
    this.login = options["login"];
    this.user = options["user"];
    this.password = options["password"];

    this.metaPopulation = new MetaPopulation(data);
    this.m = this.metaPopulation as Meta;
    this.workspace = new Workspace(this.metaPopulation);
    domain.apply(this.workspace);

    this.mapper = new Mapper(args, options);
  }

  public async sourceNodes() {

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
    ];

    ctx.session.reset();

    const loaded = await ctx
      .load(new PullRequest({ pulls }));

    this.mapper.map(loaded);
  }

}

