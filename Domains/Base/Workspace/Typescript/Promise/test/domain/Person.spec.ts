import { MetaPopulation, Person, Session, Workspace } from "@allors";
import { AxiosHttp } from "@basePromise/http/AxiosHttp";
import { Database, Loaded, Scope } from "@basePromise/index";

import { constructorByName } from "@generatedDomain/domain.g";
import { data } from "@generatedMeta/meta.g";

import { assert } from "chai";
import "mocha";

describe("People",
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

                    // people.forEach((v) => console.log(v.FullName));
                });
        });
});
