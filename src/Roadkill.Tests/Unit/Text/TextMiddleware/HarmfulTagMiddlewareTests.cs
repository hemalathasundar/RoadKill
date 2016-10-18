using Ganss.XSS;
using NUnit.Framework;
using Roadkill.Core.Cache;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.Sanitizer;
using Roadkill.Core.Text.TextMiddleware;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text
{
    [TestFixture]
    [Category("Unit")]
    public class HarmfulTagMiddlewareTests
    {
        [Test]
        public void should_clean_html_using_sanitizer()
        {
            // Arrange
            string markdown = "<div onclick=\"javascript:alert('ouch');\">test</div>";
            string expectedHtml = "<div>test</div>";

            var pagehtml = new PageHtml() {Html = markdown};

            var factory = new HtmlSanitizerFactory(new ApplicationSettings() { UseHtmlWhiteList = true });
            var middleware = new HarmfulTagMiddleware(factory);

            // Act
            PageHtml actualPageHtml = middleware.Invoke(pagehtml);

            // Assert
            Assert.That(actualPageHtml.Html, Is.EqualTo(expectedHtml));
        }
    }
}