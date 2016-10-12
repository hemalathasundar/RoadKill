using System;
using System.Collections.Generic;

namespace Roadkill.Core.Text
{
    public class TextMiddlewareBuilder
    {
        private readonly string _markdown;
        private readonly List<Middleware> _middleItems;

        public TextMiddlewareBuilder(string markdown)
        {
            _markdown = markdown;
            _middleItems = new List<Middleware>();
        }

        public void Use(Middleware middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            _middleItems.Add(middleware);
        }

        public PageHtml Execute()
        {
            var pageHtml = new PageHtml() {Html = _markdown};

            foreach (Middleware item in _middleItems)
            {
                try
                {
                    pageHtml = item.Invoke(pageHtml);
                }
                catch (Exception)
                {
                    // TODO: logging
                }
            }

            return pageHtml;
        }
    }
}
