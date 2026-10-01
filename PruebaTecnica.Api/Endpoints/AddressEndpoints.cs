using MediatR;
using PruebaTecnica.Application.Commands;

namespace PruebaTecnica.Api.Endpoints;

public static class AddressEndpoints
{
    public static IEndpointRouteBuilder MapAddressEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/addresses")
            .WithTags("Addresses");

        group.MapPut("/{id:int}", async (
            int id,
            UpdateAddressCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var updated = await sender.Send(
                command with { Id = id },
                cancellationToken);

            return updated
                ? Results.NoContent()
                : Results.NotFound(new { message = "Dirección no encontrada." });
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:int}", async (
            int id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var deleted = await sender.Send(
                new DeleteAddressCommand(id),
                cancellationToken);

            return deleted
                ? Results.NoContent()
                : Results.NotFound(new { message = "Dirección no encontrada." });
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
