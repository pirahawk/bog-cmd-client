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

            var articleRef = articleResponse.Links.FirstOrDefault(link => link.Relation == LinkRelValueObject.SELF);

            if (articleRef == null || string.IsNullOrWhiteSpace(articleRef.Href))
            {
                throw new Exception("ArticleResponse is missing SELF link");
            }

            return articleRef.Href;
        }
    }
}