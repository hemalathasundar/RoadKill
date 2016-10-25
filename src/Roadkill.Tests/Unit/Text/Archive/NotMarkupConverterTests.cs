namespace Roadkill.Tests.Unit.Text.Archive
{
    class NotMarkupConverterTests
    {
		//[Test]
		//public void imageparsed_should_convert_to_absolute_path()
		//{
		//	// Arrange
		//	UrlResolverMock resolver = new UrlResolverMock();
		//	resolver.AbsolutePathSuffix = "123";
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);
		//	_markupConverter.UrlResolver = resolver;

		//	// Act
		//	bool wasCalled = false;
		//	_markupConverter.MarkupParser.ImageParsed += (object sender, HtmlImageTag e) =>
		//	{
		//		wasCalled = (e.Src == "/Attachments/DSC001.jpg123");
		//	};

		//	_markupConverter.ToHtml("![Image title](/DSC001.jpg)");

		//	// Assert
		//	Assert.True(wasCalled, "ImageParsed.ImageEventArgs.Src did not match.");
		//}

		//[Test]
		//public void links_with_dashes_or_23_are_rewritten_and_not_parsed_as_encoded_hashes()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"#myanchortag\">hello world</a> <a href=\"https://www.google.com/some-page-23\" class=\"external-link\" rel=\"nofollow\">google</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[hello world](#myanchortag) [google](https://www.google.com/some-page-23)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		//}

		//[Test]
		//public void links_to_named_anchors_should_not_have_external_css_class()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"#myanchortag\">hello world</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[hello world](#myanchortag)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		//}

		//[Test]
		//public void links_starting_with_tilde_should_resolve_as_attachment_paths()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/Attachments/my/folder/image1.jpg\">hello world</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[hello world](~/my/folder/image1.jpg)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void external_links_with_anchor_tag_should_retain_the_anchor()
		//{
		//	// Issue #172
		//	// Arrange
		//	_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"http://www.google.com/?blah=xyz#myanchor\" class=\"external-link\" rel=\"nofollow\">Some link text</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](http://www.google.com/?blah=xyz#myanchor)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void internal_wiki_page_link_should_not_have_nofollow_attribute()
		//{
		//	// Arrange
		//	_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo-page" }, "foo", "admin", DateTime.Today);
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/wiki/1/foo-page\">Some link text</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](foo-page)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void attachment_link_should_not_have_nofollow_attribute()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/Attachments/folder/myfile.jpg\">Some link text</a> <a href=\"/Attachments/folder2/myfile.jpg\">Some link text</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](~/folder/myfile.jpg) [Some link text](attachment:/folder2/myfile.jpg)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void specialurl_link_should_not_have_nofollow_attribute()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/wiki/Special:Random\">Some link text</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](Special:Random)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void internal_links_with_anchor_tag_should_retain_the_anchor()
		//{
		//	// Issue #172
		//	// Arrange
		//	_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/wiki/1/foo#myanchor\">Some link text</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](foo#myanchor)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void internal_links_with_urlencoded_anchor_tag_should_retain_the_anchor()
		//{
		//	// Issue #172
		//	// Arrange
		//	_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/wiki/1/foo%23myanchor\">Some link text</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](foo%23myanchor)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void internal_links_with_anchor_tag_should_retain_the_anchor_with_markdown()
		//{
		//	// Issue #172
		//	// Arrange
		//	_pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/wiki/1/foo#myanchor\">Some link text</a></p>\n"; // use /index/ as no routing exists

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[Some link text](foo#myanchor)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void links_with_the_word_script_in_url_should_not_be_cleaned()
		//{
		//	// Issue #159
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"http://msdn.microsoft.com/en-us/library/system.componentmodel.descriptionattribute.aspx\" class=\"external-link\" rel=\"nofollow\">ComponentModel.Description</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[ComponentModel.Description](http://msdn.microsoft.com/en-us/library/system.componentmodel.descriptionattribute.aspx)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void links_with_angle_brackets_and_quotes_should_be_encoded()
		//{
		//	// Issue #159
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"http://www.google.com/%22%3Ejavascript:alert(%27hello%27)\" class=\"external-link\" rel=\"nofollow\">ComponentModel</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[ComponentModel](http://www.google.com/\">javascript:alert('hello'))");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void links_starting_with_attachmentcolon_should_resolve_as_attachment_paths()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/Attachments/my/folder/image1.jpg\">hello world</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[hello world](attachment:/my/folder/image1.jpg)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void links_starting_with_specialcolon_should_resolve_as_full_specialpage()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"/wiki/Special:Foo\">My special page</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[My special page](Special:Foo)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void links_starting_with_http_www_mailto_tag_are_no_rewritten_as_internal()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><a href=\"http://www.blah.com/\" class=\"external-link\" rel=\"nofollow\">link1</a> " +
		//		"<a href=\"www.blah.com\" class=\"external-link\">link2</a> " +
		//		"<a href=\"mailto:spam@gmail.com\" class=\"external-link\" rel=\"nofollow\">spam</a></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("[link1](http://www.blah.com) [link2](www.blah.com) [](mailto:spam@gmail.com)");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
		//}

		//[Test]
		//public void should_allow_style_tags()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);

		//	string expectedHtml = "<p><b style=\"color: black\"></b></p>\n";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml("<b style='color:black'><script>alert('foo')</script></b>");

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expectedHtml));
		//}

		//[Test]
		//public void should_not_render_toc_with_multiple_curlies()
		//{
		//	// Arrange
		//	_markupConverter = new MarkupConverter(_applicationSettings, _pageRepository, _pluginFactory);
		//	_markupConverter.UrlResolver = new UrlResolverMock();

		//	string htmlFragment = "Give me a {{TOC}} and a {{{TOC}}} - the should not render a TOC";
		//	string expected = @"<p>Give me a </p><div class=""floatnone""><div class=""image_frame""><img src=""/Attachments/TOC""></div></div> and a TOC - the should not render a TOC"
		//		+ "\n<p></p>";

		//	// Act
		//	string actualHtml = _markupConverter.ToHtml(htmlFragment);

		//	// Assert
		//	Assert.That(actualHtml, Is.EqualTo(expected), actualHtml);
		//}
	}
}
