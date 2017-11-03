import { Fixture } from "../fixture";
import { MainPage } from "../pages/main.po";
import { Population } from "../Population";

describe("/", () => {
  let fixture: Fixture;
  let page: MainPage;

  beforeEach(async () => {

    fixture = new Fixture();
    await fixture.setup();
    page = new MainPage();
  });

  it("should have title", async () => {

    await page.navigateTo();
    const title = await page.title;
    expect(title).toEqual("Allors Apps");
  });
});
