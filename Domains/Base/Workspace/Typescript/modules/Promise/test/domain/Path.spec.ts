import { domain, Organisation, Person } from "../../src/allors/domain";
import { Fetch, MetaPopulation, PullRequest, Session, Workspace } from "../../src/allors/framework";
import { data, FetchFactory, FetchPerson, IncludeLanguage, IncludePerson } from "../../src/allors/meta";
import { Database, Loaded, Scope } from "../../src/allors/promise";

import { AxiosHttp } from "../../src/allors/promise/base/http/AxiosHttp";

import { assert } from "chai";
import "mocha";

describe("Path",
    () => {
        let metaPopulation: MetaPopulation;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);
        });

        describe("with empty step",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        path: {},
                    });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.isUndefined(path);
                });
            });

        describe("with one role",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        path: {
                            Employees: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.deepEqual(path, { step: "b95c7b34a295460082c8826cc2186a00" });
                });
            });

        describe("with two roles",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch =
                        factory.Organisation({
                            id: "0",
                            path: {
                                Employees: {
                                    Photo: {},
                                },
                            },
                        });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.deepEqual(path, {
                        next: {
                            step: "f6624facdb8e4fb29e8618021b59d31d",
                        },
                        step: "b95c7b34a295460082c8826cc2186a00",
                    });

                });
            });

        describe("with a subclass role",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.User({
                        id: "0",
                        path: {
                            Organisation_CycleOne: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.deepEqual(path, { step: "9033ae7383f645299f8184fd9d35d597" });
                });
            });

        describe("with a non exsiting role",
            () => {
                it("should throw exception", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        path: {
                            Oops: {},
                        } as any,
                    });

                    assert.throw(() => {
                        const json = JSON.stringify(fetch);
                    }, Error);
                });
            });

        describe("with one association",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        path: {
                            PeopleWhereCycleOne: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.deepEqual(path, { step: "dec66a7b56f54010a2e737e25124bc77" });
                });
            });
        describe("with one subclass association",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Deletable({
                        id: "0",
                        path: {
                            Organisation_PeopleWhereCycleOne: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.deepEqual(path, { step: "dec66a7b56f54010a2e737e25124bc77" });
                });
            });
    });
