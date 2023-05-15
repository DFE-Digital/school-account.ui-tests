using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFE.SA.UITests.Helpers
{
    public class Helpers
    {
        public static async Task Screenshot(IPage page)
        {
            var date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var title = await page.TitleAsync();
            string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName+$"\\Screenshot\\{date}_{title}-.png";
            var so = new PageScreenshotOptions()
            {
                Path = path,
                FullPage = true,
            };
            await page.ScreenshotAsync(so);
        }

    }
}
