using System;
using System.Linq;
using Bog.Api.Domain.Models.Http;
using Bog.Api.Domain.Values;

namespace Bog.Cmd.Domain.Extensions
{
    public static class ArticleResponseExtensions
    {
        public static string GetSelfApiLink(this ArticleResponse articleResponse)
        {
            if (articleResponse == null) throw new ArgumentNullException(nameof(articleResponse));
            return GetApiLink(articleResponse, LinkRelValueObject.SELF);
        }

        public static string GetApiLink(this ArticleResponse articleResponse, string relLinkToFind)
        {
            if (articleResponse == null) throw new ArgumentNullException(nameof(articleResponse));

            return FindLink(articleResponse.Links, relLinkToFind);
        }

        public static string GetApiLink(this ArticleEntryResponse articleEntryResponse, string relLinkToFind)
        {
            if (articleEntryResponse == null) throw new ArgumentNullException(nameof(articleEntryResponse));

            return FindLink(articleEntryResponse.Links, relLinkToFind);
        }

        private static string FindLink(Link[] links, string relLinkToFind)
        {
            var articleRef = links.FirstOrDefault(link => link.Relation == relLinkToFind);

            if (articleRef == null || string.IsNullOrWhiteSpace(articleRef.Href))
            {
                throw new Exception($"ArticleResponse is missing {relLinkToFind} link");
            }

            return articleRef.Href;
        }
    }
}