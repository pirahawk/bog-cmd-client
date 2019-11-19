using System;

namespace Bog.Cmd.Domain.Values
{
    public static class BogApiRouteValues
    {
        public const string PING = "api/ping";
        public const string ARTICLE = "api/article";
        public static Func<Guid, string> ArticleByIdReferenceTemplate = (articleId) => $"{ARTICLE}/{articleId}";
    }
}