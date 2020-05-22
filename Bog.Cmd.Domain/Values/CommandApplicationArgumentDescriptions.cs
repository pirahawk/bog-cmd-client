namespace Bog.Cmd.Domain.Values
{
    public static class CommandApplicationArgumentDescriptions
    {
        public const string FILE_NAME = "The name of the file containing the entry text markdown";
        public const string BLOG_ID = "The Blog Id to use as context";
        public const string AUTHOR = "Article Author";
        public const string TITLE = "The title of the article";
        public const string DESCRIPTION = "A description of the article";
        public const string PUBLISH = "Mark as published";
        public const string UNPUBLISH = "Mark as not published";
        public const string ARTICLE_ID = "The Article Id to use within the current context";
        public const string MEDIA_GLOB_PATTERN = "The glob pattern to use to select media files in the current directory";
    }
}