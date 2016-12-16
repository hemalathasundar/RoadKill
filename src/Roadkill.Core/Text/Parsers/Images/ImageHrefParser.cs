using System;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text.Parsers.Links;

namespace Roadkill.Core.Text.Parsers.Images
{
    public class ImageHrefParser
    {
        private readonly ApplicationSettings _applicationSettings;
        private static readonly Regex _imgFileRegex = new Regex("^File:", RegexOptions.IgnoreCase);
	    private readonly UrlHelper _urlHelper;

        public ImageHrefParser(ApplicationSettings applicationSettings, UrlHelper urlHelper)
        {
			if (applicationSettings == null)
				throw new ArgumentNullException(nameof(applicationSettings));

			_applicationSettings = applicationSettings;
	        _urlHelper = urlHelper;
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
                htmlImageTag.Src = ConvertToAbsolutePath(urlPath);
            }

            return htmlImageTag;
        }

		/// <summary>
		/// Converts relative paths to absolute ones, e.g. ~/mydir/page1.html to /mywiki/mydir/page1.html.
		/// </summary>
		/// <returns>An absolute path for the resource.</returns>
		private string ConvertToAbsolutePath(string relativeUrl)
		{
			return _urlHelper.Content(relativeUrl);
		}
	}
}