using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Playwright.Axe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using static System.Net.Mime.MediaTypeNames;

namespace DFE.SA.UITests.Helpers
{
    public class Helpers : PageTest
    {
        private static string? startUpPath;
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

        public static async Task GetAxeRules(IPage page)
        {
            IList<AxeRuleMetadata> axeRules = await page.GetAxeRules();
            foreach (var rule in axeRules)
            {
                Console.WriteLine($"Rule name: {rule.RuleId} Help: {rule.Help} HelpUrl: {rule.HelpUrl}");
                Console.WriteLine($"Tags: {string.Join(", ", rule.Tags)}");
            }
        }

        public static async Task GetAxeRulesWithTags(IPage page)
        {
            // Only take rules which are wcag2aa or wcag2a.
            IList<string> tags = new List<string>() { "wcag21aa", "wcag2a", "wcag22aa" };

            IList<AxeRuleMetadata> axeRules = await page.GetAxeRules(tags);
            foreach (var rule in axeRules)
            {
                Console.WriteLine($"Rule name: {rule.RuleId} Help: {rule.Help} HelpUrl: {rule.HelpUrl}");
                Console.WriteLine($"Tags: {string.Join(", ", rule.Tags)}");
            }
        }

        public static async Task RunRulesAgainstTheCurrentStateOfThePage(IPage page)
        {
            AxeResults axeResults = await page.RunAxe();

            Console.WriteLine($"Axe ran against {axeResults.Url} on {axeResults.Timestamp}.");

            Console.WriteLine($"Rules that failed:");
            foreach (var violation in axeResults.Violations)
            {
                Console.WriteLine($"Rule Id: {violation.Id} Impact: {violation.Impact} HelpUrl: {violation.HelpUrl}.");

                foreach (var node in violation.Nodes)
                {
                    Console.WriteLine($"\tViolation found in Html: {node.Html}.");

                    foreach (var target in node.Target)
                    {
                        Console.WriteLine($"\t\t{target}.");
                    }
                }
            }

            Console.WriteLine($"Rules that passed successfully:");
            foreach (var pass in axeResults.Passes)
            {
                Console.WriteLine($"Rule Id: {pass.Id} Impact: {pass.Impact} HelpUrl: {pass.HelpUrl}.");
            }

            Console.WriteLine($"Rules that did not fully run:");
            foreach (var incomplete in axeResults.Incomplete)
            {
                Console.WriteLine($"Rule Id: {incomplete.Id}.");
            }

            Console.WriteLine($"Rules that were not applicable:");
            foreach (var inapplicable in axeResults.Inapplicable)
            {
                Console.WriteLine($"Rule Id: {inapplicable.Id}.");
            }
        }

        public static async Task<AxeRunOptions> AxeRunOptionsWithParameter(IPage page, String tag, bool colourContrast)
        {
            AxeRunOptions options = new AxeRunOptions(
            // Run with tags.
            runOnly: new AxeRunOnly(AxeRunOnlyType.Tag, new List<string> { tag }),

            // Specify rules.
            rules: new Dictionary<string, AxeRuleObjectValue>()
            {
                // Don't run colour-contrast.
                {"color-contrast", new AxeRuleObjectValue(colourContrast)}
            },

            // Limit result types to Violations.
            resultTypes: new List<AxeResultGroup>()
            {
                AxeResultGroup.Violations
            },

            // Don't return css selectors in results.
            selectors: false,

            // Return CSS selector for elements, with all the element's ancestors.
            ancestry: true,

            // Don't return xpath selectors for elements.
            xpath: false,

            // Don't run axe on iframes inside the document.
            iframes: false);
                 
            return options;

            //AxeResults axeResults = await page.RunAxe(options);
            //axeResults = await page.RunAxe(context, options);
            //axeResults = await locator.RunAxe(options);
        }

        public static async Task GenerateAccessibilityReport(IPage page)
        {
            startUpPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            AxeHtmlReportOptions reportOptions = new(reportDir: startUpPath + "\\AccessibilityReport/");
            AxeResults axeResults1 = await page.RunAxe(reportOptions: reportOptions);
        }

        public override BrowserNewContextOptions ContextOptions()
        {
            var options = base.ContextOptions() ?? new();
            options.RecordVideoDir = "videos";

            return options;
        }


        public static void DeleteExistingFilesInDirectory()
        {
            DirectoryInfo di = new DirectoryInfo(startUpPath + "\\videos/");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
        }

    }
}
