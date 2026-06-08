using LoanApproval.Application.Repositories;
using LoanApproval.Infrastructure.Persistence;
using LoanApproval.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApproval.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add EF Core with PostgreSQL
        services.AddDbContext<AppWriteDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))
                .UseSnakeCaseNamingConvention());

        services.AddDbContext<AppQueryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("PostgreSQL"))
                .UseSnakeCaseNamingConvention());

        services.AddScoped<ILoanRepository, LoanRepository>();
        
        return services;
    }
}