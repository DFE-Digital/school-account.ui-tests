using Microsoft.Playwright;

namespace DFE.SA.UITests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        private readonly ILocator _lnkLogin;
        private readonly ILocator _continue;
        private readonly ILocator _latestNewsAndUpdates;

        public LoginPage(IPage page)
        {
            _page = page;
            _lnkLogin = _page.GetByRole(AriaRole.Link, new() { Name = "Sign in" });
            _continue = _page.GetByRole(AriaRole.Button, new() { Name = "Continue" });
            _latestNewsAndUpdates = _page.GetByRole(AriaRole.Heading, new() { Name = "Latest news and updates" });
        }

        private async Task EnterUserName(string userName) => await _page.FillAsync("id=event-name", userName);

        public async Task EnterPassword(string Password) => await _page.FillAsync("id=password", Password);


        public async Task ClickLogin() => await _lnkLogin.ClickAsync();

        public async Task ClickContinue() => await _continue.ClickAsync();
        public async Task Login(string userName, string password)
        {
            await EnterUserName(userName);
            await Helpers.Helpers.Screenshot(_page);
            await EnterPassword(password);
            await Helpers.Helpers.Screenshot(_page);

        }

        public async Task<bool> IsLatestNewsAndUpdatesExists()
        {
            await Helpers.Helpers.Screenshot(_page);
            return await _latestNewsAndUpdates.IsVisibleAsync();
        }

   }
}
