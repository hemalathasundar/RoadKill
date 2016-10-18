using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ganss.XSS;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;
using Roadkill.Core.Plugins;
using Roadkill.Core.Text;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.Sanitizer;
using Roadkill.Core.Text.TextMiddleware;
using StructureMap;

namespace Roadkill.Core.DependencyResolution.StructureMap
{
    public class TextRegistry : Registry
    {
        public TextRegistry()
        {
            For<IPluginFactory>().Use<PluginFactory>();
            For<IMarkupParser>().Use<MarkdigParser>();
            For<IHtmlSanitizerFactory>().Use<HtmlSanitizerFactory>();

            For<TextMiddlewareBuilder>()
                .AlwaysUnique()
                .Use("TextMiddlewareBuilder", ctx =>
                {
                    var builder = new TextMiddlewareBuilder();

                    var textPluginRunner = ctx.GetInstance<TextPluginRunner>();
                    var markupParser = ctx.GetInstance<IMarkupParser>();
                    var htmlSanitizerFactory = ctx.GetInstance<IHtmlSanitizerFactory>();
                    var customTokenParser = ctx.GetInstance<CustomTokenParser>();

                    builder.Use(new TextPluginBeforeParseMiddleware(textPluginRunner));
                    builder.Use(new MarkupParserMiddleware(markupParser));
                    builder.Use(new HarmfulTagMiddleware(htmlSanitizerFactory));
                    builder.Use(new CustomTokenMiddleware(customTokenParser));
                    builder.Use(new TextPluginAfterParseMiddleware(textPluginRunner));

                    return builder;
                });
        }
    }
}
