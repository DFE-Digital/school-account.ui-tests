using DFE.SA.UITests.Models;
using DFE.SA.UITests.Pages;
using NUnit.Framework;
using TechTalk.SpecFlow.Assist;


namespace DFE.SA.UITests.StepDefinitions
{
    [Binding]
      public class LoginStepDefinitions
    {

        private readonly LoginPage _loginPage;

        public LoginStepDefinitions(LoginPage homepage)
        {
            _loginPage = homepage;

        }

        [Given(@"I navigate to the website home page")]
        public static void GivenINavigateToTheWebsiteHomePage()
        {
            Console.WriteLine("test");
        }

        [When(@"I login with a valid users credentials (.*) (.*)")]
        public async Task WhenILoginWithAValidUsersCredentials(string userName, string password)
        {
            await _loginPage.EnterPassword(password);
            await _loginPage.ClickContinue();
            await _loginPage.Login(userName, password);
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
            var isExist = await _loginPage.IsLatestNewsAndUpdatesExists();
             Assert.IsTrue(isExist);
            isExist.Should().BeTrue();

        }

        [Then(@"the user should not be logged in")]
        public static void ThenTheUserShouldNotBeLoggedIn()
        {
     
        }
    }
}
