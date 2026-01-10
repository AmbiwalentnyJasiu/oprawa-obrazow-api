using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OprawaObrazow.Data;
using OprawaObrazow.Interceptors;
using OprawaObrazow.Modules.Auth;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Color;
using OprawaObrazow.Modules.Frame;
using OprawaObrazow.Modules.FramePiece;
using OprawaObrazow.Modules.Order;
using OprawaObrazow.Modules.Supplier;
using OprawaObrazow.Shared.Services;
using Serilog;

var builder = WebApplication.CreateBuilder( args );

builder.Logging.ClearProviders();
builder.Host.UseSerilog( ( context, configuration ) => configuration.ReadFrom.Configuration( context.Configuration ) );

// Add services to the container.

builder.Services.AddControllers();

//Database contexts
builder.Services.AddDbContext<DatabaseContext>()
       .AddDbContext<AuditContext>();

//Interceptors
builder.Services.AddScoped<SoftDeleteInterceptor>()
       .AddScoped<AuditInterceptor>();

//Repositories
builder.Services.AddScoped( typeof( IBaseRepository<> ), typeof( BaseRepository<> ) )
       .AddScoped<IUserRepository, UserRepository>();

//Services
builder.Services.AddScoped<IAuthService, AuthService>()
       .AddScoped<IPasswordHashService, PasswordHashService>()
       .AddScoped<IJwtService, JwtService>()
       .AddScoped<IImageService, ImageService>()
       .AddScoped<IColorService, ColorService>()
       .AddScoped<IFrameService, FrameService>()
       .AddScoped<IFramePieceService, FramePieceService>()
       .AddScoped<IOrderService, OrderService>()
       .AddScoped<ISupplierService, SupplierService>();

//Dto mappings
builder.Services.AddAutoMapper( typeof( MappingProfile ).Assembly );


builder.Services.AddAuthentication( options =>
       {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       } )
       .AddJwtBearer( options =>
       {
         options.TokenValidationParameters = new TokenValidationParameters
         {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = builder.Configuration["Jwt:Issuer"] ??
             throw new Exception( "Jwt:Issuer not found in configuration" ),
           ValidAudience = builder.Configuration["Jwt:Audience"] ??
             throw new Exception( "Jwt:Audience not found in configuration" ),
           IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( builder.Configuration["Jwt:Key"] ??
             throw new Exception(
               "Jwt:Key not found in configuration" ) ) )
         };
       } );

builder.Services.AddAuthorization();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

using ( var scope = app.Services.CreateScope() )
{
  var services = scope.ServiceProvider;
  var context = services.GetRequiredService<DatabaseContext>();
  context.Database.EnsureCreated();
  var auditContext = services.GetRequiredService<AuditContext>();
  auditContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if ( app.Environment.IsDevelopment() )
{
  app.MapOpenApi();
  app.MapGet( "/", () => Results.Redirect( "/openapi/v1.json" ) );
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();