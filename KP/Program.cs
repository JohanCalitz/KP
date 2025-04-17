using API.Data;
using API.Data.StartupExtensions;
using API.Services;
using API.Services.Constants;
using API.Services.Interfaces;
using API.Services.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
                  .Enrich.FromLogContext()
                  .WriteTo.Console()
                  .CreateLogger();

Log.Information("Starting web api");
try
{
    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
    ConfigurationManager config = builder.Configuration;

    Log.Information("Adding dbcontext");
    builder.Services.AddDbContext<KingPriceDbContext>(options =>
        options.UseSqlServer(config[AppSettingKeyConstants.APIConnectionString] ?? throw new Exception($"Unable to get the database connectionstring"))); // we do this because in a production environment the key will live in a keyvault and not in our appsettings.json and return a error if not retrieved

    Log.Information("Adding system services");
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    Log.Information("Adding swagger");
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "King Price", Version = "v1" });

        // Add JWT Bearer auth definition
        c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Description = "Enter: **Bearer YOUR_JWT_TOKEN**"
        });

        // Apply Bearer scheme globally
        c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
    });

    Log.Information("Adding services");
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IGroupService, GroupService>();
    builder.Services.AddScoped<IPermissionService, PermissionService>();

    Log.Information("Adding Authentication");
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config[AppSettingKeyConstants.AuthIssuer],
            ValidAudience = config[AppSettingKeyConstants.AuthAudience],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[AppSettingKeyConstants.AuthKey])),
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Log.Information($"Authentication failed: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Log.Information($"Token validated for user: {context.Principal?.Identity?.Name}");
                return Task.CompletedTask;
            }
        };
    });

    Log.Information("Adding Rate Limiter");
    builder.Services.AddRateLimiter(rate => rate
    .AddFixedWindowLimiter(policyName: "fixed", options =>
    {
        options.PermitLimit = 50;
        options.Window = TimeSpan.FromMinutes(1);
        options.QueueLimit = 2;
        options.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    }));

    Log.Information("Adding Cors Policy");
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder =>
        {
            builder.AllowAnyOrigin()
                   .WithMethods("GET", "POST", "PUT", "DELETE")
                   .AllowAnyHeader();
        });
    });

    Log.Information("Adding mappers");
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    WebApplication app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    Log.Information("Adding security policy");

    HeaderPolicyCollection securityPolicy = new HeaderPolicyCollection()
    .AddFrameOptionsDeny()
    .AddXssProtectionBlock()
    .AddContentTypeOptionsNoSniff()
    .AddStrictTransportSecurityMaxAgeIncludeSubDomains(maxAgeInSeconds: 31536000)  // 1 year
    .RemoveServerHeader();

    app.UseCors("CorsPolicy");
    app.UseRateLimiter();

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    Log.Information("Starting with post application startup");
    using (IServiceScope scope = app.Services.CreateScope())
    {
        IServiceProvider service = scope.ServiceProvider;
        KingPriceDbContext? dbContext = service.GetService<KingPriceDbContext>();
        if (dbContext != null)
        {
            Log.Information("Handling migration");
            AppStartup.HandleMigration(dbContext);
            Log.Information("Seeding dummy data");
            AppStartup.SeedDummyData(dbContext);
        }
        else
        {
            Log.Error("Unable to get the dbcontext");
        }
    }

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application startup error");
    Log.CloseAndFlush();
    Console.ReadLine();
}
public partial class Program { }