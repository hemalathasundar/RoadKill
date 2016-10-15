using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Text.TextMiddleware
{
    public class CustomTokenMiddleware : Middleware
    {
        private readonly CustomTokenParser _customTokenParser;

        public CustomTokenMiddleware(CustomTokenParser customTokenParser)
        {
            _customTokenParser = customTokenParser;
        }

        public override PageHtml Invoke(PageHtml pageHtml)
        {
            pageHtml.Html = _customTokenParser.ReplaceTokensAfterParse(pageHtml.Html);
            return pageHtml;
        }
    }
}
