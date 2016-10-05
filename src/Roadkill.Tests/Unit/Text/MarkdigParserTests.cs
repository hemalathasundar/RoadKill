using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Roadkill.Core.Text.Parsers;
using Roadkill.Core.Text.Parsers.Markdig;

namespace Roadkill.Tests.Unit.Text
{
    public class MarkdigParserTests
    {
        [Test]
        public void should()
        {
            // given
            var parser = new MarkdigParser();
            parser.LinkParsed += Parser_LinkParsed;

            // when
            string html = parser.ToHtml("[i am a link](http://www.google.com){.main}");

            // then
            Console.WriteLine(html);
        }

        private void Parser_LinkParsed(object sender, Core.Converters.HtmlLinkTag e)
        {
            e.Href = "lol";
        }
    }
}
