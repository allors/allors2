module Extra {
   
    export class Population {
        workspace: Allors.Workspace;
        session: Allors.Session;

        john: Allors.Domain.Person;

        constructor() {
            this.workspace = new Allors.Workspace(Allors.Protocol.metaPopulation);
            this.session = new Allors.Session(this.workspace);

            this.john = this.createPerson("John");
        }

        createPerson(firstName: string): Allors.Domain.Person {
            var person = <Allors.Domain.Person>this.session.create("Person");
            person.FirstName = firstName;
            return person;
        }
    }
}
