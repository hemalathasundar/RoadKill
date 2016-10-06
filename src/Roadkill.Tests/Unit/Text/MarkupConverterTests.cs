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
    public class MarkupParserMock : IMarkupParser
    {
        public Func<HtmlImageTag, HtmlImageTag> ImageParsed { get; set; }
        public Func<HtmlLinkTag, HtmlLinkTag> LinkParsed { get; set; }

        public string ToHtml(string markdown)
        {
            return markdown;
        }
    }

    [TestFixture]
    [Category("Unit")]
    public class MarkupConverterTests
    {
        private MocksAndStubsContainer _container;
        private ApplicationSettings _applicationSettings;
        private PageRepositoryMock _pageRepository;
        private PluginFactoryMock _pluginFactory;
        private MarkupConverter _markupConverter;

		// PluginRunner.BeforeParse
		// MarkupParser.ToHtml
		// RemoveHarmfulTags
		// Tokenparser.ReplaceTokensAfterParse
		// PageHtml.PreContainerHtml
		// PageHtml.PostContainerHtml
		// IsCacheable

		// TODO:
		// ContainsPageLink - 
		// ReplacePageLinks - Refactor into seperate class

		// TOCParser
		// Creole tests

		[SetUp]
        public void Setup()
        {
            _container = new MocksAndStubsContainer();

            _applicationSettings = _container.ApplicationSettings;
            _applicationSettings.UseHtmlWhiteList = true;
            _applicationSettings.CustomTokensPath = Path.Combine(TestConstants.WEB_PATH, "App_Data", "customvariables.xml");

            _pageRepository = _container.PageRepository;

            _pluginFactory = _container.PluginFactory;
            _markupConverter = _container.MarkupConverter;
        }

        [Test]
        public void html_should_not_be_sanitized_if_usehtmlwhitelist_setting_is_false()
        {
            // Arrange
			string htmlFragment = "<div onclick=\"javascript:alert('ouch');\">test</div>";

			_applicationSettings.UseHtmlWhiteList = false;
			_markupConverter = _container.MarkupConverterFactory.CreateConverter();

            // Act
            string actualHtml = _markupConverter.ToHtml(htmlFragment);

            // Assert
            string expectedHtml = htmlFragment + "\n";
            Assert.That(actualHtml, Is.EqualTo(expectedHtml));
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
    }
}