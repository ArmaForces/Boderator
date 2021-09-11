﻿using System;
using System.Net.Http;
using System.Threading.Tasks;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.Collections;
using ArmaForces.Boderator.BotService.Tests.TestUtilities.TestFixtures;
using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Xunit;

namespace ArmaForces.Boderator.BotService.Tests.TestUtilities.TestBases
{
    /// <summary>
    /// Base class for integration tests involving API.
    /// Provider test server and methods to invoke endpoints.
    /// </summary>
    [Collection(CollectionsNames.ApiTest)]
    public abstract class ApiTestBase
    {
        private readonly HttpClient _httpClient;

        protected IServiceProvider Provider { get; }

        protected ApiTestBase(TestApiServiceFixture testApi)
        {
            _httpClient = testApi.HttpClient;

            Provider = new ServiceCollection()
                .BuildServiceProvider();
        }

        protected async Task<Result<T>> HttpGetAsync<T>(string path)
        {
            return await HttpGetAsync(path)
                .Bind(DeserializeContent<T>);
        }

        protected async Task<Result<string>> HttpGetAsync(string path)
        {
            var httpResponseMessage = await _httpClient.GetAsync(path);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return await httpResponseMessage.Content.ReadAsStringAsync();
            }

            var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            var error = string.IsNullOrWhiteSpace(responseBody)
                ? httpResponseMessage.ReasonPhrase
                : responseBody;

            return Result.Failure<string>(error);
        }

        protected async Task<Result> HttpPostAsync<T>(string path, T body)
        {
            var httpResponseMessage = await _httpClient.PostAsync(path, new StringContent(JsonConvert.SerializeObject(body)));
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            var responseBody = await httpResponseMessage.Content.ReadAsStringAsync();
            var error = string.IsNullOrWhiteSpace(responseBody)
                ? httpResponseMessage.ReasonPhrase
                : responseBody;

            return Result.Failure<T>(error);
        }

        private static Result<T> DeserializeContent<T>(string content)
            => JsonConvert.DeserializeObject<T>(content);
    }
}