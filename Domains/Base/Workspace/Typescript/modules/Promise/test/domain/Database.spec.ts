import { Person } from "../../src/allors/domain";
import { constructorByName } from "../../src/allors/domain";
import { MetaPopulation, Session, Workspace } from "../../src/allors/framework";
import { data } from "../../src/allors/meta";
import { Database, Loaded, Scope } from "../../src/allors/promise";

import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";

describe("Database",
    () => {
        let scope: Scope;

        beforeEach(async () => {
            const metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation, constructorByName);

            const http = new AxiosHttp("http://localhost:5000/");
            await http.login("TestAuthentication/Token", "administrator");
            const database = new Database(http);
            scope = new Scope(database, workspace);
        });

        describe("Load",
            () => {
                it("should return all people", async () => {
                    const loaded: Loaded = await scope.load("People");
                    const people = loaded.collections.people as Person[];
                    assert.equal(7, people.length);
                });
        });
});
