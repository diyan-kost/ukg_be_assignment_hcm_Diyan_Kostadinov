using FluentAssertions;
using HCM.Infrastructure;
using HCM.Web.IntegrationTests.Helpers;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HCM.Web.IntegrationTests.Pages
{
    public class IndexTests : IClassFixture<HCMWebApplicationFactory<Program>>
    {
        private readonly HCMWebApplicationFactory<Program> _factory;

        public IndexTests(HCMWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Login_WhenCredentialsAreCorrect_ShouldRedirectToPersonalPage()
        {
            // Arrange
            using var client = _factory.CreateClient();

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<HCMContext>();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "LoginUserModel.Username", TestDbHelper.GetValidEmployeeUsername(db) },
                { "LoginUserModel.Password", "1" },
            });

            // Act
            var response = await client.PostAsync("/Index?handler=Login", content);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.RequestMessage!.RequestUri!.AbsolutePath.Should().StartWith("/Personal/");
        }

        [Fact]
        public async Task Login_WhenCredentialsAreIncorrect_ShouldStayOnIndexPage()
        {
            // Arrange
            using var client = _factory.CreateClient();

            using var scope = _factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<HCMContext>();

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "LoginUserModel.Username", "test326" },
                { "LoginUserModel.Password", "1" },
            });

            // Act
            var response = await client.PostAsync("/Index?handler=Login", content);

            var responseContent = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();
            document.LoadHtml(responseContent);

            var alertDiv = document.DocumentNode.SelectSingleNode("//div[@role='alert']");

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            response.RequestMessage!.RequestUri!.AbsolutePath.Should().Be("/Index");
            Assert.Contains("Username or password is incorrect", alertDiv.InnerText);
        }
    }
}
