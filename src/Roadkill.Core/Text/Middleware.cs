namespace Roadkill.Core.Text
{
    public abstract class Middleware
    {
        public abstract string Invoke(string markup);
    }
}