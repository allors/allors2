import { browser, by, element } from "protractor";

export class AngularMaterialPage {
  public navigateTo() {
    return browser.get("/");
  }

  public getTitle() {
    return browser.getTitle();
  }
}
