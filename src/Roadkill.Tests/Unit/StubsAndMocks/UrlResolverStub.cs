using System.Web.Mvc;
using Roadkill.Core.Text.Parsers.Links;

namespace Roadkill.Tests.Unit.StubsAndMocks
{
	public class UrlResolverMock : UrlResolver
	{
		public string AbsolutePathSuffix { get; set; }
		public string InternalUrl { get; set; }
		public string NewPageUrl { get; set; }

		public UrlResolverMock() : base(new UrlHelper())
		{
		}

		public UrlResolverMock(UrlHelper urlHelper) : base(urlHelper)
		{
		}

		public override string ConvertToAbsolutePath(string relativeUrl)
		{
			if (!string.IsNullOrEmpty(AbsolutePathSuffix))
				return relativeUrl + AbsolutePathSuffix;
			else
				return relativeUrl;
		}

		public override string GetInternalUrlForTitle(int id, string title)
		{
			if (!string.IsNullOrEmpty(InternalUrl))
				return InternalUrl;
			else
				return title;
		}

		public override string GetNewPageUrlForTitle(string title)
		{
			if (!string.IsNullOrEmpty(NewPageUrl))
				return NewPageUrl;
			else
				return title;
		}
	}
}
