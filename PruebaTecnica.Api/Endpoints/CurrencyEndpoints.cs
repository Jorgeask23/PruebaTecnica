using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Queries;

namespace PruebaTecnica.Api.Endpoints;

public static class CurrencyEndpoints
{
    public static IEndpointRouteBuilder MapCurrencyEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/currencies")
            .WithTags("Currencies");

        group.MapGet("", async (
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var currencies = await sender.Send(
                new GetCurrenciesQuery(),
                cancellationToken);

            return Results.Ok(currencies);
        })
        .Produces(StatusCodes.Status200OK);

        group.MapPost("", async (
            CreateCurrencyCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var currencyId = await sender.Send(
                command,
                cancellationToken);

            return Results.Created(
                $"/currencies/{currencyId}",
                new
                {
                    id = currencyId,
                    message = "Moneda creada correctamente."
                });
        })
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);

        // Operación adicional: no es requerida por la consigna.
        group.MapPut("/{id:int}", async (
            int id,
            UpdateCurrencyCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            await sender.Send(
                command with { Id = id },
                cancellationToken);

            return Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        app.MapPost("/currency/convert", async (
            ConvertCurrencyCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var convertedAmount = await sender.Send(
                command,
                cancellationToken);

            return Results.Ok(new
            {
                fromCurrency = command.FromCurrencyCode,
                toCurrency = command.ToCurrencyCode,
                originalAmount = command.Amount,
                convertedAmount
            });
        })
        .WithTags("Currency Conversion")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);

        return app;
    }
}
