using NUnit.Framework;
using Roadkill.Core.Text.Parsers.Markdig;

namespace Roadkill.Tests.Unit.Text.Parsers.Markdig
{
    public class MarkdigParserTests
    {
        [Test]
        public void should_handle_empty_strings()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }

        [Test]
        public void should_parse_basic_markdown()
        {
            // given
            string expectedHtml = "<p><a href=\"http://www.google.com\" class=\"main\">i am a link</a></p>\n";
            string markdown = "[i am a link](http://www.google.com){.main}";
            var parser = new MarkdigParser();

            // when
            string html = parser.ToHtml(markdown);

            // then
            Assert.That(html, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void should()
        {
            // given

            // when

            // then
            Assert.Fail("fail");
        }
    }
}
