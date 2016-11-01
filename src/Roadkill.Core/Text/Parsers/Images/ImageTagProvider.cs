using System.Text.RegularExpressions;
using System.Web;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text.Parsers.Links;

namespace Roadkill.Core.Text.Parsers.Images
{
    public class ImageTagProvider
    {
        private ApplicationSettings _applicationSettings;
        private static Regex _imgFileRegex = new Regex("^File:", RegexOptions.IgnoreCase);

        public UrlResolver UrlResolver { get; set; }

        public ImageTagProvider(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;

            // Create the UrlResolver to resolve all wiki urls
            HttpContextBase httpContext = null;
            if (HttpContext.Current != null)
                httpContext = new HttpContextWrapper(HttpContext.Current);

            UrlResolver = new UrlResolver(httpContext);
        }

        /// <summary>
        /// Adds the attachments folder as a prefix to all image URLs before the HTML &lt;img&gt; tag is written.
        /// </summary>
        public HtmlImageTag Parse(HtmlImageTag htmlImageTag)
        {
            if (!htmlImageTag.OriginalSrc.StartsWith("http://") && !htmlImageTag.OriginalSrc.StartsWith("https://") && !htmlImageTag.OriginalSrc.StartsWith("www."))
            {
                string src = htmlImageTag.OriginalSrc;
                src = _imgFileRegex.Replace(src, "");

                string attachmentsPath = _applicationSettings.AttachmentsUrlPath;
                string urlPath = attachmentsPath + (src.StartsWith("/") ? "" : "/") + src;
                htmlImageTag.Src = UrlResolver.ConvertToAbsolutePath(urlPath);
            }

            return htmlImageTag;
        }
    }
}