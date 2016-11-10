using NUnit.Framework;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text;
using Roadkill.Core.Text.Menu;
using Roadkill.Core.Text.Sanitizer;
using Roadkill.Core.Text.TextMiddleware;

namespace Roadkill.Tests.Unit.Text.TextMiddleware
{
    [TestFixture]
    [Category("Unit")]
    public class HarmfulTagMiddlewareTests
    {
        [Test]
        public void should_handle_null_sanitizer()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }

		[Test]
		public void should_clean_html_using_sanitizer()
		{
			// Arrange
			string markdown = "<div onclick=\"javascript:alert('ouch');\">test</div>";
			string expectedHtml = "<div>test</div>";

			var pagehtml = new PageHtml() { Html = markdown };

			var factory = new HtmlSanitizerFactory(new ApplicationSettings() { UseHtmlWhiteList = true });
			var middleware = new HarmfulTagMiddleware(factory);

			// Act
			PageHtml actualPageHtml = middleware.Invoke(pagehtml);

			// Assert
			Assert.That(actualPageHtml.Html, Is.EqualTo(expectedHtml));
		}

		[Test]
        public void todo()
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