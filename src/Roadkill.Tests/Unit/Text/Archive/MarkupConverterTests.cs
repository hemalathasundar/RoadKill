using System;
using Roadkill.Core.Text.Parsers;
using Roadkill.Core.Text.Parsers.Images;
using Roadkill.Core.Text.Parsers.Links;

namespace Roadkill.Tests.Unit.Text.Archive
{
    public class MarkupParserMockZZ : IMarkupParser
    {
        public Func<HtmlImageTag, HtmlImageTag> ImageParsed { get; set; }
        public Func<HtmlLinkTag, HtmlLinkTag> LinkParsed { get; set; }

        public string ToHtml(string markdown)
        {
            return markdown;
        }
    }
}