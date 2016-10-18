using Ganss.XSS;
using Roadkill.Core.Configuration;
using Roadkill.Core.Text;
using Roadkill.Core.Text.TextMiddleware;

namespace Roadkill.Core.Converters
{
	public class MarkupConverter
	{
        private readonly ApplicationSettings _applicationSettings;
	    private readonly HtmlSanitizer _htmlSanitizer;
	    private readonly TextPluginRunner _textPluginRunner;
	    private readonly CustomTokenParser _customTokenParser;

		public IMarkupParser MarkupParser { get; }

        /// <summary>
        /// HtmlSanitizer can be null, if the UseHtmlWhiteList setting is off.
        /// </summary>
		public MarkupConverter(ApplicationSettings applicationSettings, IMarkupParser markupParser, 
            HtmlSanitizer htmlSanitizer, TextPluginRunner textPluginRunner, CustomTokenParser customTokenParser)
		{
			MarkupParser = markupParser;

		    _applicationSettings = applicationSettings;
		    _htmlSanitizer = htmlSanitizer;
		    _textPluginRunner = textPluginRunner;
		    _customTokenParser = customTokenParser;
		}

		public PageHtml ToHtml(string text)
		{
		    var builder = new TextMiddlewareBuilder();
		    //builder.Use(new TextPluginRunner());
            //builder.Use(new MarkupParserMiddleware())

            PageHtml pageHtml = new PageHtml();

            // Text plugins before parse
            text = _textPluginRunner.BeforeParse(text, pageHtml);			

			// Parse the markup into HTML
			string html = MarkupParser.ToHtml(text);
			
			// Remove bad HTML tags
			html = RemoveHarmfulTags(html);

			// Customvariables.xml file
			html = _customTokenParser.ReplaceTokensAfterParse(html);

			// Text plugins after parse
			html = _textPluginRunner.AfterParse(html);

			// Text plugins pre and post #container HTML
			pageHtml.PreContainerHtml = _textPluginRunner.PreContainerHtml();
			pageHtml.PostContainerHtml = _textPluginRunner.PostContainerHtml();
			
			pageHtml.IsCacheable = _textPluginRunner.IsCacheable;
			pageHtml.Html = html;

			return pageHtml;
		}		

		private string RemoveHarmfulTags(string html)
		{
		    if (_applicationSettings.UseHtmlWhiteList)
			{
				return _htmlSanitizer.Sanitize(html);
			}

            return html;
		}
	}
}
