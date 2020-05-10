using System;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OrchardCore.Entities;
using OrchardCore.Media;
using OrchardCore.Settings;

namespace OrchardCore.CustomStyleSettings.Services
{
    public class CustomStyleSettingsConfiguration : IConfigureOptions<CustomStyleSettings>
    {
        private readonly ISiteService _site;
        private readonly IMediaFileStore _mediaFileStore;
        private readonly ILogger<CustomStyleSettingsConfiguration> _logger;

        public CustomStyleSettingsConfiguration(
            ISiteService site,
            IMediaFileStore mediaFileStore,
            ILogger<CustomStyleSettingsConfiguration> logger)
        {
            _site = site;
            _mediaFileStore = mediaFileStore;
            _logger = logger;
        }

        public void Configure(CustomStyleSettings options)
        {
            var siteSettings = _site.GetSiteSettingsAsync().GetAwaiter().GetResult();

            if (siteSettings.Properties["CustomStyleSettings"] != null)
            {
                var CustomStyleSettingsJToken = siteSettings.Properties["CustomStyleSettings"]["CustomStyleSettingsPart"];
                var CustomStyleSettings = CustomStyleSettingsJToken.ToObject<CustomStyleSettings>();

                //Add public url to media fields
                CustomStyleSettings.SiteLogo.Paths[0] = _mediaFileStore.MapPathToPublicUrl(CustomStyleSettings.SiteLogo.Paths[0]);
                CustomStyleSettings.SiteFavicon.Paths[0] = _mediaFileStore.MapPathToPublicUrl(CustomStyleSettings.SiteFavicon.Paths[0]);


                options.SiteLogo = CustomStyleSettings.SiteLogo;
                options.SiteFavicon = CustomStyleSettings.SiteFavicon;
            }
        }
    }
}
