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

            _markupConverter = _container.MarkupConverter;
        }
    }
}