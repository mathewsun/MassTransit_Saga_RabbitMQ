using MassTransit;
using SagasMicroservice.Saga;

namespace SagasMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddMassTransit(cfg =>
            {
                cfg.SetKebabCaseEndpointNameFormatter();
                cfg.AddDelayedMessageScheduler();
                //Тут добавляем сагу с указанием что будем сохранять ее в БД 
                //с помощью EF и будем использовать пессимистичный режим конкуренции за ресурсы
                cfg.AddSagaStateMachine<BuyItemsSaga, BuyItemsSagaState>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.ExistingDbContext<SagasDbContext>();
                    r.LockStatementProvider = new PostgresLockStatementProvider();
                });
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

            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            });

            app.Run();
        }
    }
}
