import { AngularPage } from './app.po';

describe('angular App', () => {
  let page: AngularPage;

  beforeEach(() => {
    page = new AngularPage();
    page.login('john@doe.org', 'john');
  });

  it('should have title Dashboard', () => {
    page.navigateTo();

    expect('Dashboard').toEqual(page.title);
  });
});
