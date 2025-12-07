// Cloned From TrueNorthPLM, Free License 08-02-2025 //

/*

// In Program.cs or Startup.cs
builder.Services.AddWebHelpers(builder.Configuration);

// Or with direct provider configuration
builder.Services.AddWebHelpers(CloudProvider.Azure, "your-key-here");

 */

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace FreePLM.Core.Helpers.Web
{
    /// <summary>
    /// Extension methods for registering AsyncWebHelpers and related services
    /// </summary>
    public static class WebHelperServiceExtensions
    {
        /// <summary>
        /// Adds AsyncWebHelpers and related services to the service collection
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <param name="configuration">The configuration containing cloud auth settings</param>
        /// <param name="configureClient">Optional action to configure the HttpClient</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddWebHelpers(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<HttpClient>? configureClient = null)
        {
            // Register options
            services.AddOptions();
            services.Configure<AsyncWebHelpersOptions>(configuration.GetSection("CloudAuth"));
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<AsyncWebHelpersOptions>>().Value);

            // Register named HttpClient with optional configuration
            services.AddHttpClient<IAsyncWebHelpers, AsyncWebHelpers>(client =>
            {
                // Default configuration
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30);

                // Apply custom configuration if provided
                configureClient?.Invoke(client);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }

        /// <summary>
        /// Adds AsyncWebHelpers with a specific cloud provider configuration
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to</param>
        /// <param name="provider">The cloud provider to use</param>
        /// <param name="authKey">The authentication key</param>
        /// <param name="configureClient">Optional action to configure the HttpClient</param>
        /// <returns>The service collection for chaining</returns>
        public static IServiceCollection AddWebHelpers(
            this IServiceCollection services,
            CloudProvider provider,
            string authKey,
            Action<HttpClient>? configureClient = null)
        {
            // Register options
            services.AddOptions();
            services.Configure<AsyncWebHelpersOptions>(options =>
            {
                options.Provider = provider;
                options.AuthenticationKey = authKey;
            });
            services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<AsyncWebHelpersOptions>>().Value);

            // Register named HttpClient with optional configuration
            services.AddHttpClient<IAsyncWebHelpers, AsyncWebHelpers>(client =>
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                client.Timeout = TimeSpan.FromSeconds(30);

                configureClient?.Invoke(client);
            })
            .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            return services;
        }
    }
}