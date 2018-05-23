import { browser, by, element } from "protractor";
import { BasePage } from "./base.po";

export class MainPage extends BasePage {
  public navigateTo(): Promise<any> {
    return this.navigate("/");
  }
}
