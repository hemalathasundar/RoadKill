using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Text.Parsers.Links
{
	/// <summary>
	/// Responsible for parsing tokens inside links, e.g. special:, attachment:
	/// and also internal links. The class also handles CSS classes for external
	/// and broken links.
	/// </summary>
    public class LinkHrefParser
    {
        private readonly IPageRepository _pageRepository;
        private readonly ApplicationSettings _applicationSettings;
        private readonly List<string> _externalLinkPrefixes;
		private readonly UrlHelper _urlHelper;

		private static readonly Regex _querystringRegex = new Regex("(?<querystring>(\\?).+)", RegexOptions.IgnoreCase);
		private static readonly Regex _anchorRegex = new Regex("(?<hash>(#|%23).+)", RegexOptions.IgnoreCase);

		public LinkHrefParser(IPageRepository pageRepository, ApplicationSettings applicationSettings, UrlHelper urlHelper)
        {
	        if (pageRepository == null)
		        throw new ArgumentNullException(nameof(pageRepository));

			if (applicationSettings == null)
				throw new ArgumentNullException(nameof(applicationSettings));

			_pageRepository = pageRepository;
	        _applicationSettings = applicationSettings;
	        _urlHelper = urlHelper;

			_externalLinkPrefixes = new List<string>()
            {
                "http://",
                "https://",
                "www.",
                "mailto:",
                "#",
                "tag:"
            };
        }

		/// <summary>
		/// Handles internal links, and the 'attachment:' prefix for attachment links.
		/// </summary>
		public HtmlLinkTag Parse(HtmlLinkTag htmlLinkTag)
        {
            if (!_externalLinkPrefixes.Any(x => htmlLinkTag.OriginalHref.StartsWith(x)))
            {
                htmlLinkTag.IsInternalLink = true;

                // Parse internal links, including attachments, tag: and special: links
                string href = htmlLinkTag.OriginalHref;
                string lowerHref = href.ToLower();

                if (lowerHref.StartsWith("attachment:") || lowerHref.StartsWith("~/"))
                {
                    ConvertAttachmentHrefToFullPath(htmlLinkTag);
                }
                else if (lowerHref.StartsWith("special:"))
                {
                    ConvertSpecialPageHrefToFullPath(htmlLinkTag);
                }
                else
                {
                    ConvertInternalLinkHrefToFullPath(htmlLinkTag);
                }
            }
            else
            {
                // Add the external-link class to all outward bound links, 
                // although not for anchors pointing to <a name=""> tags on the current page.
				// (however # links shouldn't be treated as internal links)
                if (!htmlLinkTag.OriginalHref.StartsWith("#"))
                    htmlLinkTag.CssClass = "external-link";
            }

            return htmlLinkTag;
        }

        /// <summary>
        /// Updates the LinkEventArgs.Href to be a full path to the attachment
        /// </summary>
        private void ConvertAttachmentHrefToFullPath(HtmlLinkTag htmlLinkTag)
        {
            string href = htmlLinkTag.OriginalHref;
            string lowerHref = href.ToLower();

            if (lowerHref.StartsWith("attachment:"))
            {
                // Remove the attachment: part
                href = href.Remove(0, 11);
                if (!href.StartsWith("/"))
                    href = "/" + href;
            }
            else if (lowerHref.StartsWith("~/"))
            {
                // Remove the ~ 
                href = href.Remove(0, 1);
            }

            // Get the full path to the attachment
            string attachmentsPath = _applicationSettings.AttachmentsUrlPath;
            htmlLinkTag.Href = ConvertToAbsolutePath(attachmentsPath) + href;
        }

        /// <summary>
        /// Updates the LinkEventArgs.Href to be a full path to the Special: page
        /// </summary>
        private void ConvertSpecialPageHrefToFullPath(HtmlLinkTag htmlLinkTag)
        {
            string href = htmlLinkTag.OriginalHref;
            htmlLinkTag.Href = ConvertToAbsolutePath("~/wiki/" + href);
        }

        /// <summary>
        /// Updates the LinkEventArgs.Href to be a full path to the page, and the CssClass
        /// </summary>
        private void ConvertInternalLinkHrefToFullPath(HtmlLinkTag htmlLinkTag)
        {
            string href = htmlLinkTag.OriginalHref;

            // Parse internal links
            string title = href;
            string querystringAndAnchor = ""; // querystrings, #anchors

			// Parse querystrings
			if (_querystringRegex.IsMatch(href))
			{
				// Grab the querystring contents
				System.Text.RegularExpressions.Match match = _querystringRegex.Match(href);
				querystringAndAnchor = match.Groups["querystring"].Value;

				// Grab the url
				title = href.Replace(querystringAndAnchor, "");
			}
			else if (_anchorRegex.IsMatch(href))
            {
                // Grab the hash contents
                System.Text.RegularExpressions.Match match = _anchorRegex.Match(href);
                querystringAndAnchor = match.Groups["hash"].Value;

                // Grab the url
                title = href.Replace(querystringAndAnchor, "");
            }

            // For markdown, only urls with "-" in them are valid, spaces are ignored.
            // Remove these, so a match is made. No page title should have a "-" in?, so replacing them is ok.
            title = title.Replace("-", " ");

            // Find the page, or if it doesn't exist point to the new page url
            Page page = _pageRepository.GetPageByTitle(title);
            if (page != null)
            {
	            string encodedTitle = PageViewModel.EncodePageTitle(page.Title);
				href = GetInternalUrlForTitle(page.Id, encodedTitle);
                href += querystringAndAnchor;
            }
            else
            {
                href = GetNewPageUrlForTitle(href);
                htmlLinkTag.CssClass = "missing-page-link";
            }

            htmlLinkTag.Href = href;
            htmlLinkTag.Target = "";
        }

		/// <summary>
		/// Converts relative paths to absolute ones, e.g. ~/mydir/page1.html to /mywiki/mydir/page1.html.
		/// </summary>
		/// <returns>An absolute path for the resource.</returns>
		public string ConvertToAbsolutePath(string relativeUrl)
		{
			return _urlHelper.Content(relativeUrl);
		}

		/// <summary>
		/// Gets the internal url of a page based on the page title.
		/// </summary>
		public string GetInternalUrlForTitle(int id, string title)
		{
			return _urlHelper.Action("Index", "Wiki", new { id = id, title = title });
		}

		/// <summary>
		/// Gets a url to the new page resource, appending the title to the querystring.
		/// For example /pages/new?title=xyz
		/// </summary>
		public string GetNewPageUrlForTitle(string title)
		{
			return _urlHelper.Action("New", "Pages", new { title = title });
		}
	}
}