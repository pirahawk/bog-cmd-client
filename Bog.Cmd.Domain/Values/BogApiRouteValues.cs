using System;

namespace Bog.Cmd.Domain.Values
{
    public static class BogApiRouteValues
    {
        public const string PING = "api/ping";
        public const string POST_ARTICLE = "api/article";
        public static Func<Guid, string> ARTICLE_GUID_FORMAT = (guid) => $"api/article/{guid}";
    }
}