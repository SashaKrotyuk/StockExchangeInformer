import { SEIPage } from './app.po';

describe('sei App', () => {
  let page: SEIPage;

  beforeEach(() => {
    page = new SEIPage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
