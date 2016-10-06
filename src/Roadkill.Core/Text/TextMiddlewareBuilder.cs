using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppFunc = System.Func<string, System.Threading.Tasks.Task>;

namespace Roadkill.Core.Text
{
    public class TextMiddlewareBuilder
    {
        private readonly string _markdown;
        private List<Middleware> _middleItems;

        public TextMiddlewareBuilder(string markdown)
        {
            _markdown = markdown;
            _middleItems = new List<Middleware>();
        }

        public void Use(Middleware middleware)
        {
            _middleItems.Add(middleware);
        }

        public string Execute()
        {
            string html = _markdown;
            foreach (Middleware item in _middleItems)
            {
                html = item.Invoke(html);
            }

            return html;
        }
    }

    public abstract class Middleware
    {
        public abstract string Invoke(string markup);
    }
}
