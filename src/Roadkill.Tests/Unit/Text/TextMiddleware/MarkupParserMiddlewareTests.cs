using NUnit.Framework;
using Roadkill.Core.Cache;
using Roadkill.Core.Text;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.TextMiddleware;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text
{
    [TestFixture]
    [Category("Unit")]
    public class MarkupParserMiddlewareTests
    {
        private MocksAndStubsContainer _container;

        [SetUp]
        public void Setup()
        {
            _container = new MocksAndStubsContainer();
        }

        [Test]
        public void should_fire_beforeparse_in_textplugin()
        {
            // Arrange
            string markdown = "some **bold** text";
            string expectedHtml = "<p>some <strong>bold</strong> text</p>\n";

            var pagehtml = new PageHtml() {Html = markdown};

            var parser = new MarkdigParser();
            var middleware = new MarkupParserMiddleware(parser);

            // Act
            PageHtml actualPageHtml = middleware.Invoke(pagehtml);

            // Assert
            Assert.That(actualPageHtml.Html, Is.EqualTo(expectedHtml));
        }
    }
}