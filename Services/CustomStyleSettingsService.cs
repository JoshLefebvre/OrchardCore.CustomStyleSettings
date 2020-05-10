using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OrchardCore.Environment.Cache;

namespace OrchardCore.CustomStyleSettings.Services
{
    public class CustomStyleSettingsService : ICustomStyleSettingsService
    {
        private readonly CustomStyleSettings _options;
        private readonly ISignal _signal;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CustomStyleSettingsService> _logger;

        private const string CacheKey = "CustomStyleSettings";

        public CustomStyleSettingsService(
            IOptions<CustomStyleSettings> options, 
            ISignal signal,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider,
            ILogger<CustomStyleSettingsService> logger
            )
        {
            _options = options.Value;
            _signal = signal;
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public CustomStyleSettings GetCustomStyleSettings()
        {
            CustomStyleSettings settings;

            if (!_memoryCache.TryGetValue(CacheKey, out settings))
            {
                var session = GetSession();

                settings = new CustomStyleSettings()
                {
                    SiteLogo = _options.SiteLogo,
                    SiteFavicon = _options.SiteFavicon
                };

                if (settings != null)
                {
                    _memoryCache.Set(CacheKey, settings);
                    _signal.SignalToken(CacheKey);
                }
            }

            return settings;
        }

        private YesSql.ISession GetSession()
        {
            var httpContextAccessor = _serviceProvider.GetService<IHttpContextAccessor>();
            return httpContextAccessor.HttpContext.RequestServices.GetService<YesSql.ISession>();
        }
    }
}
