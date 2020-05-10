using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.CustomStyleSettings.Services;
using OrchardCore.Data.Migration;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;
using OrchardCore.Settings;

namespace OrchardCore.CustomStyleSettings
{
    public class Startup : StartupBase
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IDataMigration, Migrations>();
            services.AddScoped<IPermissionProvider, OrchardCore.CustomStyleSettings.Permissions>();
            services.AddScoped<INavigationProvider, OrchardCore.CustomStyleSettings.AdminMenu>();

            //Custom Settings
            services.AddTransient<IConfigureOptions<CustomStyleSettings>, CustomStyleSettingsConfiguration>();
            services.AddTransient<ICustomStyleSettingsService, CustomStyleSettingsService>();
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }
}
