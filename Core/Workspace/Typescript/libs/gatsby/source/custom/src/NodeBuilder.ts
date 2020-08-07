import { NodePluginArgs, PluginOptions } from 'gatsby';

import { AxiosHttp, Database, Context, Loaded } from '@allors/promise/core';
import { Meta, PullFactory } from '@allors/meta/generated';
import { Workspace } from '@allors/domain/system';
import { NodeMapper, ContentDigests } from '@allors/gatsby/source/core';
import { PullRequest } from '@allors/protocol/system';
import { Media } from '@allors/domain/generated';

export class NodeBuilder {
  http: AxiosHttp;
  m: Meta;
  contentDigests: ContentDigests;
  mapper: NodeMapper;
  login: string;
  user: string;
  password: string;

  constructor(private workspace: Workspace, args: NodePluginArgs, { plugins, ...options }: PluginOptions) {

    this.m = workspace.metaPopulation as Meta;

    const baseURL = options['url'] as string;
    this.login = options['login'] as string;
    this.user = options['user'] as string;
    this.password = options['password'] as string;

    this.http = new AxiosHttp({
      baseURL,
      // httpAgent: new http.Agent({ keepAlive: true }),
      // httpsAgent: new https.Agent({ keepAlive: true }),
      maxContentLength: 50 * 1000 * 1000,
      maxRedirects: 10,
      timeout: 10 * 60000,
    });

    this.mapper = new NodeMapper(args);
    this.contentDigests = new ContentDigests(args);
  }

  public async build(): Promise<void> {
    const loaded = await this.Load();
    this.contentDigests.onLoad(loaded);

    const { session } = loaded;

    // Filter
    const allObjectIds = loaded.response.objects.map((v) => v[0]);
    const allObjects = allObjectIds.map((v) => session.get(v));
    const gatsbyObjects = allObjects.filter((v) => v.objectType._isGatsby);

    const objects = gatsbyObjects.filter((v) => {
      // Media: should be an image
      if (v.objectType === this.m.Media) {
        const media = v as Media;
        if (media.isImage) {
          const isSupported = media.isImage;
          return isSupported;
        }
      }

      return true;
    });

    objects.forEach((v) => {
      if (v.objectType === this.m.Person) {
        // TODO: Do some postprocessing
      }
    });

    this.mapper.map(objects, this.contentDigests);
  }

  private async Load(): Promise<Loaded> {
    await this.http.login(this.login, this.user, this.password);
    const database = new Database(this.http);
    const ctx = new Context(database, this.workspace);
    const pull = new PullFactory(this.m);

    const pulls = [
      pull.Organisation({
        include: {
          Owner: {},
          Employees: {},
        },
      }),
      pull.Person(),
      pull.Media(),
    ];

    const loaded = await ctx.load(new PullRequest({ pulls }));
    return loaded;
  }
}
