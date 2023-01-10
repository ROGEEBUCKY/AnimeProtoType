using Infrastructure.Configration;
using Infrastructure.IRepository;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extension;
public static class Extension
{
    public static IServiceCollection AddConnection(this IServiceCollection service, string path)
    {
        service.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(path);
        });
        return service;
    }
    public static IServiceCollection UnitOfWork(this IServiceCollection service)
    {
        service.AddScoped<IUnitOfWork, UnitOfWork>();
        return service;
    }
}