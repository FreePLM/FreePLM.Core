// Cloned From TrueNorthPLM, Free License 08-02-2025 //

/*

// In Program.cs or Startup.cs
builder.Services.AddWebHelpers(builder.Configuration);

// Or with direct provider configuration
builder.Services.AddWebHelpers(CloudProvider.Azure, "your-key-here");

 */

namespace FreePLM.Core.Helpers.Web
{
    /// <summary>
    /// Supported cloud providers for authentication
    /// </summary>
    public enum CloudProvider
    {
        Azure,
        AWS,
        GCP,
        Custom
    }

    /// <summary>
    /// Configuration options for cloud provider authentication and other things
    /// </summary>
    public class AsyncWebHelpersOptions
    {
        /// <summary>
        /// The cloud provider to use for authentication
        /// </summary>
        public CloudProvider Provider { get; set; } = CloudProvider.Azure;

        /// <summary>
        /// The authentication key or token
        /// </summary>
        public string AuthenticationKey { get; set; } = string.Empty;

        /// <summary>
        /// Custom header name for authentication when using CloudProvider.Custom
        /// </summary>
        public string? CustomHeaderName { get; set; }

        /// <summary>
        /// Gets the appropriate authentication header name based on the cloud provider
        /// </summary>
        public string GetAuthHeaderName()
        {
            return Provider switch
            {
                CloudProvider.Azure => "x-functions-key",
                CloudProvider.AWS => "x-api-key",
                CloudProvider.GCP => "Authorization",
                CloudProvider.Custom when !string.IsNullOrEmpty(CustomHeaderName) => CustomHeaderName,
                CloudProvider.Custom => throw new InvalidOperationException("CustomHeaderName must be specified when using CloudProvider.Custom"),
                _ => throw new ArgumentException($"Unsupported cloud provider: {Provider}")
            };
        }

        /// <summary>
        /// Gets the formatted authentication header value based on the cloud provider
        /// </summary>
        public string GetAuthHeaderValue()
        {
            if (string.IsNullOrEmpty(AuthenticationKey))
            {
                throw new InvalidOperationException("AuthenticationKey must be specified");
            }

            return Provider switch
            {
                CloudProvider.GCP => $"Bearer {AuthenticationKey}",
                CloudProvider.Azure or CloudProvider.AWS or CloudProvider.Custom => AuthenticationKey,
                _ => throw new ArgumentException($"Unsupported cloud provider: {Provider}")
            };
        }
    }
}
