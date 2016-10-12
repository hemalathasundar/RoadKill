namespace Roadkill.Core.Text.TextMiddleware
{
    public class TextPluginMiddleware : Middleware
    {
        private readonly TextPluginRunner _textPluginRunner;

        public TextPluginMiddleware(TextPluginRunner textPluginRunner)
        {
            _textPluginRunner = textPluginRunner;
        }

        public override PageHtml Invoke(PageHtml pageHtml)
        {
            pageHtml.Html = _textPluginRunner.AfterParse(pageHtml.Html);
            pageHtml.PreContainerHtml = _textPluginRunner.PreContainerHtml();
            pageHtml.PostContainerHtml = _textPluginRunner.PostContainerHtml();
            pageHtml.IsCacheable = _textPluginRunner.IsCacheable;

            return pageHtml;
        }
    }
}