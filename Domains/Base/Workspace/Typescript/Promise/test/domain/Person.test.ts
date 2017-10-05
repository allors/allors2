import { Person, Session, workspace } from "../../src/allors/domain";
import { Database, Loaded, Scope } from "../../src/allors/promise";
import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";

describe("People",
    () => {
        let scope: Scope;

        beforeEach(async () => {
            const http = new AxiosHttp("http://localhost:5000/");
            await http.login("TestAuthentication/SignIn", "administrator");
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
