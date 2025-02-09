
using MassTransit;
using MoneyMicroservice.Consumers;

namespace MoneyMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddDelayedMessageScheduler();
                cfg.AddConsumer<AddMoneyConsumer>();
                cfg.AddConsumer<GetMoneyConsumer>();
                cfg.UsingRabbitMq((brc, rbfc) =>
                {
                    rbfc.UseInMemoryOutbox();
                    rbfc.UseMessageRetry(r =>
                    {
                        r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
                    });
                    rbfc.UseDelayedMessageScheduler();
                    rbfc.Host("localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    rbfc.ConfigureEndpoints(brc);
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
