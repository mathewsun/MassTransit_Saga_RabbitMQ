using MassTransit;

namespace ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddDelayedMessageScheduler();
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
