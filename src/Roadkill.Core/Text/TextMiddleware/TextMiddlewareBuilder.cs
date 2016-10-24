using System;
using System.Collections.Generic;
using Roadkill.Core.Text.Menu;

namespace Roadkill.Core.Text.TextMiddleware
{
    public class TextMiddlewareBuilder
    {
        public List<Middleware> MiddleItems { get; set; }

        public TextMiddlewareBuilder()
        {
            MiddleItems = new List<Middleware>();
        }

        public void Use(Middleware middleware)
        {
            if (middleware == null)
                throw new ArgumentNullException(nameof(middleware));

            MiddleItems.Add(middleware);
        }

        public PageHtml Execute(string markdown)
        {
            var pageHtml = new PageHtml() {Html = markdown};

            foreach (Middleware item in MiddleItems)
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
