import { AngularMaterialPage } from './app.po';

describe('angular-material App', () => {
  let page: AngularMaterialPage;

  beforeEach(() => {
    page = new AngularMaterialPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
