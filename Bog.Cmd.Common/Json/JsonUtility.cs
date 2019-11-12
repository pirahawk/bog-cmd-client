using System;
using System.Text.Json;

namespace Bog.Cmd.Common.Json
{
    public static class JsonUtility
    {
        static JsonSerializerOptions Options
        {
            get
            {
                return new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    IgnoreNullValues = true,
                    AllowTrailingCommas = true,
                    WriteIndented = true
                };
            }
        }

        public static TModel Deserialize<TModel>(string modelText)
        {
            if (string.IsNullOrWhiteSpace(modelText))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(modelText));

            return JsonSerializer.Deserialize<TModel>(modelText, Options);
        }

        public static string Serialize<TModel>(TModel model)
        {
            return JsonSerializer.Serialize(model, Options);
        }

        public static string Prettify<TModel>(string content)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));

            var model = Deserialize<TModel>(content);
            var prettyContent = Serialize(model);

            return prettyContent;
        }
    }
}