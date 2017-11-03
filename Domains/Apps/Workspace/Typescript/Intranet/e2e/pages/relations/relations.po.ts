import { browser, by, element } from "protractor";
import { BasePage } from "../base.po";

export class RelationsPage extends BasePage {
  public async navigateTo(): Promise<any> {
    await browser.waitForAngular();
    return this.navigate("/relations");
  }
}
