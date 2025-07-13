using System.Data;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Application.ApplicationUsers;
using WebApi.Application.Localization;
using WebApi.DataAccess;

namespace WebApi.Implementation.Localization
{
    public class EFTranslator : ITranslator
    {
        private readonly DatabaseContext _dbContext;
        private readonly IApplicationUser _user;
        private readonly IMemoryCache _memoryCache;

        public EFTranslator(
            DatabaseContext dbContext,
            IApplicationUser user,
            IMemoryCache memoryCache
        )
        {
            _dbContext = dbContext;
            _user = user;
            _memoryCache = memoryCache;
        }

        public string Translate(string key)
        {
            var cachedTranslations = LoadLocalizationData();

            if (cachedTranslations.TryGetValue(key, out string? translatedValue))
            {
                return translatedValue;
            }

            return key;
        }

        private string CacheKey => $"LocalizationCache_{_user.Locale}";

        private Dictionary<string, string> LoadLocalizationData()
        {
            Dictionary<string, string> localizationData;

            if (_memoryCache.TryGetValue(CacheKey, out localizationData))
            {
                return localizationData;
            }

            localizationData = _dbContext.Translations
                .Where(x => x.Locale == _user.Locale.Name)
                .Select(x => new KeyValuePair<string, string>(x.Key, x.Value))
                .ToDictionary();

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(12));

            _memoryCache.Set(CacheKey, localizationData, cacheEntryOptions);

            return localizationData;
        }
    }
}
