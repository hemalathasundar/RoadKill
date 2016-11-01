using System;
using NUnit.Framework;
using Roadkill.Core.Text.Parsers.Images;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text.Archive
{
	class NotMarkupConverterTests
	{
		[Test]
		public void should_allow_style_tags()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><b style=\"color: black\"></b></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("<b style='color:black'><script>alert('foo')</script></b>");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		}

		[Test]
		public void should_not_render_toc_with_multiple_curlies()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);
			//_markupConverter.UrlResolver = new UrlResolverMock();

			//string htmlFragment = "Give me a {{TOC}} and a {{{TOC}}} - the should not render a TOC";
			//string expected = @"<p>Give me a </p><div class=""floatnone""><div class=""image_frame""><img src=""/Attachments/TOC""></div></div> and a TOC - the should not render a TOC"
			//	+ "\n<p></p>";

			//// Act
			//string actualHtml = _markupConverter.ToHtml(htmlFragment);

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expected), actualHtml);
		}
	}
}
