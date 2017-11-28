import Axios from "axios";

import { MetaDomain, Person } from "@allors";

import { Fixture } from "../../fixture";
import { RelationsPage } from "../../pages/relations/relations.po";
import { Population } from "../../Population";

import { browser } from "protractor";

describe("/relations", () => {
  let fixture: Fixture;
  let m: MetaDomain;
  let page: RelationsPage;

  beforeEach(async () => {
    fixture = new Fixture();
    m = fixture.m;
    await fixture.setup();
    page = new RelationsPage();
  });

  it("should have title", async () => {

    await page.navigateTo();
    const title = await page.title;
    expect(title).toEqual("Relations Dashboard");
  });
});
