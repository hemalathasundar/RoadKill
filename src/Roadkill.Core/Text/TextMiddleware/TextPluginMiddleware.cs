using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Text.TextMiddleware
{
    public class TextPluginBeforeParseMiddleware : Middleware
    {
        private readonly TextPluginRunner _textpluginRunner;

        public TextPluginBeforeParseMiddleware(TextPluginRunner textpluginRunner)
        {
            _textpluginRunner = textpluginRunner;
        }

        public override PageHtml Invoke(PageHtml pageHtml)
        {
            string text = _textpluginRunner.BeforeParse(pageHtml.Html, pageHtml);
            pageHtml.Html = text;

            return pageHtml;
        }
    }

    public class MarkupParserMiddleware
    {
        
    }

    public class HarmfulTagMiddleware
    {
        
    }

    public class CustomTokenMiddleware
    {
        
    }

    public class TextPluginAfterParseMiddleware
    {
    }
}
