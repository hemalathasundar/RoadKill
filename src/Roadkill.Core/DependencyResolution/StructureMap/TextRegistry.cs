using Roadkill.Core.Plugins;
using Roadkill.Core.Text;
using Roadkill.Core.Text.CustomTokens;
using Roadkill.Core.Text.Parsers;
using Roadkill.Core.Text.Parsers.Markdig;
using Roadkill.Core.Text.Plugins;
using Roadkill.Core.Text.Sanitizer;
using Roadkill.Core.Text.TextMiddleware;
using StructureMap;
using StructureMap.Graph;

namespace Roadkill.Core.DependencyResolution.StructureMap
{
    public class TextRegistry : Registry
    {
        public TextRegistry()
        {
            Scan(ScanTypes);

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
                }).Singleton();
        }

        private void ScanTypes(IAssemblyScanner scanner)
        {
            scanner.AddAllTypesOf<CustomTokenParser>();
        }
    }
}
