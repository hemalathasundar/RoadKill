using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Web;
using Ganss.XSS;
using Markdig.Parsers;
using Roadkill.Core.Configuration;
using Roadkill.Core.Database.Repositories;
using Roadkill.Core.Text;
using Roadkill.Core.Logging;

namespace Roadkill.Core.Converters
{
	public class MarkupConverter
	{
        private readonly ApplicationSettings _applicationSettings;
	    private readonly HtmlSanitizer _htmlSanitizer;
	    private readonly TextPluginRunner _pluginRunner;
	    private readonly CustomTokenParser _tokenParser;

		public IMarkupParser MarkupParser { get; private set; }

        /// <summary>
        /// HtmlSanitizer can be null, if the UseHtmlWhiteList setting is off.
        /// </summary>
		public MarkupConverter(ApplicationSettings applicationSettings, IMarkupParser markupParser, 
            HtmlSanitizer htmlSanitizer, TextPluginRunner pluginRunner, CustomTokenParser tokenParser)
		{
			if (!_applicationSettings.Installed)
			{
				// Skip the chain of creation, as the markup converter isn't needed but is created by
				// StructureMap - this is required for installation
				return;
			}

			MarkupParser = markupParser;

		    _applicationSettings = applicationSettings;
		    _htmlSanitizer = htmlSanitizer;
		    _pluginRunner = pluginRunner;
		    _tokenParser = tokenParser;
		}

        public string ConvertMenuMarkupToHtml(string menuMarkup)
		{
			return MarkupParser.ToHtml(menuMarkup);
		}

		public PageHtml ToHtml(string text)
		{
            PageHtml pageHtml = new PageHtml();

            // Text plugins before parse
            text = _pluginRunner.BeforeParse(text, pageHtml);			

			// Parse the markup into HTML
			string html = MarkupParser.ToHtml(text);
			
			// Remove bad HTML tags
			html = RemoveHarmfulTags(html);

			// Customvariables.xml file
			html = _tokenParser.ReplaceTokensAfterParse(html);

			// Text plugins after parse
			html = _pluginRunner.AfterParse(html);

			// Text plugins pre and post #container HTML
			pageHtml.PreContainerHtml = _pluginRunner.PreContainerHtml();
			pageHtml.PostContainerHtml = _pluginRunner.PostContainerHtml();
			
			pageHtml.IsCacheable = _pluginRunner.IsCacheable;
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
