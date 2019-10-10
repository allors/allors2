/// <reference path="./tsUnit.ts" />
module Tests {

  export class PersonTests extends tsUnit.TestClass {

        population: Extra.Population;

        setUp() {
            this.population = new Extra.Population();
        }


        testFullName() {
            var session = this.population.session;

            var jane = session.create("Person") as Allors.Domain.Person;
            jane.FirstName = "Jane";
            jane.LastName = "Doe";

            this.isTrue(jane.FullName === "Jane Doe");
        }
    }
}
