using FluentAssertions;
using Fizzler;
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
namespace  CRUD.TEST
{

    public  class IntegrationTestPerson  : IClassFixture<IntegrationWebApplicationTest>
    {
        private readonly HttpClient _httpClient;
        public IntegrationTestPerson(IntegrationWebApplicationTest factory)
        {
            _httpClient = factory.CreateClient();
        }
        [Fact]
        public async Task IndexShouldReturnView()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("/");
            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();   
            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;
            document.QuerySelectorAll("table.htmlDoc").Should().NotBeNull();
        }
    }
}
