using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Writers;
using Microsoft.OpenApi.Extensions;
using PizzeriaWebApi_EF.Data;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.Identity;
using PizzeriaWebApi_EF.Services;
using System.Text;
using System.Text.Json.Serialization;
using TomasosPizzeria_API.Data.Interfaces;
using TomasosPizzeria_API.Data.Repos;
using TomasosPizzeria_API.Services;
using Swashbuckle.AspNetCore.Swagger;
using Newtonsoft.Json;
using PizzeriaWebApi_EF.Middleware;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // JWT-konfiguration
        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new InvalidOperationException("JWT key is missing in configuration (Jwt:Key).");

        var key = Encoding.UTF8.GetBytes(jwtKey);

        // Add controllers + JSON hantering
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Add DB Contexts
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlServer(connectionString));

        builder.Services.AddDbContext<ApplicationUserContext>(options =>
            options.UseSqlServer(connectionString));

        // Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationUserContext>()
            .AddDefaultTokenProviders();

        // JWT-auth
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        // Dependency Injection
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<IIngredientService, IngredientService>();
        builder.Services.AddScoped<IngredientRepository>();
        builder.Services.AddScoped<ICategoryService, CatagoryService>();
        builder.Services.AddScoped<CategoryRepository>();
        builder.Services.AddScoped<IDishService, DishService>();
        builder.Services.AddScoped<DishRepository>();
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<OrderRepository>();
        builder.Services.AddSingleton<JwtTokenGenerator>();

        // Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tomasos API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter 'Bearer {your token}'"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
            });
        });

        var app = builder.Build();

        // Skapa roller automatiskt
        using (var scope = app.Services.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = new[] { "Admin", "RegularUser", "PremiumUser" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        // Middleware pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapGet("/", () => Results.Redirect("/swagger"));
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        // Export Swagger JSON med Newtonsoft.Json
        app.MapGet("/export-swagger", (ISwaggerProvider swaggerProvider) =>
        {
            var swaggerDoc = swaggerProvider.GetSwagger("v1");

            using var stream = new MemoryStream();
            using var writer = new StreamWriter(stream);

            var jsonWriter = new OpenApiJsonWriter(writer); // INTE i using

            swaggerDoc.SerializeAsV3(jsonWriter); // Viktig metod för korrekt OpenAPI 3-export
            writer.Flush();

            var json = Encoding.UTF8.GetString(stream.ToArray());
            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "swagger.json");

            File.WriteAllText(outputPath, json);

            return Results.Ok("Swagger export complete.");
        });

        app.Run();
    }
}