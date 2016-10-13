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
    }
}