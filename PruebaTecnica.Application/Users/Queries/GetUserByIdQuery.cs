using MediatR;
using PruebaTecnica.Application.DTOs;

namespace PruebaTecnica.Application.Queries;

public record GetUserByIdQuery(int Id) : IRequest<UserResponse?>;