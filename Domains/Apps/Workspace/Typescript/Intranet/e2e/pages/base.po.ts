import { browser, by, element, ExpectedConditions } from "protractor";
import { config } from "./config";

export abstract class BasePage {

  public async navigate(route: string): Promise<any> {
    await browser.waitForAngular();
    await browser.executeScript(`
            var allors = window.allors;
            var ngZone = allors.ngZone;
            var router = allors.router;

            return ngZone.run(() => {
              router.navigate(["${route}"]);
            });
        `);
    await browser.wait(ExpectedConditions.urlContains(route), config.wait);
    await browser.waitForAngular();
  }

  public get title() {
    return browser.getTitle();
  }
}
