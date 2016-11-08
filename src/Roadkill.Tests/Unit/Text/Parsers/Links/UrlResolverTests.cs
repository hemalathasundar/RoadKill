using System;
using NUnit.Framework;
using Roadkill.Core.Text.Parsers.Links;

namespace Roadkill.Tests.Unit.Text.Parsers.Links
{
	public class UrlResolverTests
	{
		[Test]
		public void ConvertToAbsolutePath_should_resolve_with_HttpContext()
		{
			// Arrange
			var urlHelper = new UrlHelperMock();
			var resolver = new UrlResolver(urlHelper);

			// Act
			string actualPath = resolver.ConvertToAbsolutePath("~/mydir/page1.html");

			// Assert
			Assert.That(actualPath, Is.EqualTo("~/mydir/page1.html"));
		}

		[Test]
		public void GetInternalUrlForTitle_should_create_action_with_HttpContext()
		{
			// Arrange
			string expectedAction = "Index/Wiki/1/title";
			var urlHelper = new UrlHelperMock() {ExpectedAction = expectedAction};
			var resolver = new UrlResolver(urlHelper);

			// Act
			string actualAction = resolver.GetInternalUrlForTitle(1, "title");

			// Assert
			Assert.That(actualAction, Is.EqualTo(expectedAction));
		}

		[Test]
		public void GetNewPageUrlForTitle_should_create_action_with_HttpContext()
		{
			// Arrange
			string expectedAction = "Index/Wiki/1/title";
			var urlHelper = new UrlHelperMock() { ExpectedAction = expectedAction };
			var resolver = new UrlResolver(urlHelper);

			// Act
			string actualAction = resolver.GetNewPageUrlForTitle("title");

			// Assert
			Assert.That(actualAction, Is.EqualTo(expectedAction));
		}
	}
}