import { AngularMaterialPage } from './app.po';

describe('Angular Material App', () => {
  let page: AngularMaterialPage;

  beforeEach(() => {
    page = new AngularMaterialPage();
  });

  it('should display welcome message', async () => {
    page.navigateTo();
    expect(await page.getTitle()).toEqual('Material');
  });
});
