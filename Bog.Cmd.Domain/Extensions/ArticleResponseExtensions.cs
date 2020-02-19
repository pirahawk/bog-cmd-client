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

            var articleRef = articleResponse.Links.FirstOrDefault(link => link.Relation == relLinkToFind);

            if (articleRef == null || string.IsNullOrWhiteSpace(articleRef.Href))
            {
                throw new Exception($"ArticleResponse is missing {relLinkToFind} link");
            }

            return articleRef.Href;
        }
    }
}