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

        describe("with empty include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        include: {},
                    });

                    const json = JSON.stringify(fetch);
                    const include = JSON.parse(json).include;

                    assert.isArray(include);
                    assert.isEmpty(include);
                });
            });

        describe("with one role include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        include: {
                            Employees: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const include = JSON.parse(json).include;

                    assert.deepEqual(include, [
                        {
                            rt: "b95c7b34a295460082c8826cc2186a00",
                        },
                    ]);
                });
            });

        describe("with two roles include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        include: {
                            Employees: {},
                            Manager: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const include = JSON.parse(json).include;

                    assert.deepEqual(include, [
                        {
                            rt: "b95c7b34a295460082c8826cc2186a00",
                        },
                        {
                            rt: "19de0627fb1c4f559b6531d8008d0a48",
                        },
                    ]);
                });
            });

        describe("with a nested role include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        include: {
                            Employees: {
                                Photo: {},
                            },
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const include = JSON.parse(json).include;

                    assert.deepEqual(include, [
                        {
                            n: [{
                                rt: "f6624facdb8e4fb29e8618021b59d31d",
                            }],
                            rt: "b95c7b34a295460082c8826cc2186a00",
                        },
                    ]);
                });
            });

        describe("with a subclass role include",
            () => {
                it("should serialize to correct json", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Deletable({
                        id: "0",
                        include: {
                            Person_Photo: {},
                        },
                    });

                    const json = JSON.stringify(fetch);
                    const include = JSON.parse(json).include;

                    assert.deepEqual(include, [
                        {
                            rt: "f6624facdb8e4fb29e8618021b59d31d",
                        },
                    ]);
                });
            });

        describe("with a non exsiting role include",
            () => {
                it("should throw exception", () => {

                    const factory = new FetchFactory(metaPopulation);

                    const fetch = factory.Organisation({
                        id: "0",
                        include: {
                            Oops: {},
                        } as any,
                    });

                    assert.throw(() => {
                        const json = JSON.stringify(fetch);
                    }, Error);
                });
            });
    });
