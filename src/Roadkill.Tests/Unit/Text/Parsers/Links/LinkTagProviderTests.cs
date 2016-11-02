using System;
using NUnit.Framework;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Text.Parsers.Links;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text.Parsers.Links
{
	public class LinkTagProviderTests
	{
		private PageRepositoryMock _pageRepository;
		private ApplicationSettings _applicationSettings;
		private LinkTagProvider _provider;

		[SetUp]
		public void Setup()
		{
			var container = new MocksAndStubsContainer();
			_pageRepository = container.PageRepository;
			_applicationSettings = container.ApplicationSettings;

			_provider = new LinkTagProvider(_pageRepository, _applicationSettings);
		}

		[Test]
		public void href_with_dashes_and_23_are_not_encoded()
		{
			// Arrange
			HtmlLinkTag linkTag = new HtmlLinkTag("https://www.google.com/some-page-23", "https://www.google.com/some-page-23", "text", "");

			// Act
			HtmlLinkTag actualTag = _provider.Parse(linkTag);

			// Assert
			Assert.That(actualTag.Href, Is.EqualTo("https://www.google.com/some-page-23"));
		}

		[Test]
		public void anchor_href_should_not_have_external_css_class()
		{
			// Arrange
			HtmlLinkTag linkTag = new HtmlLinkTag("#myanchortag", "#myanchortag", "text", "");

			// Act
			HtmlLinkTag actualTag = _provider.Parse(linkTag);

			// Assert
			Assert.That(actualTag.Href, Is.EqualTo("#myanchortag"));
			Assert.That(actualTag.CssClass, Is.EqualTo(""));
		}

		[Test]
		public void href_with_tilde_should_resolve_as_attachment_paths()
		{
			// Arrange
			HtmlLinkTag linkTag = new HtmlLinkTag("~/my/folder/image1.jpg", "~/my/folder/image1.jpg", "text", "");

			// Act
			HtmlLinkTag actualTag = _provider.Parse(linkTag);

			// Assert
			Assert.That(actualTag.Href, Is.EqualTo("/Attachments/my/folder/image1.jpg"));
		}

		[Test]
		public void href_external_link_with_anchor_should_return_anchor()
		{
			// Arrange - Issue #172 (Bitbucket)
			HtmlLinkTag linkTag = new HtmlLinkTag("http://www.google.com/?blah=xyz#myanchor", "http://www.google.com/?blah=xyz#myanchor", "text", "");

			// Act
			HtmlLinkTag actualTag = _provider.Parse(linkTag);

			// Assert
			Assert.That(actualTag.Href, Is.EqualTo("http://www.google.com/?blah=xyz#myanchor"));
		}

		[Test]
		public void href_internal_existing_wiki_page_link_should_use_wiki_prefix()
		{
			// Arrange
			_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo page" }, "foo", "admin", DateTime.Today);
			HtmlLinkTag linkTag = new HtmlLinkTag("foo-page", "foo-page", "text", "");

			// Act
			HtmlLinkTag actualTag = _provider.Parse(linkTag);

			// Assert
			Assert.That(actualTag.Href, Is.EqualTo("/wiki/1/foo-page"));
		}

		[Test, Ignore]
		public void attachment_link_should_not_have_nofollow_attribute()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/Attachments/folder/myfile.jpg\">Some link text</a> <a href=\"/Attachments/folder2/myfile.jpg\">Some link text</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[Some link text](~/folder/myfile.jpg) [Some link text](attachment:/folder2/myfile.jpg)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void specialurl_link_should_not_have_nofollow_attribute()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/wiki/Special:Random\">Some link text</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[Some link text](Special:Random)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void internal_links_with_anchor_tag_should_retain_the_anchor()
		{
			//// Issue #172
			//// Arrange
			//_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/wiki/1/foo#myanchor\">Some link text</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[Some link text](foo#myanchor)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void internal_links_with_urlencoded_anchor_tag_should_retain_the_anchor()
		{
			//// Issue #172
			//// Arrange
			//_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/wiki/1/foo%23myanchor\">Some link text</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[Some link text](foo%23myanchor)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void internal_links_with_anchor_tag_should_retain_the_anchor_with_markdown()
		{
			//// Issue #172
			//// Arrange
			//_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/wiki/1/foo#myanchor\">Some link text</a></p>\n"; // use /index/ as no routing exists

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[Some link text](foo#myanchor)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void links_with_the_word_script_in_url_should_not_be_cleaned()
		{
			//// Issue #159
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"http://msdn.microsoft.com/en-us/library/system.componentmodel.descriptionattribute.aspx\" class=\"external-link\" rel=\"nofollow\">ComponentModel.Description</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[ComponentModel.Description](http://msdn.microsoft.com/en-us/library/system.componentmodel.descriptionattribute.aspx)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void links_with_angle_brackets_and_quotes_should_be_encoded()
		{
			//// Issue #159
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"http://www.google.com/%22%3Ejavascript:alert(%27hello%27)\" class=\"external-link\" rel=\"nofollow\">ComponentModel</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[ComponentModel](http://www.google.com/\">javascript:alert('hello'))");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void links_starting_with_attachmentcolon_should_resolve_as_attachment_paths()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/Attachments/my/folder/image1.jpg\">hello world</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[hello world](attachment:/my/folder/image1.jpg)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void links_starting_with_specialcolon_should_resolve_as_full_specialpage()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"/wiki/Special:Foo\">My special page</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[My special page](Special:Foo)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

		[Test, Ignore]
		public void links_starting_with_http_www_mailto_tag_are_no_rewritten_as_internal()
		{
			//// Arrange
			//_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

			//string expectedHtml = "<p><a href=\"http://www.blah.com/\" class=\"external-link\" rel=\"nofollow\">link1</a> " +
			//	"<a href=\"www.blah.com\" class=\"external-link\">link2</a> " +
			//	"<a href=\"mailto:spam@gmail.com\" class=\"external-link\" rel=\"nofollow\">spam</a></p>\n";

			//// Act
			//string actualHtml = _markupConverter.ToHtml("[link1](http://www.blah.com) [link2](www.blah.com) [](mailto:spam@gmail.com)");

			//// Assert
			//Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		}

	}
}
