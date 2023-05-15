//using Microsoft.Playwright;

//namespace DFE.SA.UITests.Drivers
//{
//    public class Driver : IDisposable
//    {
//        private readonly Task<IPage> _page;
//        private IBrowser? _browser;

//        public Driver()
//        {
//            _page = InitializePlaywright();

//        }

//        public IPage Page => _page.Result;
//        //public IPage Page => _page.GetAwaiter().GetResult();

//            private async Task<IPage> InitializePlaywright()
//        {
//            //Playwright
//           var playwright = await Playwright.CreateAsync();

//            //Browser
//            _browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
//            {
//                Headless = false
//            });

//            return await _browser.NewPageAsync();

//        }

//        public void Dispose() => _browser?.CloseAsync();

//    }
//}
