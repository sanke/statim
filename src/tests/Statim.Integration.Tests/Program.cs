// See https://aka.ms/new-console-template for more information


using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Statim.Extensions;
using Statim.Integration.Tests;

var host = Host.CreateDefaultBuilder()
    .UseStatim()
    .ConfigureServices(services =>
    {
        services.AddHostedService<TestService>();
    })
    .Build();


await host.RunAsync();