import { workspace, Session, Person } from "../../src/domain";

import * as chai from "chai";

const expect = chai.expect;

describe("Person",
    () => {
        describe("displayName",
            () => {
                const session = new Session(workspace);

                const person = session.create("Person") as Person;

                describe("nothing set", () => {
                    it("should be N/A", () => {
                        expect(person.displayName()).to.equal("N/A");
                    });
                });

                describe("with username john@doe.com", () => {
                    beforeEach(() => {
                        person.UserName = "john@doe.com";
                    });

                    it("should be john@doe.com", () => {
                        expect(person.displayName()).to.equal("john@doe.com");
                    });
                });

                describe("with lastName Doe", () => {
                    beforeEach(() => {
                        person.LastName = "Doe";
                    });

                    it("should be Doe", () => {
                        expect(person.displayName()).to.equal("Doe");
                    });
                });

                describe("with firstName John", () => {
                    beforeEach(() => {
                        person.FirstName = "John";
                    });

                    it("should be John Doe", () => {
                        expect(person.displayName()).to.equal("John Doe");
                    });

                    it("should be John Doe when executed twice", () => {
                        expect(person.displayName()).to.equal("John Doe");
                    });
                });
            }
        );

        describe("hello",
            () => {
            const session = new Session(workspace);

            const person = session.create("Person") as Person;

            describe("with lastName Doe and firstName John", () => {
                person.LastName = "Doe";
                person.FirstName = "John";

                it("should be Hello John Doe", () => {
                    expect(person.hello()).to.equal("Hello John Doe");
                });
            });
        });
});
