using MediatR;

namespace ERP.Application.Admissions.Commands.LoginAdmin;

public sealed record LoginAdminCommand(string Username, string Password) : IRequest<LoginAdminResult>;














