using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Statim.Jobs;

namespace Statim.Extensions;

public static class HostBuilderExtensions
{
    public static IHostBuilder UseStatim(this IHostBuilder builder)
    {
        return UseStatim(builder, options =>
        {
            
        });
    }

    public static IHostBuilder UseStatim(this IHostBuilder builder, Action<StatimOptions> options)
    {
        builder.ConfigureServices((context, services) =>
        {
            services.AddTransient(typeof(IDateTimeProvider), typeof(DateTimeProvider));
            services.AddTransient(typeof(IJobContainer), typeof(JobContainer));
            services.AddSingleton(typeof(IStatimScheduler), typeof(StatimScheduler));
            services.AddScoped<IJobContext, JobContext>();
            services.AddSingleton(typeof(IHostedService),
                (provider) =>
                {
                    var scheduler = provider.GetService<IStatimScheduler>();
                    
                    return scheduler!;
                });
        });

        return builder;
    }
}