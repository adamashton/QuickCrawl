﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Core;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Yelp
{
    public class ApiClient : IDisposable
    {
        private const string YelpBaseAddress = "https://api.yelp.com";

        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly Lazy<HttpClient> _client;

        public ApiClient(IOptions<YelpSettings> settings)
        {
            _clientId = settings.Value.ClientId;
            _clientSecret = settings.Value.ClientSecret;
            _client = new Lazy<HttpClient>(InitialiseClient);
        }

        public async Task<SearchResponse> Search(Coordinates coordinates, int radius, List<string> categories, int offset = 0, int limit = 20)
        {
            string requestUri = $"/v3/businesses/search?latitude={coordinates.Latitude}&longitude={coordinates.Longitude}&radius={radius}&offset={offset}&limit={limit}";

            if (categories != null && categories.Any())
            {
                string categoriesCommaSeparated = string.Join(",", categories);
                requestUri += "&categories=" + categoriesCommaSeparated;
            }

            HttpClient httpClient = _client.Value;
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
            
            if (!httpResponseMessage.IsSuccessStatusCode)
                await HandleError(httpResponseMessage, "Search failed.");

            string content = await httpResponseMessage.Content.ReadAsStringAsync();
            SearchResponse searchResponse = JsonConvert.DeserializeObject<SearchResponse>(content);

            return searchResponse;
        }

        private HttpClient InitialiseClient()
        {
            HttpClient result = new HttpClient()
            {
                BaseAddress = new Uri(YelpBaseAddress)
            };

            Task<AuthToken> getAuthTokenTask = Task.Run(async () => await GetAuthToken(result));
            getAuthTokenTask.Wait();
            AuthToken authToken = getAuthTokenTask.Result;

            if (authToken == null)
                throw new Exception("Could not get auth token");

            result.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authToken.token_type, authToken.access_token);
            return result;
        }

        private async Task<AuthToken> GetAuthToken(HttpClient httpClient)
        {
            string requestUri = "/oauth2/token";
            List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
            postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            postData.Add(new KeyValuePair<string, string>("client_id", _clientId));
            postData.Add(new KeyValuePair<string, string>("client_secret", _clientSecret));

            string response;
            using (var content = new FormUrlEncodedContent(postData))
            {
                content.Headers.Clear();
                content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                var responseMessage = await httpClient.PostAsync(requestUri, content);
                response = await responseMessage.Content.ReadAsStringAsync();

                if (!responseMessage.IsSuccessStatusCode)
                    await HandleError(responseMessage, "Could not obtain auth token.");
            }

            AuthToken authToken = JsonConvert.DeserializeObject<AuthToken>(response);
            return authToken;
        }

        private async Task HandleError(HttpResponseMessage httpResponseMessage, string message = "Error Response")
        {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            string request = httpResponseMessage.RequestMessage.RequestUri.ToString();
            throw new Exception($"{message}. Code:{httpResponseMessage.StatusCode} RequestUri:{request} Message:{responseContent}");
        }

        private class AuthToken
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public int expires_in { get; set; }
        }

        public void Dispose()
        {
            if (_client.IsValueCreated)
                _client.Value.Dispose();
        }
    }
}
