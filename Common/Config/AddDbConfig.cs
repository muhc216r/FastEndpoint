using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Common;

public static partial class ServiceConfig
{
    public static IServiceCollection AddDbConfig<T>(this IServiceCollection services,string connection) where T : DbContext
    {
        services.AddDbContextPool<T>(x => x.UseSqlServer(connection));
        return services;
    }
}