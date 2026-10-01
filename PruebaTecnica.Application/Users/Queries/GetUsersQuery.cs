using MediatR;
using PruebaTecnica.Application.DTOs;

namespace PruebaTecnica.Application.Queries;

public record GetUsersQuery(bool? IsActive = null)
    : IRequest<List<UserResponse>>;
