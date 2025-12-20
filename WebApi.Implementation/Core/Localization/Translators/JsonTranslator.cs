using System.Text.Json;
using Microsoft.Extensions.Hosting;
using WebApi.Application.Core.Localization;
using WebApi.Implementation.Core.Localization.Constants;

namespace WebApi.Implementation.Core.Localization.Translators;

public class JsonTranslator : ITranslator
{
    private readonly ILocaleResolver _localeResolver;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly Dictionary<string, Dictionary<string, object?>> _translations = new();

    public JsonTranslator(ILocaleResolver localeResolver, IHostEnvironment hostEnvironment)
    {
        _localeResolver = localeResolver;
        _hostEnvironment = hostEnvironment;
        LoadTranslations();
    }

    public string Translate(string key)
    {
        var locale = _localeResolver.Resolve();
        var localeCode = locale.TwoLetterISOLanguageName;

        if (!_translations.ContainsKey(localeCode))
        {
            localeCode = LocaleConstants.DefaultLocale;
        }

        if (!_translations.ContainsKey(localeCode))
        {
            return key;
        }

        var translation = GetNestedValue(_translations[localeCode], key);
        return translation ?? key;
    }

    private void LoadTranslations()
    {
        var translationsPath = Path.Combine(_hostEnvironment.ContentRootPath, "Core", "Localization", "Translations");

        if (!Directory.Exists(translationsPath))
        {
            return;
        }

        foreach (var localeCode in LocaleConstants.SupportedLocales)
        {
            var filePath = Path.Combine(translationsPath, $"{localeCode}.json");

            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                var translations = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonContent);

                if (translations != null)
                {
                    _translations[localeCode] = ParseJsonElements(translations);
                }
            }
        }
    }

    private static Dictionary<string, object?> ParseJsonElements(Dictionary<string, object> input)
    {
        var result = new Dictionary<string, object?>();

        foreach (var kvp in input)
        {
            if (kvp.Value is JsonElement element)
            {
                result[kvp.Key] = ParseJsonElement(element);
            }
            else
            {
                result[kvp.Key] = kvp.Value;
            }
        }

        return result;
    }

    private static object? ParseJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Object => element.EnumerateObject()
                .ToDictionary(prop => prop.Name, prop => ParseJsonElement(prop.Value)),
            JsonValueKind.Array => element.EnumerateArray()
                .Select(ParseJsonElement).ToArray(),
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Number => element.GetDouble(),
            JsonValueKind.Null => null,
            _ => element.ToString()
        };
    }

    private static string? GetNestedValue(Dictionary<string, object> dictionary, string key)
    {
        var parts = key.Split('.');
        object current = dictionary;

        foreach (var part in parts)
        {
            if (current is Dictionary<string, object> dict && dict.ContainsKey(part))
            {
                current = dict[part];
            }
            else
            {
                return null;
            }
        }

        return current?.ToString();
    }
}