using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionService.Application.Shared.Interfaces;
using TransactionService.Domain.Repositories;
using TransactionService.Domain.Repositories.Shared;
using TransactionService.Infrastructure.Http;
using TransactionService.Infrastructure.Persistence;
using TransactionService.Infrastructure.Persistence.Repositories;
using TransactionService.Infrastructure.Persistence.Repositories.Shared;

namespace TransactionService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHttpClient<IProductsApiClient, ProductsApiClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["ProductServiceApi:BaseUrl"]!);
            client.Timeout = TimeSpan.FromSeconds(30);
        });

        return services;
    }
}