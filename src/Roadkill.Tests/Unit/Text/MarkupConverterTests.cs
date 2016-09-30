using System;
using System.IO;
using NUnit.Framework;
using Roadkill.Core.Cache;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;
using Roadkill.Core.Database;
using Roadkill.Tests.Unit.StubsAndMocks;

namespace Roadkill.Tests.Unit.Text
{
    [TestFixture]
    [Category("Unit")]
    public class MarkupConverterTests
    {
        private MocksAndStubsContainer _container;

        private ApplicationSettings _applicationSettings;
        private SettingsRepositoryMock _settingsRepository;
        private PageRepositoryMock _pageRepository;
        private PluginFactoryMock _pluginFactory;
        private MarkupConverter _markupConverter;

        [SetUp]
        public void Setup()
        {
            _container = new MocksAndStubsContainer();

            _applicationSettings = _container.ApplicationSettings;
            _applicationSettings.UseHtmlWhiteList = true;
            _applicationSettings.CustomTokensPath = Path.Combine(TestConstants.WEB_PATH, "App_Data", "customvariables.xml");

            _settingsRepository = _container.SettingsRepository;
            _pageRepository = _container.PageRepository;

            _pluginFactory = _container.PluginFactory;
            _markupConverter = _container.MarkupConverter;
            _markupConverter.UrlResolver = new UrlResolverMock();
        }

