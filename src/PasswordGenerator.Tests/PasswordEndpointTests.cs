using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PasswordGenerator.Api.Contracts;
using PasswordGenerator.Api.Models;
using PasswordGenerator.Api.Repositories;

namespace PasswordGenerator.Tests;

public class PasswordEndpointTests
{
    [Fact]
    public async Task PostPasswords_ReturnsCreatedWithPasswordAndGuid()
    {
        using var factory = CreateFactory();
        using HttpClient client = factory.CreateClient();

        using HttpResponseMessage response = await client.PostAsync("/api/passwords", content: null);
        PasswordResponse? body = await response.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(body);
        Assert.NotEqual(Guid.Empty, body.Id);
        Assert.Equal(16, body.Password.Length);
        Assert.Equal($"/api/passwords/{body.Id}", response.Headers.Location?.OriginalString);
    }

    [Fact]
    public async Task GetPassword_ReturnsPasswordForExistingGuid()
    {
        using var factory = CreateFactory();
        using HttpClient client = factory.CreateClient();

        using HttpResponseMessage createResponse = await client.PostAsync("/api/passwords", content: null);
        PasswordResponse? createdPassword = await createResponse.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.Created, createResponse.StatusCode);
        Assert.NotNull(createdPassword);

        using HttpResponseMessage getResponse = await client.GetAsync($"/api/passwords/{createdPassword.Id}");
        PasswordResponse? retrievedPassword = await getResponse.Content.ReadFromJsonAsync<PasswordResponse>();

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(retrievedPassword);
        Assert.Equal(createdPassword.Id, retrievedPassword.Id);
        Assert.Equal(createdPassword.Password, retrievedPassword.Password);
    }

    [Fact]
    public async Task GetPassword_ReturnsProblemDetailsForMalformedGuid()
    {
        using var factory = CreateFactory();
        using HttpClient client = factory.CreateClient();

        using HttpResponseMessage response = await client.GetAsync("/api/passwords/not-a-guid");
        ProblemDetails? problem = await response.Content.ReadFromJsonAsync<ProblemDetails>();

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);
        Assert.NotNull(problem);
        Assert.Equal(StatusCodes.Status400BadRequest, problem.Status);
    }

    [Fact]
    public async Task GetPassword_ReturnsNotFoundForUnknownGuid()
    {
        using var factory = CreateFactory();
        using HttpClient client = factory.CreateClient();

        using HttpResponseMessage response = await client.GetAsync($"/api/passwords/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    private static WebApplicationFactory<Program> CreateFactory()
    {
        return new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder => builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IPasswordRepository>();
                services.AddSingleton<IPasswordRepository, InMemoryPasswordRepository>();
            }));
    }

    private sealed class InMemoryPasswordRepository : IPasswordRepository
    {
        private readonly Dictionary<Guid, GeneratedPassword> passwords = new();

        public Task AddAsync(GeneratedPassword password, CancellationToken cancellationToken = default)
        {
            passwords.Add(password.Id, password);
            return Task.CompletedTask;
        }

        public Task<GeneratedPassword?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            passwords.TryGetValue(id, out GeneratedPassword? password);
            return Task.FromResult(password);
        }
    }
}