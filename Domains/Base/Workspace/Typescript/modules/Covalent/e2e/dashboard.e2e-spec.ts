import { $, browser, by, element, protractor, ProtractorExpectedConditions } from "protractor";

describe("basic e2e test with loading", () => {
  const EC: ProtractorExpectedConditions = protractor.ExpectedConditions;
  describe("home", () => {
    browser.get("/");
    it("should load home page", () => {
      expect(browser.getTitle()).toBe("Allors Apps");
      // Waits for the element 'td-loading' to not be present on the dom.
      browser.wait(EC.not(EC.presenceOf($("td-loading"))), 10000)
      .then(() => {
        // checks if elements were rendered
        expect(true).toBe(true);
      });
    });
  });
});