        [Test]
        public void parser_should_not_be_null_for_markuptypes()
        {
            // Arrange, act

            // Assert
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);
            Assert.NotNull(_markupConverter.Parser);

            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);
            Assert.NotNull(_markupConverter.Parser);
        }

        [Test]
        public void imageparsed_should_convert_to_absolute_path()
        {
            // Arrange
            UrlResolverMock resolver = new UrlResolverMock();
            resolver.AbsolutePathSuffix = "123";
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);
            _markupConverter.UrlResolver = resolver;

            // Act
            bool wasCalled = false;
            _markupConverter.Parser.ImageParsed += (object sender, ImageEventArgs e) =>
            {
                wasCalled = (e.Src == "/Attachments/DSC001.jpg123");
            };

            _markupConverter.ToHtml("![Image title](/DSC001.jpg)");

            // Assert
            Assert.True(wasCalled, "ImageParsed.ImageEventArgs.Src did not match.");
        }

        [Test]
        [TestCase("http://i223.photobucket.com/albums/dd45/wally2603/91e7840f.jpg")]
        [TestCase("https://i223.photobucket.com/albums/dd45/wally2603/91e7840f.jpg")]
        [TestCase("www.photobucket.com/albums/dd45/wally2603/91e7840f.jpg")]
        public void ImageParsed_Should_Not_Rewrite_Images_As_Internal_That_Start_With_Known_Prefixes(string imageUrl)
        {
            // Arrange
            UrlResolverMock resolver = new UrlResolverMock();
            resolver.AbsolutePathSuffix = "123";

            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);
            _markupConverter.UrlResolver = resolver;

            bool wasCalled = false;
            _markupConverter.Parser.ImageParsed += (object sender, ImageEventArgs e) =>
            {
                wasCalled = (e.Src == imageUrl);
            };

            // Act
            _markupConverter.ToHtml("![Image title](" + imageUrl + ")");

            // Assert
            Assert.True(wasCalled);
        }

        [Test]
        public void should_remove_script_link_iframe_frameset_frame_applet_tags_from_text()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);
            string markdown = " some text <script type=\"text/html\">while(true)alert('lolz');</script>" +
                "<iframe src=\"google.com\"></iframe><frame>blah</frame> <applet code=\"MyApplet.class\" width=100 height=140></applet>" +
                "<frameset src='new.html'></frameset>";

            string expectedHtml = "<p>some text blah </p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml(markdown);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void links_starting_with_hash_or_https_or_hash_are_not_rewritten_as_internal()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"#myanchortag\">hello world</a> <a href=\"https://www.google.com/\" class=\"external-link\" rel=\"nofollow\">google</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[hello world](#myanchortag) [google](https://www.google.com)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void links_with_dashes_or_23_are_rewritten_and_not_parsed_as_encoded_hashes()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"#myanchortag\">hello world</a> <a href=\"https://www.google.com/some-page-23\" class=\"external-link\" rel=\"nofollow\">google</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[hello world](#myanchortag) [google](https://www.google.com/some-page-23)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void links_to_named_anchors_should_not_have_external_css_class()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"#myanchortag\">hello world</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[hello world](#myanchortag)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void links_starting_with_tilde_should_resolve_as_attachment_paths()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/Attachments/my/folder/image1.jpg\">hello world</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[hello world](~/my/folder/image1.jpg)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void external_links_with_anchor_tag_should_retain_the_anchor()
        {
            // Issue #172
            // Arrange
            _pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"http://www.google.com/?blah=xyz#myanchor\" class=\"external-link\" rel=\"nofollow\">Some link text</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](http://www.google.com/?blah=xyz#myanchor)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void internal_wiki_page_link_should_not_have_nofollow_attribute()
        {
            // Arrange
            _pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo-page" }, "foo", "admin", DateTime.Today);
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/wiki/1/foo-page\">Some link text</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](foo-page)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void attachment_link_should_not_have_nofollow_attribute()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/Attachments/folder/myfile.jpg\">Some link text</a> <a href=\"/Attachments/folder2/myfile.jpg\">Some link text</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](~/folder/myfile.jpg) [Some link text](attachment:/folder2/myfile.jpg)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void specialurl_link_should_not_have_nofollow_attribute()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/wiki/Special:Random\">Some link text</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](Special:Random)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void internal_links_with_anchor_tag_should_retain_the_anchor()
        {
            // Issue #172
            // Arrange
            _pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/wiki/1/foo#myanchor\">Some link text</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](foo#myanchor)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void internal_links_with_urlencoded_anchor_tag_should_retain_the_anchor()
        {
            // Issue #172
            // Arrange
            _pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/wiki/1/foo%23myanchor\">Some link text</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](foo%23myanchor)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void internal_links_with_anchor_tag_should_retain_the_anchor_with_markdown()
        {
            // Issue #172
            // Arrange
            _pageRepository.AddNewPage(new Page() { Id = 1, Title = "foo" }, "foo", "admin", DateTime.Today);
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/wiki/1/foo#myanchor\">Some link text</a></p>\n"; // use /index/ as no routing exists

            // Act
            string actualHtml = _markupConverter.ToHtml("[Some link text](foo#myanchor)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void links_with_the_word_script_in_url_should_not_be_cleaned()
        {
            // Issue #159
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"http://msdn.microsoft.com/en-us/library/system.componentmodel.descriptionattribute.aspx\" class=\"external-link\" rel=\"nofollow\">ComponentModel.Description</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[ComponentModel.Description](http://msdn.microsoft.com/en-us/library/system.componentmodel.descriptionattribute.aspx)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void links_with_angle_brackets_and_quotes_should_be_encoded()
        {
            // Issue #159
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"http://www.google.com/%22%3Ejavascript:alert(%27hello%27)\" class=\"external-link\" rel=\"nofollow\">ComponentModel</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[ComponentModel](http://www.google.com/\">javascript:alert('hello'))");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }


        [Test]
        public void links_starting_with_attachmentcolon_should_resolve_as_attachment_paths()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/Attachments/my/folder/image1.jpg\">hello world</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[hello world](attachment:/my/folder/image1.jpg)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void links_starting_with_specialcolon_should_resolve_as_full_specialpage()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"/wiki/Special:Foo\">My special page</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[My special page](Special:Foo)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void links_starting_with_http_www_mailto_tag_are_no_rewritten_as_internal()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><a href=\"http://www.blah.com/\" class=\"external-link\" rel=\"nofollow\">link1</a> "+
                "<a href=\"www.blah.com\" class=\"external-link\">link2</a> " +
                "<a href=\"mailto:spam@gmail.com\" class=\"external-link\" rel=\"nofollow\">spam</a></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("[link1](http://www.blah.com) [link2](www.blah.com) [](mailto:spam@gmail.com)");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void html_should_not_be_sanitized_if_usehtmlwhitelist_setting_is_false()
        {
            // Arrange
            _applicationSettings.UseHtmlWhiteList = false;
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string htmlFragment = "<div onclick=\"javascript:alert('ouch');\">test</div>";
            MarkupConverter converter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            // Act
            string actualHtml = converter.ToHtml(htmlFragment);

            // Assert
            string expectedHtml = htmlFragment + "\n";
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void should_not_render_toc_with_multiple_curlies()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);
            _markupConverter.UrlResolver = new UrlResolverMock();

            string htmlFragment = "Give me a {{TOC}} and a {{{TOC}}} - the should not render a TOC";
            string expected = @"<p>Give me a </p><div class=""floatnone""><div class=""image_frame""><img src=""/Attachments/TOC""></div></div> and a TOC - the should not render a TOC"
                + "\n<p></p>";

            // Act
            string actualHtml = _markupConverter.ToHtml(htmlFragment);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expected), actualHtml);
        }

        [Test]
        [Ignore("TODO: Fix this")]
        public void warningbox_token_with_nowiki_adds_pre_and_renders_token_html()
        {
            // Arrange..make sure expectedHtml uses \n and not \r\n
            string expectedHtml = @"<p><div class=""alert alert-warning"">ENTER YOUR CONTENT HERE 
<pre>here is my C#code
</pre>
</p>
<p></div><br style=""clear:both""/>
</p>";

            expectedHtml = expectedHtml.Replace("\r\n", "\n"); // fix line ending issues

            // Act
            ;
            string actualHtml = _markupConverter.ToHtml(@"@@warningbox:ENTER YOUR CONTENT HERE 

        here is my C#code
 

@@");
            Console.WriteLine(actualHtml);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml), actualHtml);
        }

        [Test]
        public void should_ignore_textplugins_beforeparse_when_isenabled_is_false()
        {
            // Arrange
            string markupFragment = "This is my ~~~usertoken~~~";
            string expectedHtml = "<p>This is my <span>usertoken</span></p>\n";

            TextPluginStub plugin = new TextPluginStub();
            plugin.Repository = new SettingsRepositoryMock();
            plugin.PluginCache = new SiteCache(CacheMock.RoadkillCache);
            _pluginFactory.RegisterTextPlugin(plugin);

            // Act
            string actualHtml = _markupConverter.ToHtml(markupFragment);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void should_ignore_textplugins_afterparse_when_isenabled_is_false()
        {
            // Arrange
            string markupFragment = "Here is some markup **some bold**";
            string expectedHtml = "<p>Here is some markup <strong style='color:green'><iframe src='javascript:alert(test)'>some bold</strong></p>\n";

            TextPluginStub plugin = new TextPluginStub();
            plugin.Repository = new SettingsRepositoryMock();
            plugin.PluginCache = new SiteCache(CacheMock.RoadkillCache);
            _pluginFactory.RegisterTextPlugin(plugin);

            // Act
            string actualHtml = _markupConverter.ToHtml(markupFragment);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void should_fire_beforeparse_in_textplugin()
        {
            // Arrange
            string markupFragment = "This is my ~~~usertoken~~~";
            string expectedHtml = "<p>This is my <span>usertoken</span></p>\n";

            TextPluginStub plugin = new TextPluginStub();
            plugin.Repository = new SettingsRepositoryMock();
            plugin.PluginCache = new SiteCache(CacheMock.RoadkillCache);
            plugin.Settings.IsEnabled = true;
            _pluginFactory.RegisterTextPlugin(plugin);

            // Act
            string actualHtml = _markupConverter.ToHtml(markupFragment);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void should_fire_afterparse_in_textplugin_and_output_should_not_be_cleaned()
        {
            // Arrange
            string markupFragment = "Here is some markup **some bold**";
            string expectedHtml = "<p>Here is some markup <strong style='color:green'><iframe src='javascript:alert(test)'>some bold</strong></p>\n";

            TextPluginStub plugin = new TextPluginStub();
            plugin.Repository = new SettingsRepositoryMock();
            plugin.PluginCache = new SiteCache(CacheMock.RoadkillCache);
            plugin.Settings.IsEnabled = true;
            _pluginFactory.RegisterTextPlugin(plugin);

            // Act
            string actualHtml = _markupConverter.ToHtml(markupFragment);

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void should_allow_style_tags()
        {
            // Arrange
            _markupConverter = new MarkupConverter(_applicationSettings, _settingsRepository, _pageRepository, _pluginFactory);

            string expectedHtml = "<p><b style=\"color: black\"></b></p>\n";

            // Act
            string actualHtml = _markupConverter.ToHtml("<b style='color:black'><script>alert('foo')</script></b>");

            // Assert
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
        }

        // TODO:
        // ContainsPageLink - 
        // ReplacePageLinks - Refactor into seperate class

        // TOCParser
        // Creole tests
    }
}