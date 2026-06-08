using LoanApproval.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LoanApproval.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });
        
        return services;
    }
}