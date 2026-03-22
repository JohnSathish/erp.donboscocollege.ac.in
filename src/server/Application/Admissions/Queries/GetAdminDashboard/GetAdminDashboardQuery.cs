using ERP.Application.Admissions.ViewModels;
using MediatR;

namespace ERP.Application.Admissions.Queries.GetAdminDashboard;

public sealed record GetAdminDashboardQuery() : IRequest<AdminDashboardDto>;














