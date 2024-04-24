using MassTransit;
using RabbitMQ.MassTransit.Consumer.Components;
using System.Reflection;

namespace RabbitMQ.MassTransit.Consumer;
public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var app = builder.BuildServicesConfiguration().Build();

        
        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }

    public static WebApplicationBuilder BuildServicesConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddRazorComponents()
            .AddInteractiveServerComponents();


        builder.Services.AddMassTransit(x =>
        {
            var entryAssembly = Assembly.GetExecutingAssembly();
            x.AddConsumers(entryAssembly);
            x.UsingRabbitMq((ctx, cfg) => {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(ctx);
            });

        });

        return builder;
    }
}
