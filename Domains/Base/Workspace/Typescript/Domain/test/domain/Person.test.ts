import { workspace, Session, Person } from "../../src/allors/domain";

import * as chai from "chai";

const expect = chai.expect;

describe("Person",
    () => {
        let session: Session;

        beforeEach(() => {
            session = new Session(workspace);
        });

        describe("displayName",
            () => {
                let person: Person;

                beforeEach(() => {
                    person = session.create("Person") as Person;
                });

                it("should be N/A when nothing set", () => {
                    expect(person.displayName()).to.equal("N/A");
                });

                it("should be john@doe.com when username is john@doe.com", () => {
                    person.UserName = "john@doe.com";
                    expect(person.displayName()).to.equal("john@doe.com");
                });

                it("should be Doe when lastName is Doe", () => {
                    person.LastName = "Doe";
                    expect(person.displayName()).to.equal("Doe");
                });

                it("should be John with firstName John", () => {
                    person.FirstName = "John";
                    expect(person.displayName()).to.equal("John");
                });

                it("should be John Doe (even twice) with firstName John and lastName Doe", () => {
                    person.FirstName = "John";
                    person.LastName = "Doe";
                    expect(person.displayName()).to.equal("John Doe");
                    expect(person.displayName()).to.equal("John Doe");
                });
            }
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

                    expect(person.hello()).to.equal("Hello John Doe");
                });
        });
});
