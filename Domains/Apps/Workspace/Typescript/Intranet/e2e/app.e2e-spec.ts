import { AngularPage } from './app.po';

describe('angular App', () => {
  let page: AngularPage;

  beforeEach(() => {
    page = new AngularPage();
    page.login('john@doe.org', 'john');
  });

  it('should have title Allors Apps', async () => {
    page.navigateTo();
    const title = await page.title;
    expect('Allors Apps').toEqual(title);
  });
});
