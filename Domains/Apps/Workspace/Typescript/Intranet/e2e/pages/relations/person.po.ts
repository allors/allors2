import { $, browser, by, element, ExpectedConditions, WebElementPromise } from "protractor";
import { BasePage } from "../base.po";
import { config } from "../config";

export class PersonPage extends BasePage {

  public async navigateTo(id: string): Promise<any> {
    const url = `/person/${id}`;
    await this.navigate(url);
  }

  public async save(): Promise<any> {
    const button = await browser.findElement(by.buttonText("SAVE"));
    await button.click();

    await browser.waitForAngular();
  }

  get FirstName(): WebElementPromise {
    return browser.findElement(by.css("input[placeholder='First Name']"));
  }
}
