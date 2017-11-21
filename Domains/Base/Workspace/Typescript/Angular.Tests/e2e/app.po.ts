import { browser, by, element } from 'protractor';

export class AngularPage {
  navigateTo() {
    return browser.get('/');
  }

  login(user: string, password: string) {
    browser.get('/login');
    element(by.css('input[formControlName=\'userName\']')).sendKeys(user);
    element(by.css('input[formControlName=\'password\']')).sendKeys(password);
    element(by.buttonText('Sign In')).click();
  }

  get title() {
    return browser.getTitle();
  }
}
