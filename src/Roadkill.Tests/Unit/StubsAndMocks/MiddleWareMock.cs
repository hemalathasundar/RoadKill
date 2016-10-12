using Roadkill.Core.Text;

namespace Roadkill.Tests.Unit.Text
{
    public class MiddleWareMock : Middleware
    {
        public string SearchString { get; set; }
        public string Replacement { get; set; }

        public override string Invoke(string markup)
        {
            return markup.Replace(SearchString, Replacement);
        }
    }
}