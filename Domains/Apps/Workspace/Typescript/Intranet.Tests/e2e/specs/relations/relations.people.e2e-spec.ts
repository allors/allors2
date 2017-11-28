import Axios from "axios";

import { MetaDomain, Person } from "@allors";

import { Fixture } from "../../fixture";
import { RelationsPeoplePage } from "../../pages/relations/relations.people.po";
import { Population } from "../../Population";

describe("/relations/people", () => {
  let fixture: Fixture;
  let m: MetaDomain;
  let page: RelationsPeoplePage;

  beforeEach(async () => {
    fixture = new Fixture();
    m = fixture.m;
    await fixture.setup();
    page = new RelationsPeoplePage();
  });

  it("should have title", async () => {
    await page.navigateTo();
    const title = await page.title;
    expect(title).toEqual("People");
  });
});
