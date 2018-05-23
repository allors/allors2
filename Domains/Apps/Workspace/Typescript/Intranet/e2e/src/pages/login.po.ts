import { browser, by, element } from "protractor";
import { BasePage } from "./base.po";

export class LoginPage extends BasePage {
  public async navigateTo(): Promise<any> {
    await browser.get("/login");
  }

  public async login(user: string, password: string) {
    await browser.get("/login");
    await element(await by.css("input[formControlName='userName']")).sendKeys(user);
    await element(await by.css("input[formControlName='password']")).sendKeys(password);
    await element(await by.buttonText("Sign In")).click();
  }
}
