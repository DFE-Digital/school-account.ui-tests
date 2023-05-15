﻿using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using DFE.SA.UITests.Settings;
using Microsoft.Playwright;
using System.Reflection;
using static System.Net.Mime.MediaTypeNames;

namespace DFE.SA.UITests.Hooks
{
    [Binding]
    public class TestHooks
    {
        private readonly IObjectContainer _objectContainer;
        private static ExtentReports? extent;
        private static ExtentTest? scenario;
        private static ExtentTest? featureName;
        private static string? startUpPath;
        private static string? dirname;

        public TestHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;

        }
        [BeforeTestRun]
        public static void OneTimeSetUp()
        {
            
            startUpPath = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName;
            dirname = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(dirname);
            htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.Config.DocumentTitle = "Test Report | StudentAccount";
            htmlReporter.Config.ReportName = "StudentAccount";
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);

        }

        [AfterTestRun]
        public static void TearDownReport()
        {
            //Flush report once test completes
            extent.Flush();
        }

        [BeforeFeature]
        [Obsolete]
        public static void BeforeFeature()
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);
        }

        [AfterStep]
        [Obsolete]
        public static void InsertReportingSteps()
        {
            featureName.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);

            var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            
            switch (ScenarioContext.Current.TestError)
            {
                case null:
                    if (stepType == "Given")
                        scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "When")
                        scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "Then")
                        scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                    else if (stepType == "And")
                        scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
                    break;
                default:
                    if (ScenarioContext.Current.TestError != null)
                    {
                        string screenShotPath = startUpPath + "\\Screenshots\\Test.png";
                        if (stepType == "Given")
                            scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message,
                                MediaEntityBuilder.CreateScreenCaptureFromPath(screenShotPath).Build());
                        else if (stepType == "When")
                            scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.InnerException,
                                 MediaEntityBuilder.CreateScreenCaptureFromPath(screenShotPath).Build());
                        else if (stepType == "Then")
                            scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message,
                                 MediaEntityBuilder.CreateScreenCaptureFromPath(screenShotPath).Build());
                    }

                    break;
            }
            
        }


        [BeforeScenario]
        [Obsolete]
        public async Task BeforeScenarioAsync()
        {
            DeleteExistingFilesInDirectory();
            //Create dynamic scenario name
            scenario = featureName.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);
            var playwright = await Playwright.CreateAsync();
            var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = ConfigurationService.GetWebSettings().HeadLess,
                SlowMo = ConfigurationService.GetWebSettings().SlowMo,

            });
            var context = await browser.NewContextAsync(new()
            {
                RecordVideoDir = startUpPath + "\\videos/"

            });
        
            var page = await context.NewPageAsync();

            await page.GotoAsync(ConfigurationService.GetWebSettings().BaseUrl);
            await Helpers.Helpers.Screenshot(page);
            _objectContainer.RegisterInstanceAs(browser);
            _objectContainer.RegisterInstanceAs(page);
        }


        [AfterScenario]
        public async Task AfterScenario()
        {
            var browser = _objectContainer.Resolve<IBrowser>();
            await browser.CloseAsync();
        }

        private static void DeleteExistingFilesInDirectory()
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
