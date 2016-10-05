using System.Linq;
using Ganss.XSS;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database;
using Roadkill.Core.Plugins;
using Roadkill.Core.Text;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.Sanitizer;

namespace Roadkill.Core.Converters
{
    public class MarkupConverterFactory : IMarkupConverterFactory
    {
        private readonly ApplicationSettings _applicationSettings;
        private readonly IPageRepository _pageRepository;
        private readonly IPluginFactory _pluginFactory;

        public MarkupConverterFactory(ApplicationSettings applicationSettings, 
            IPageRepository pageRepository, IPluginFactory pluginFactory)
        {
            _applicationSettings = applicationSettings;
            _pageRepository = pageRepository;
            _pluginFactory = pluginFactory;
        }

        public MarkupConverter CreateConverter()
        {
            MarkdigParser markupParser = CreateMarkdigParser();
            HtmlSanitizer htmlSanitizer = CreateHtmlSanitizer();
            CustomTokenParser tokenParser = new CustomTokenParser(_applicationSettings);
            TextPluginRunner runner = new TextPluginRunner(_pluginFactory);

            return new MarkupConverter(_applicationSettings, markupParser, htmlSanitizer, runner, tokenParser);
        }

        private MarkdigParser CreateMarkdigParser()
        {
            var markupParser = new MarkdigParser();
            var linkTagProvider = new LinkTagProvider(_pageRepository);
            var imgTagProvider = new ImageTagProvider(_applicationSettings);

            markupParser.LinkParsed = linkTagProvider.LinkParsed;
            markupParser.ImageParsed += imgTagProvider.ImageParsed;

            return markupParser;
        }

        private HtmlSanitizer CreateHtmlSanitizer()
        {
            if (!_applicationSettings.UseHtmlWhiteList)
                return null;

            HtmlWhiteList htmlWhiteList = HtmlWhiteList.Deserialize(_applicationSettings);
            string[] allowedTags = htmlWhiteList.ElementWhiteList.Select(x => x.Name).ToArray();
            string[] allowedAttributes =
                htmlWhiteList.ElementWhiteList.SelectMany(x => x.AllowedAttributes.Select(y => y.Name)).ToArray();

            if (allowedTags.Length == 0)
                allowedTags = null;

            if (allowedAttributes.Length == 0)
                allowedAttributes = null;

            var htmlSanitizer = new HtmlSanitizer(allowedTags, null, allowedAttributes);
            htmlSanitizer.AllowDataAttributes = false;
            htmlSanitizer.AllowedAttributes.Add("class");
            htmlSanitizer.AllowedAttributes.Add("id");
            htmlSanitizer.AllowedSchemes.Add("mailto");
            htmlSanitizer.RemovingAttribute += (sender, e) =>
            {
                // Don't clean /wiki/Special:Tag urls in href="" attributes
                if (e.Attribute.Name.ToLower() == "href" && e.Attribute.Value.Contains("Special:"))
                {
                    e.Cancel = true;
                }
            };

            return htmlSanitizer;
        }
    }
}