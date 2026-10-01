using MediatR;
using PruebaTecnica.Application.Commands;
using PruebaTecnica.Application.Queries;

namespace PruebaTecnica.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users")
            .WithTags("Users");

        group.MapPost("", async (
            CreateUserCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var userId = await sender.Send(command, cancellationToken);

            return Results.Created(
                $"/users/{userId}",
                new
                {
                    id = userId,
                    message = "Usuario creado correctamente."
                });
        })
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict);

        group.MapGet("", async (
            ISender sender,
            bool? isActive,
            CancellationToken cancellationToken) =>
        {
            var users = await sender.Send(
                new GetUsersQuery(isActive),
                cancellationToken);

            return Results.Ok(users);
        })
        .Produces(StatusCodes.Status200OK);

        group.MapGet("/{id:int}", async (
            int id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var user = await sender.Send(
                new GetUserByIdQuery(id),
                cancellationToken);

            return user is null
                ? Results.NotFound(new { message = "Usuario no encontrado." })
                : Results.Ok(user);
        })
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}", async (
            int id,
            UpdateUserCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var updated = await sender.Send(
                command with { Id = id },
                cancellationToken);

            return updated
                ? Results.NoContent()
                : Results.NotFound(new { message = "Usuario no encontrado." });
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        group.MapDelete("/{id:int}", async (
            int id,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var deleted = await sender.Send(
                new DeleteUserCommand(id),
                cancellationToken);

            return deleted
                ? Results.NoContent()
                : Results.NotFound(new { message = "Usuario no encontrado." });
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        group.MapGet("/{userId:int}/addresses", async (
            int userId,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var addresses = await sender.Send(
                new GetAddressesQuery(userId),
                cancellationToken);

            return Results.Ok(addresses);
        })
        .Produces(StatusCodes.Status200OK);

        group.MapPost("/{userId:int}/addresses", async (
            int userId,
            CreateAddressCommand command,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var addressId = await sender.Send(
                command with { UserId = userId },
                cancellationToken);

            return Results.Created(
                $"/users/{userId}/addresses",
                new
                {
                    id = addressId,
                    message = "Dirección creada correctamente."
                });
        })
        .Produces(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status404NotFound);

        return app;
    }
}
