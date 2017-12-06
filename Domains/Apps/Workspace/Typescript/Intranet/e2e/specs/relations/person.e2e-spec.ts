import Axios from "axios";

import { MetaDomain, Person } from "../../allors";

import { Fixture } from "../../fixture";
import { PersonPage } from "../../pages/relations/person.po";
import { Population } from "../../Population";

import { browser, by, element, ExpectedConditions } from "protractor";
import { config } from "../../pages/config";

describe("/person", () => {
  let fixture: Fixture;
  let m: MetaDomain;
  let page: PersonPage;
  let population: Population;

  beforeEach(async () => {
    fixture = new Fixture();
    m = fixture.m;
    await fixture.setup();
    page = new PersonPage();

    population = await fixture.load([m.Person.ObjectType]);
  });

  it("should have title", async () => {
    const people: Person[] = population.get(m.Person);
    const administrator = people.find((v) => v.UserName === "Administrator");

    await page.navigateTo(administrator.id);

    const title = await page.title;
    expect(title).toEqual("Person");
  });

  it("should allow update", async () => {
    const people: Person[] = population.get(m.Person);
    const administrator = people.find((v) => v.UserName === "Administrator");

    await page.navigateTo(administrator.id);

    await page.FirstName.clear();
    await page.FirstName.sendKeys("Conan");

    await page.save();

    await fixture.load([m.Person.ObjectType]);

    fixture.scope.session.reset();

    expect(administrator.FirstName).toEqual("Conan");
  });
});
