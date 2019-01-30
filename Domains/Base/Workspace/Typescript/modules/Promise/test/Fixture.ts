import { domain } from '../src/allors/domain';
import { MetaPopulation, Workspace } from '../src/allors/framework';
import { data, Meta, TreeFactory, FetchFactory, PullFactory } from '../src/allors/meta';
import { Database, Scope } from '../src/allors/promise';

import { AxiosHttp } from '../src/allors/promise/base/http/AxiosHttp';

export class Fixture {

    readonly FULL_POPULATION = 'full';

    metaPopulation: MetaPopulation;
    m: Meta;
    scope: Scope;

    pull: PullFactory;
    tree: TreeFactory;
    fetch: FetchFactory;

    readonly x = new Object();

    async init(population?: string) {

        this.metaPopulation = new MetaPopulation(data);
        this.m = this.metaPopulation as Meta;
        const workspace = new Workspace(this.metaPopulation);
        domain.apply(workspace);

        const http = new AxiosHttp('http://localhost:5000/');
        await http.login('TestAuthentication/Token', 'administrator');
        await http.get('Test/Setup', { population });
        const database = new Database(http);
        this.scope = new Scope(database, workspace);

        this.pull = new PullFactory(this.metaPopulation);
        this.tree = new TreeFactory(this.metaPopulation);
        this.fetch = new FetchFactory(this.metaPopulation);
    }
}
