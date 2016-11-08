using System.Web;
using System.Web.Mvc;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Text.Parsers.Links
{
	public class UrlResolver
	{
		private readonly UrlHelper _urlHelper;

		public UrlResolver(UrlHelper urlHelper)
		{
			_urlHelper = urlHelper;
		}

		/// <summary>
		/// Converts relative paths to absolute ones, e.g. ~/mydir/page1.html to /mywiki/mydir/page1.html.
		/// </summary>
		/// <returns>An absolute path for the resource.</returns>
		public virtual string ConvertToAbsolutePath(string relativeUrl)
		{
			return _urlHelper.Content(relativeUrl);
		}

		/// <summary>
		/// Gets the internal url of a page based on the page title.
		/// </summary>
		public virtual string GetInternalUrlForTitle(int id, string title)
		{
			return _urlHelper.Action("Index", "Wiki", new { id = id, title = PageViewModel.EncodePageTitle(title) });
		}

		/// <summary>
		/// Gets a url to the new page resource, appending the title to the querystring.
		/// For example /pages/new?title=xyz
		/// </summary>
		public virtual string GetNewPageUrlForTitle(string title)
		{
			return _urlHelper.Action("New", "Pages", new { title = title });
		}
	}
}