using System;
using System.Collections.Generic;
using System.Linq;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Text.Parsers.Markdig
{
    public class MarkdigImagesAndLinkWalker
    {
        private readonly Action<HtmlImageTag> _imageDelegate;
        private readonly Action<HtmlLinkTag> _linkDelegate;

        public MarkdigImagesAndLinkWalker(Action<HtmlImageTag> imageDelegate, Action<HtmlLinkTag> linkDelegate)
        {
            _imageDelegate = imageDelegate;
            _linkDelegate = linkDelegate;
        }

        public void WalkAndBindParseEvents(MarkdownObject markdownObject)
        {
            foreach (MarkdownObject child in markdownObject.Descendants())
            {
                // LinkInline can be both an <img.. or a <a href="...">
                LinkInline link = child as LinkInline;
                if (link != null)
                {
                    EnsureAttributesInLink(link);

                    if (link.IsImage)
                    {
                        string altText = "";

                        var descendentForAltTag = child.Descendants().FirstOrDefault();
                        if (descendentForAltTag != null)
                            altText = descendentForAltTag.ToString();

                        if (_imageDelegate != null)
                        {
                            HtmlImageTag args = InvokeImageParsedEvent(link.Url, altText);

                            // Update the HTML from the data the event gives back
                            link.Url = args.Src;
                            link.Title = altText;
                        }

                        // Replace to alt= attribute, it's a literal
                        var literalInline = new LiteralInline(altText);
                        link.FirstChild.ReplaceBy(literalInline);

                        // Necessary for links and Bootstrap 3
                        AddAttribute(link, "border", "0");

                        // Make all images expand via this Bootstrap class 
                        AddClass(link, "img-responsive"); 
                    }
                    else
                    {
                        if (_linkDelegate != null)
                        {
                            HtmlLinkTag args = InvokeLinkParsedEvent(link.Url, link.Title, link.Label);

                            // Update the HTML from the data the event gives back
                            link.Url = args.Href;

                            if (!string.IsNullOrEmpty(args.Target))
                            {
                                AddAttribute(link, "target", args.Target);
                            }


                            if (!string.IsNullOrEmpty(args.CssClass))
                                AddClass(link, args.CssClass);

                            // Replace the link's text
                            var literalInline = new LiteralInline(args.Text);
                            link.FirstChild.ReplaceBy(literalInline);
                        }

                        // Markdig TODO: make these configurable (external-links: [])
                        if (!string.IsNullOrEmpty(link.Url) && 
                            (link.Url.ToLower().StartsWith("http://") || 
                            link.Url.ToLower().StartsWith("https://") ||
                            link.Url.ToLower().StartsWith("mailto:"))
                            )
                        {
                            AddAttribute(link, "rel", "nofollow");
                        }
                    }
                }

                WalkAndBindParseEvents(child);
            }
        }

        private void EnsureAttributesInLink(LinkInline link)
        {
            HtmlAttributes attributes = link.GetAttributes();
            if (attributes == null)
            {
                attributes = new HtmlAttributes();
                attributes.Classes = new List<string>();
            }

            if (attributes.Properties == null)
            {
                attributes.Properties = new List<KeyValuePair<string, string>>();
            }

            if (attributes.Classes == null)
            {
                attributes.Classes = new List<string>();
            }
        }

        private void AddAttribute(LinkInline link, string name, string value)
        {
            HtmlAttributes attributes = link.GetAttributes();

            if (!attributes.Properties.Any(x => x.Key == name))
            {
                attributes.AddPropertyIfNotExist(name, value);
                link.SetAttributes(attributes);
            }
        }

        private void AddClass(LinkInline link, string cssClass)
        {
            HtmlAttributes attributes = link.GetAttributes();

            if (!attributes.Classes.Any(x => x == cssClass))
            {
                attributes.Classes.Add(cssClass);
                link.SetAttributes(attributes);
            }
        }

        private HtmlImageTag InvokeImageParsedEvent(string url, string altText)
        {
            // Markdig TODO
            //string linkID = altText.ToLowerInvariant();

            HtmlImageTag args = new HtmlImageTag(url, url, altText, "");
            _imageDelegate(args);

            return args;
        }

        private HtmlLinkTag InvokeLinkParsedEvent(string url, string text, string target)
        {
            HtmlLinkTag args = new HtmlLinkTag(url, url, text, target);
            _linkDelegate(args);

            return args;
        }
    }
}