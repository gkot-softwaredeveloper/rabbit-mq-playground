
using MassTransit;
using RabbitMQ.MassTransit.Producer.Events.Senders;

namespace RabbitMQ.MassTransit.Producer;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var app = builder.BuildServicesConfiguration()
                         .Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }

    public static WebApplicationBuilder BuildServicesConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddTransient<IPingEventSender, PingEventSender>();

        builder.Services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((ctx, cfg) => {
                cfg.Host("localhost", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

        });

        return builder;
    }
}

