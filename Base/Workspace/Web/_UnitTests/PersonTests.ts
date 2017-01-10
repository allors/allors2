namespace Tests {

    export class PersonTests extends tsUnit.TestClass {

        displayName() {
            const workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            const session = new Allors.Session(workspace);

            const person = session.create("Person") as Person;

            this.areIdentical("N/A", person.displayName);

            person.UserName = "john@doe.com";

            this.areIdentical("john@doe.com", person.displayName);

            person.LastName = "Doe";

            this.areIdentical("Doe", person.displayName);

            person.FirstName = "John";

            this.areIdentical("John Doe", person.displayName);

            this.areIdentical("John Doe", person.displayName);
        }

        hello() {
            const workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            const session = new Allors.Session(workspace);

            const person = session.create("Person") as Person;
            
            person.LastName = "Doe";
            person.FirstName = "John";

            this.areIdentical("Hello John Doe", person.hello());
        }
    }
}