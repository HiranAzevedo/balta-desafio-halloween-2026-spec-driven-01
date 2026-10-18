using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PasswordGenerator.Api.Contracts;
using PasswordGenerator.Api.Services;

namespace PasswordGenerator.Api.Endpoints;

public static class PasswordEndpoints
{
    public static void MapPasswordEndpoints(this WebApplication app)
    {
        app.MapPost("/api/passwords", CreatePasswordAsync);
        app.MapGet("/api/passwords/{id}", GetPasswordAsync);
    }

    private static async Task<Created<PasswordResponse>> CreatePasswordAsync(
        IPasswordService passwordService,
        CancellationToken cancellationToken)
    {
        var generatedPassword = await passwordService.GenerateAsync(cancellationToken);
        var response = new PasswordResponse(generatedPassword.Id, generatedPassword.Value);

        return TypedResults.Created($"/api/passwords/{generatedPassword.Id}", response);
    }

    private static async Task<Results<Ok<PasswordResponse>, NotFound, ProblemHttpResult>> GetPasswordAsync(
        string id,
        IPasswordService passwordService,
        CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(id, out Guid passwordId))
        {
            return TypedResults.Problem(
                title: "GUID inválido",
                detail: "O identificador deve estar em formato GUID.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        var generatedPassword = await passwordService.GetByIdAsync(passwordId, cancellationToken);

        if (generatedPassword is null)
        {
            return TypedResults.NotFound();
        }

        var response = new PasswordResponse(generatedPassword.Id, generatedPassword.Value);
        return TypedResults.Ok(response);
    }
}