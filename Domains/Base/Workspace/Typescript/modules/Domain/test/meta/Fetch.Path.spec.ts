import { domain, Organisation, Person } from "../../src/allors/domain";
import { Fetch, MetaPopulation, PullRequest, Session, Workspace } from "../../src/allors/framework";
import { data, FetchFactory, FetchPerson, IncludeLanguage, IncludePerson } from "../../src/allors/meta";

import { assert } from "chai";
import "mocha";

describe("Fetch",
    () => {
        let metaPopulation: MetaPopulation;

        beforeEach(async () => {
            metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation);
            domain.apply(workspace);
        });

        describe("with empty step path",
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

        describe("with one role path",
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

        describe("with two roles path",
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

        describe("with a subclass role path",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.User({
                        id: "0",
                        path: {
                            Person_CycleOne: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const path = JSON.parse(json).path;

                    assert.deepEqual(path, { step: "79ffeed6e06a42f4b12fd7f7c98b6499" });
                });
            });

        describe("with a non exsiting role path",
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

        describe("with one association path",
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
        describe("with one subclass association path",
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
