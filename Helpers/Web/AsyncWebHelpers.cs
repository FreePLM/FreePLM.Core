// Cloned From TrueNorthPLM, Free License 08-02-2025 //

/*

// In Program.cs or Startup.cs
builder.Services.AddWebHelpers(builder.Configuration);

// Or with direct provider configuration
builder.Services.AddWebHelpers(CloudProvider.Azure, "your-key-here");

 */

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace FreePLM.Core.Helpers.Web
{
    public interface IAsyncWebHelpers
    {
        /// <summary>
        /// API key used for authorization in HTTP headers.
        /// </summary>
        string ApiKey { get; set; }

        /// <summary>
        /// Adds a single HTTP header to be included in subsequent requests.
        /// If a header with the same name already exists, it will be updated with the new value.
        /// </summary>
        /// <param name="name">The name of the HTTP header.</param>
        /// <param name="value">The value of the HTTP header.</param>
        /// <exception cref="ArgumentNullException">Thrown when name or value is null.</exception>
        /// <exception cref="ArgumentException">Thrown when name is empty or contains invalid characters.</exception>
        void AddHeader(string name, string value);

        /// <summary>
        /// Adds multiple HTTP headers to be included in subsequent requests.
        /// For each header, if one with the same name already exists, it will be updated with the new value.
        /// </summary>
        /// <param name="headers">Dictionary containing header names and their corresponding values.</param>
        /// <exception cref="ArgumentNullException">Thrown when headers dictionary is null.</exception>
        /// <exception cref="ArgumentException">Thrown when any header name is empty or contains invalid characters.</exception>
        void AddHeaders(IDictionary<string, string> headers);

        /// <summary>
        /// Removes all custom HTTP headers that have been previously added.
        /// This does not affect the API key header which is managed separately.
        /// </summary>
        void ClearHeaders();

        /// <summary>
        /// Sends a DELETE request to the specified URL and returns a typed response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <param name="jsonConverter">Optional custom JSON converter.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        /// <example>
        /// <code>
        /// // Delete a user and get the updated user count
        /// var userCount = await webHelper.DeleteAsync<int>("api/users/123");
        /// </code>
        /// </example>

        Task<T?> DeleteAsync<T>(string url, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a DELETE request with a body to the specified URL.
        /// </summary>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if the operation succeeded, false otherwise.</returns>
        /// <example>
        /// <code>
        /// // Delete users matching certain criteria
        /// var criteria = new DeleteCriteria { Department = "IT", InactiveFor = TimeSpan.FromDays(90) };
        /// var success = await webHelper.DeleteAsync("api/users/batch", criteria);
        /// </code>
        /// </example>
        Task<bool> DeleteAsync<S>(string url, S stringValue, CancellationToken cancellationToken = default, bool allowFailure = false);


        /// <summary>
        /// Sends a DELETE request with a body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="jsonConverter">Optional custom JSON converter for response deserialization.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        Task<T?> DeleteWithBodyAsync<T, S>(string url, S stringValue, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a GET request and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <param name="jsonConverter">Optional custom JSON converter.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        /// <example>
        /// <code>
        /// // Get user details
        /// var user = await webHelper.GetAsync<UserDto>("api/users/123");
        /// </code>
        /// </example>
        Task<T?> GetAsync<T>(string url, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a GET request with a body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="jsonConverter">Optional custom JSON converter.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        /// <example>
        /// <code>
        /// // Get filtered user list
        /// var filter = new UserFilter { Role = "Admin", Status = "Active" };
        /// var users = await webHelper.GetAsync<List<UserDto>, UserFilter>("api/users/search", filter);
        /// </code>
        /// </example>
        Task<T?> GetAsync<T, S>(string url, S stringValue, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a PATCH request with a body.
        /// </summary>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the PATCH request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if the operation succeeded, false otherwise.</returns>
        /// <example>
        /// <code>
        /// // Update user status
        /// var patch = new { Status = "Inactive" };
        /// var success = await webHelper.PatchAsync("api/users/123", patch);
        /// </code>
        /// </example>
        Task<bool> PatchAsync<S>(string url, S stringValue, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a POST request with a body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="contentValue">The object to serialize as the request body.</param>
        /// <param name="jsonConverter">Optional custom JSON converter.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        /// <example>
        /// <code>
        /// // Create a new user
        /// var newUser = new CreateUserDto { Name = "John Doe", Email = "john@example.com" };
        /// var createdUser = await webHelper.PostAsync<UserDto, CreateUserDto>("api/users", newUser);
        /// </code>
        /// </example>
        Task<T?> PostAsync<T, S>(string url, S contentValue, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);
        /// <summary>
        /// Sends a PUT request with a body.
        /// </summary>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>True if the operation succeeded, false otherwise.</returns>
        /// <example>
        /// <code>
        /// // Update user information
        /// var updatedUser = new UserDto 
        /// { 
        ///     Id = 123,
        ///     Name = "John Doe Updated",
        ///     Email = "john.updated@example.com"
        /// };
        /// var success = await webHelper.PutAsync("api/users/123", updatedUser);
        /// </code>
        /// </example>
        Task<bool> PutAsync<S>(string url, S stringValue, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a PUT request without a body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="jsonConverter">Optional custom JSON converter for response deserialization.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        /// <example>
        /// <code>
        /// // Activate a user account and get the updated user status
        /// var userStatus = await webHelper.PutAsync<UserStatusDto>("api/users/123/activate");
        /// </code>
        /// </example>
        Task<T?> PutAsync<T>(string url, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);

        /// <summary>
        /// Sends a PUT request with a body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="jsonConverter">Optional custom JSON converter for response deserialization.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        /// <example>
        /// <code>
        /// // Update user role and get the updated user permissions
        /// var roleUpdate = new UpdateRoleDto { Role = "Administrator" };
        /// var permissions = await webHelper.PutAsync<UserPermissionsDto, UpdateRoleDto>(
        ///     "api/users/123/role",
        ///     roleUpdate);
        /// </code>
        /// </example>
        Task<T?> PutAsync<T, S>(string url, S stringValue, JsonConverter? jsonConverter = null, CancellationToken cancellationToken = default, bool allowFailure = false);
    }

    /// <summary>
    /// A helper class for performing asynchronous HTTP operations using HttpClient.
    /// Supports GET, POST, PUT, PATCH, and DELETE operations with an API key for authorization.
    /// </summary>
    public class AsyncWebHelpers : IAsyncWebHelpers
    {
        private readonly ILogger<AsyncWebHelpers> _logger;
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _customHeaders = new();
        private readonly AsyncWebHelpersOptions _asyncWebHelpersOptions;

        /// <summary>
        /// API key used for authorization in HTTP headers.
        /// This property is maintained for backward compatibility.
        /// </summary>
        public string ApiKey
        {
            get => _asyncWebHelpersOptions.AuthenticationKey;
            set => _asyncWebHelpersOptions.AuthenticationKey = value;
        }

        /// <summary>
        /// Constructor for AsyncWebHelpers.
        /// </summary>
        /// <param name="loggerFactory">Factory for creating loggers to log the class's activities.</param>
        /// <param name="httpClient">The HttpClient instance used to send HTTP requests.</param>
        public AsyncWebHelpers(
            ILoggerFactory loggerFactory,
            HttpClient httpClient,
            AsyncWebHelpersOptions asyncWebHelpersOptions)
        {
            _logger = loggerFactory.CreateLogger<AsyncWebHelpers>();
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _asyncWebHelpersOptions = asyncWebHelpersOptions ?? throw new ArgumentNullException(nameof(asyncWebHelpersOptions));
        }


        /// <summary>
        /// Adds a single HTTP header to be included in subsequent requests.
        /// If a header with the same name already exists, it will be updated with the new value.
        /// </summary>
        /// <param name="name">The name of the HTTP header.</param>
        /// <param name="value">The value of the HTTP header.</param>
        /// <exception cref="ArgumentNullException">Thrown when name or value is null.</exception>
        /// <exception cref="ArgumentException">Thrown when name is empty or contains invalid characters.</exception>
        public void AddHeader(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Header name cannot be null or empty.", nameof(name));

            if (value == null)
                throw new ArgumentNullException(nameof(value), "Header value cannot be null.");

            _customHeaders[name] = value;
        }

        /// <summary>
        /// Adds multiple HTTP headers to be included in subsequent requests.
        /// For each header, if one with the same name already exists, it will be updated with the new value.
        /// </summary>
        /// <param name="headers">Dictionary containing header names and their corresponding values.</param>
        /// <exception cref="ArgumentNullException">Thrown when headers dictionary is null.</exception>
        /// <exception cref="ArgumentException">Thrown when any header name is empty or contains invalid characters.</exception>
        public void AddHeaders(IDictionary<string, string> headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers), "Headers dictionary cannot be null.");

            foreach (var header in headers)
            {
                AddHeader(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Removes all custom HTTP headers that have been previously added.
        /// This does not affect the API key header which is managed separately.
        /// </summary>
        public void ClearHeaders()
        {
            _customHeaders.Clear();
        }

        /// <summary>
        /// Applies all configured headers to the HttpClient, including the API key and custom headers.
        /// This method is called automatically before each request.
        /// </summary>
        private void ApplyHeaders()
        {
            _httpClient.DefaultRequestHeaders.Clear();

            // Add cloud provider authentication header if key is present
            if (!string.IsNullOrEmpty(_asyncWebHelpersOptions.AuthenticationKey))
            {
                _httpClient.DefaultRequestHeaders.Add(
                    _asyncWebHelpersOptions.GetAuthHeaderName(),
                    _asyncWebHelpersOptions.GetAuthHeaderValue());
            }

            // Add custom headers
            foreach (var header in _customHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        /// <summary>
        /// Sends a GET request to the specified URL and deserializes the response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <returns>An object of type T if the request is successful, otherwise null.</returns>
        public async Task<T?> GetAsync<T>(
                                                string url,
                                                JsonConverter? jsonConverter = null,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing GET request to {Url}", url);

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.GetAsync(url, cancellationToken), allowFailure: allowFailure);

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing GET request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a GET request with a serialized object of type S as content, and deserializes the response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of object to be serialized and sent as the request body.</typeparam>
        /// <param name="url">The URL to send the GET request to.</param>
        /// <param name="stringValue">The object to be serialized and sent as content in the request.</param>
        /// <returns>An object of type T if the request is successful, otherwise null.</returns>
        public async Task<T?> GetAsync<T, S>(
                                                string url,
                                                S stringValue,
                                                JsonConverter? jsonConverter = null,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing GET request with body to {Url}", url);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(stringValue),
                        Encoding.UTF8,
                        "application/json")
                };

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.SendAsync(httpRequestMessage, cancellationToken), allowFailure: allowFailure);

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing GET request with body to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a POST request to the specified URL with a serialized object of type S as the request body, and deserializes the response into an object of type T.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of object to be serialized and sent as the request body.</typeparam>
        /// <param name="url">The URL to send the POST request to.</param>
        /// <param name="contentValue">The object to be serialized and sent as content in the POST request.</param>
        /// <returns>An object of type T if the request is successful, otherwise null.</returns>
        public async Task<T?> PostAsync<T, S>(
                                                string url,
                                                S contentValue,
                                                JsonConverter? jsonConverter = null,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing POST request to {Url}", url);

                var content = new StringContent(
                    JsonConvert.SerializeObject(contentValue),
                    Encoding.UTF8,
                    "application/json");

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.PostAsync(url, content, cancellationToken), allowFailure: allowFailure);

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing POST request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a PUT request to the specified URL with a serialized object of type T as the request body.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized and sent as the request body.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="stringValue">The object to be serialized and sent as content in the PUT request.</param>
        public async Task<bool> PutAsync<S>(
                                                string url,
                                                S stringValue,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing PUT request to {Url}", url);

                var content = new StringContent(
                    JsonConvert.SerializeObject(stringValue),
                    Encoding.UTF8,
                    "application/json");

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.PutAsync(url, content, cancellationToken), allowFailure: allowFailure);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing PUT request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a PUT request without any request body to the specified URL and deserializes the response into an object of type S.
        /// </summary>
        /// <typeparam name="S">The type of object to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <returns>An object of type S if the request is successful, otherwise null.</returns>
        public async Task<T?> PutAsync<T>(
                                                string url,
                                                JsonConverter? jsonConverter = null,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing PUT request to {Url}", url);

                var content = new StringContent(
                    string.Empty,
                    Encoding.UTF8,
                    "application/json");

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.PutAsync(url, content, cancellationToken), allowFailure: allowFailure);

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing PUT request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a PUT request to the specified URL with a serialized object of type T as the request body, and deserializes the response into an object of type S.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized and sent as the request body.</typeparam>
        /// <typeparam name="S">The type of object to deserialize the response into.</typeparam>
        /// <param name="url">The URL to send the PUT request to.</param>
        /// <param name="stringValue">The object to be serialized and sent as content in the PUT request.</param>
        /// <returns>An object of type S if the request is successful, otherwise null.</returns>
        public async Task<T?> PutAsync<T, S>(
                                                string url,
                                                S stringValue,
                                                JsonConverter? jsonConverter = null,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing PUT request to {Url}", url);

                var content = new StringContent(
                    JsonConvert.SerializeObject(stringValue),
                    Encoding.UTF8,
                    "application/json");

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.PutAsync(url, content, cancellationToken), allowFailure: allowFailure);

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing PUT request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a PATCH request to the specified URL with a serialized object of type T as the request body.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized and sent as the request body.</typeparam>
        /// <param name="url">The URL to send the PATCH request to.</param>
        /// <param name="stringValue">The object to be serialized and sent as content in the PATCH request.</param>
        public async Task<bool> PatchAsync<S>(
                                                string url,
                                                S stringValue,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing PATCH request to {Url}", url);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, url)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(stringValue),
                        Encoding.UTF8,
                        "application/json")
                };

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.SendAsync(httpRequestMessage, cancellationToken), allowFailure: allowFailure);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing PATCH request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a DELETE request to the specified URL with a serialized object of type T as the request body.
        /// </summary>
        /// <typeparam name="T">The type of object to be serialized and sent as the request body.</typeparam>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <param name="stringValue">The object to be serialized and sent as content in the DELETE request.</param>
        public async Task<bool> DeleteAsync<S>(
                                                string url,
                                                S stringValue,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing DELETE request with body to {Url}", url);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(stringValue),
                        Encoding.UTF8,
                        "application/json")
                };

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.SendAsync(httpRequestMessage, cancellationToken), allowFailure: allowFailure);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing DELETE request with body to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a DELETE request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the DELETE request to.</param>
        public async Task<T?> DeleteAsync<T>(
                                                string url,
                                                JsonConverter? jsonConverter = null,
                                                CancellationToken cancellationToken = default,
                                                bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing DELETE request to {Url}", url);

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.DeleteAsync(url, cancellationToken), allowFailure: allowFailure);

                var content = await response.Content.ReadAsStringAsync();

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing DELETE request to {Url}", url);
                throw;
            }
        }

        /// <summary>
        /// Sends a DELETE request with a body and deserializes the response.
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response into.</typeparam>
        /// <typeparam name="S">The type of the request body.</typeparam>
        /// <param name="url">The URL to send the DELETE request to.</param>
        /// <param name="stringValue">The object to serialize as the request body.</param>
        /// <param name="jsonConverter">Optional custom JSON converter for response deserialization.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>Deserialized response of type T or null.</returns>
        public async Task<T?> DeleteWithBodyAsync<T, S>(
            string url,
            S stringValue,
            JsonConverter? jsonConverter = null,
            CancellationToken cancellationToken = default,
            bool allowFailure = false)
        {
            try
            {
                _logger.LogDebug("Executing DELETE request with body to {Url}", url);

                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, url)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(stringValue),
                        Encoding.UTF8,
                        "application/json")
                };

                var response = await ExecuteRequestAsync(() =>
                    _httpClient.SendAsync(httpRequestMessage, cancellationToken), allowFailure: allowFailure);

                return await DeserializeResponseAsync<T>(response, jsonConverter);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "Error executing DELETE request with body to {Url}", url);
                throw;
            }
        }


        private async Task<HttpResponseMessage> ExecuteRequestAsync(Func<Task<HttpResponseMessage>> request, bool allowFailure = false)
        {
            ApplyHeaders();
            var response = await request();

            // only allow failure if explicitly requested. otherwise, throw an exception on failure
            if (!allowFailure)
                response.EnsureSuccessStatusCode();

            return response;
        }

        private async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response, JsonConverter? jsonConverter = null)
        {
            string content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(content)) return default;
            return jsonConverter != null
                ? JsonConvert.DeserializeObject<T>(content, jsonConverter)
                : JsonConvert.DeserializeObject<T>(content);
        }

    }

    /*
     
     public class UserService
    {
        private readonly IAsyncWebHelpers _webHelpers;
        private readonly ILogger<UserService> _logger;

        public UserService(IAsyncWebHelpers webHelpers, ILogger<UserService> logger)
        {
            _webHelpers = webHelpers;
            _logger = logger;
        }

        public async Task<List<UserDto>?> GetUsersWithTimeoutAsync(UserFilterDto filter, int timeoutSeconds = 30)
        {
            // Create a cancellation token source with timeout
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));

            try
            {
                // Make the API call with cancellation token
                var users = await _webHelpers.GetAsync<List<UserDto>, UserFilterDto>(
                    "api/users/search",
                    filter,
                    cancellationToken: cts.Token);

                return users;
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Request to get users was cancelled due to timeout after {Timeout} seconds", timeoutSeconds);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users with filter");
                throw;
            }
        }

        public async Task<List<UserDto>?> GetUsersWithCancellationAsync(
            UserFilterDto filter,
            IProgress<int>? progress = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                // Simulate progress updates
                for (int i = 0; i < 100 && !cancellationToken.IsCancellationRequested; i += 10)
                {
                    progress?.Report(i);
                    await Task.Delay(200, cancellationToken);
                }

                var users = await _webHelpers.GetAsync<List<UserDto>, UserFilterDto>(
                    "api/users/search",
                    filter,
                    cancellationToken: cancellationToken);

                progress?.Report(100);
                return users;
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("User search operation was cancelled by the user");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users with filter");
                throw;
            }
        }
    }


    public class UserViewModel
    {
        private readonly UserService _userService;
        private CancellationTokenSource? _searchCts;

        public async Task SearchUsersAsync()
        {
            try
            {
                // Cancel any existing search
                _searchCts?.Cancel();
                _searchCts = new CancellationTokenSource();

                var progress = new Progress<int>(percent =>
                {
                    // Update UI with progress
                    SearchProgress = percent;
                });

                var filter = new UserFilterDto
                {
                    Department = "IT",
                    Status = "Active"
                };

                var users = await _userService.GetUsersWithCancellationAsync(
                    filter,
                    progress,
                    _searchCts.Token);

                if (users != null)
                {
                    // Update UI with results
                    Users = users;
                }
            }
            finally
            {
                _searchCts?.Dispose();
                _searchCts = null;
            }
        }

        public void CancelSearch()
        {
            _searchCts?.Cancel();
        }
    }
     
     
     */
}