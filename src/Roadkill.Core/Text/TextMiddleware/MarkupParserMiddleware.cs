using Roadkill.Core.Text.Parsers.Markdig;

namespace Roadkill.Core.Text.TextMiddleware
{
    public class MarkupParserMiddleware : Middleware
    {
        private MarkdigParser _parser;

        public MarkupParserMiddleware(MarkdigParser parser)
        {
            _parser = parser;
        }

        public override PageHtml Invoke(PageHtml pageHtml)
        {
            pageHtml.Html = _parser.ToHtml(pageHtml.Html);
            return pageHtml;
        }
    }
}