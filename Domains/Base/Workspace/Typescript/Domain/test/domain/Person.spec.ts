import { MetaPopulation, Person, Session, Workspace } from "@allors";
import { constructorByName } from "@generatedDomain/domain.g";
import { data, MetaDomain } from "@generatedMeta/meta.g";

import { assert } from "chai";
import "mocha";

describe("Person",
    () => {

        let session: Session;

        beforeEach(() => {
            const metaPopulation = new MetaPopulation(data);
            const workspace = new Workspace(metaPopulation, constructorByName);

            session = new Session(workspace);
        });

        describe("displayName",
            () => {
                let person: Person;

                beforeEach(() => {
                    person = session.create("Person") as Person;
                });

                it("should be N/A when nothing set", () => {
                    assert.equal(person.displayName, "N/A");
                });

                it("should be john@doe.com when username is john@doe.com", () => {
                    person.UserName = "john@doe.com";
                    assert.equal(person.displayName, "john@doe.com");
                });

                it("should be Doe when lastName is Doe", () => {
                    person.LastName = "Doe";
                    assert.equal(person.displayName, "Doe");
                });

                it("should be John with firstName John", () => {
                    person.FirstName = "John";
                    assert.equal(person.displayName, "John");
                });

                it("should be John Doe (even twice) with firstName John and lastName Doe", () => {
                    person.FirstName = "John";
                    person.LastName = "Doe";
                    assert.equal(person.displayName, "John Doe");
                    assert.equal(person.displayName, "John Doe");
                });
            },
        );

        describe("hello",
            () => {
                let person: Person;

                beforeEach(() => {
                    person = session.create("Person") as Person;
                });

                it("should be Hello John Doe when lastName Doe and firstName John", () => {
                    person.LastName = "Doe";
                    person.FirstName = "John";

                    assert.equal(person.hello(), "Hello John Doe");
                });
        });
});
