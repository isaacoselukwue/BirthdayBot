using BirthdayBot;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton<Birthday>();
    })
    .Build();

await host.RunAsync();
