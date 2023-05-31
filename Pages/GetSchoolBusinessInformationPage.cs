using DFE.SA.UITests.Helpers;
using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.SchoolAccount.UiTests.Web.Pages
{
    public class GetSchoolBusinessInformationPage
    {
        private readonly IPage _page;
        private readonly ILocator _latestNewsAndUpdates;
        public GetSchoolBusinessInformationPage(IPage page)
        {
            _page = page;
            _latestNewsAndUpdates = _page.GetByRole(AriaRole.Heading, new() { Name = "Latest news and updates" });
        }

        
        public async Task<string> GetTitle() => await _page.TitleAsync();
           

        public async Task<bool> IsLatestNewsAndUpdatesExists()
        {
            await Helpers.Screenshot(_page);
            return await _latestNewsAndUpdates.IsVisibleAsync();
        }
    }
}
