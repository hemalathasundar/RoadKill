using Ganss.XSS;

namespace Roadkill.Core.Text.TextMiddleware
{
    public class HarmfulTagMiddleware : Middleware
    {
        private HtmlSanitizer _sanitizer;

        public HarmfulTagMiddleware(HtmlSanitizer sanitizer)
        {
            _sanitizer = sanitizer;
        }

        public override PageHtml Invoke(PageHtml pageHtml)
        {
            pageHtml.Html = _sanitizer.Sanitize(pageHtml.Html);
            return pageHtml;
        }
    }
}