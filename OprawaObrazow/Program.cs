using OprawaObrazow.Data;
using OprawaObrazow.Interceptors;
using OprawaObrazow.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddDbContext<AuditContext>();

builder.Services.AddScoped<SoftDeleteInterceptor>();
builder.Services.AddScoped<AuditInterceptor>();

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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