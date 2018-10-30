import { domain} from '../src/allors/domain';
import { MetaPopulation, Workspace } from '../src/allors/framework';
import { data, MetaDomain, TreeFactory, FetchFactory } from '../src/allors/meta';
import { Database, Scope } from '../src/allors/promise';

import { AxiosHttp } from '../src/allors/promise/base/http/AxiosHttp';

export class Fixture {

    metaPopulation: MetaPopulation;
    m: MetaDomain;
    scope: Scope;
    tree: TreeFactory;
    fetch: FetchFactory;

    async init() {
        this.metaPopulation = new MetaPopulation(data);
        this.m = this.metaPopulation.metaDomain;
        const workspace = new Workspace(this.metaPopulation);
        domain.apply(workspace);

        const http = new AxiosHttp('http://localhost:5000/');
        await http.login('TestAuthentication/Token', 'administrator');
        await http.get('Test/Setup');
        const database = new Database(http);
        this.scope = new Scope(database, workspace);

        this.tree = new TreeFactory(this.metaPopulation);
        this.fetch = new FetchFactory(this.metaPopulation);
    }
}
