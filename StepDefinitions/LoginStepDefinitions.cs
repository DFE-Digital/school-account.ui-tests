using DFE.SA.UITests.Models;
using DFE.SA.UITests.Pages;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using Selenium.Axe;
using TechTalk.SpecFlow.Assist;
using Json.Net;
using Newtonsoft.Json;
using DFE.SA.UITests.Settings;
using Playwright.Axe;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;
using Microsoft.Playwright;
using Dfe.SchoolAccount.UiTests.Web.Pages;
using Microsoft.Playwright.NUnit;

namespace DFE.SA.UITests.StepDefinitions
{
    [Binding]
      public class LoginStepDefinitions
    {

        private readonly LoginPage _loginPage;
        private readonly IPage _page;
        private readonly GetSchoolBusinessInformationPage _getSchoolBusinessInformationPage;
  
        public LoginStepDefinitions(IPage page,LoginPage homepage, GetSchoolBusinessInformationPage getSchoolBusinessInformationPage)
        {
            _page = page;
            _loginPage = homepage;
            _getSchoolBusinessInformationPage = getSchoolBusinessInformationPage;
        }

        [Given(@"I navigate to the website home page")]
        public static void GivenINavigateToTheWebsiteHomePage()
        {
            
        }

        [When(@"I login with a valid users credentials (.*) (.*)")]
        public async Task WhenILoginWithAValidUsersCredentials(string userName, string password)
        {
            await _loginPage.EnterPassword(ConfigurationService.GetWebSettings().password);
            await _loginPage.ClickContinue();
            await _loginPage.Login(ConfigurationService.GetWebSettings().userName, ConfigurationService.GetWebSettings().password);
            await _loginPage.ClickLogin();

        }

        [When(@"I login with users credentials")]
        public async Task WhenILoginWithUsersCredentials(Table table)
        {
            var testData = table.CreateInstance<User>();
            //await _loginPage.EnterPassword(testData.Password);
            //await _loginPage.ClickContinue();

            await _loginPage.Login(testData.UserName, testData.Password);
            await _loginPage.ClickLogin();
        }

        [Then(@"the user should be logged in successfully")]
        public async Task ThenTheUserShouldBeLoggedInSuccessfully()
        {
            String title = await _getSchoolBusinessInformationPage.GetTitle();
            
            Assert.AreEqual(title, "Content page template – Get school business information – GOV.UK Prototype Kit");
           
            var isExist = await _getSchoolBusinessInformationPage.IsLatestNewsAndUpdatesExists();
            Assert.IsTrue(isExist);
            isExist.Should().BeTrue();

            AxeResults axeResults = await _page.RunAxe();
            Assert.AreEqual(0, axeResults.Violations.Count);
            Console.WriteLine($"Axe ran against testing {axeResults.Url} on {axeResults.Timestamp}.");

            await Helpers.Helpers.GetAxeRulesWithTags(_page);
            await Helpers.Helpers.GenerateAccessibilityReport(_page);

        }

        [Then(@"the user should not be logged in")]
        public static void ThenTheUserShouldNotBeLoggedIn()
        {
            
        }
    }
}
