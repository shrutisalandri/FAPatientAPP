import { WebextractsystemsPage } from './app.po';

describe('webextractsystems App', () => {
  let page: WebextractsystemsPage;

  beforeEach(() => {
    page = new WebextractsystemsPage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!');
  });
});
