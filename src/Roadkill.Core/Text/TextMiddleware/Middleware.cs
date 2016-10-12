namespace Roadkill.Core.Text
{
    public abstract class Middleware
    {
        public abstract PageHtml Invoke(PageHtml pageHtml);
    }
}