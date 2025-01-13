using InventoryManagement.Application.Items.Queries;
using InventoryManagement.Infra.IoC.DependencyInjection;
using InventoryManagement.Infra.IoC.Extensions;
using InventoryManagement.Infra.IoC.Mappings;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddInfrastructureAPI();
        builder.Services.AddAutoMapper(typeof(AutoMapperProfileConfiguration));

        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(GetAllItemQuery).Assembly);
        });

        // Configurar o logger
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.Logging.AddDebug();

        builder.Logging.AddFilter("Microsoft", LogLevel.Warning)
                        .AddFilter("System", LogLevel.Warning)
                        .AddFilter("InventoryManagement", LogLevel.Information);

        var app = builder.Build();

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
}