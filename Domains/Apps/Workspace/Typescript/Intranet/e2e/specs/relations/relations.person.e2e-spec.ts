import Axios from "axios";

import { MetaDomain, Person } from "@allors";

import { Fixture } from "../../fixture";
import { RelationsPersonPage } from "../../pages/relations/relations.person.po";
import { Population } from "../../Population";

describe("/relations/person", () => {
  let fixture: Fixture;
  let m: MetaDomain;
  let page: RelationsPersonPage;
  let population: Population;

  beforeEach(async () => {
    fixture = new Fixture();
    m = fixture.m;
    await fixture.setup();
    page = new RelationsPersonPage();
    population = await fixture.load();
  });

  it("should have title", async () => {
    const people: Person[] = population.get(m.Person);
    const koen = people.find((v) => v.UserName === "Administrator");

    await page.navigateTo(koen.id);

    const title = await page.title;
    expect(title).toEqual("Person overview");
  });

  it("should have all fields set", async () => {
    const people: Person[] = population.get(m.Person);
    const koen = people.find((v) => v.UserName === "Administrator");

    await page.navigateTo(koen.id);

    await expect(page.FirstName.getAttribute("value")).toEqual("");
  });
});
