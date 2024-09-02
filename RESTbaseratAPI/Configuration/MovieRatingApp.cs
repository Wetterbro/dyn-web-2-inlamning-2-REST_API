using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.OpenApi.Models;
using RESTbaseratAPI.Data;
using RESTbaseratAPI.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RESTbaseratAPI.Configuration;

public class MovieRatingApp
{
    private readonly WebApplicationBuilder _builder;
    private readonly WebApplication _app;

    public MovieRatingApp(string[] args)
    {
        _builder = WebApplication.CreateBuilder(args);

        ConfigureServices(_builder);
        
        _app = _builder.Build();

        ConfigureMiddlewares();
    }

    private void ConfigureServices(WebApplicationBuilder _builder)
    {
        // Add services to the container.
        _builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(_builder.Configuration.GetConnectionString("SqlServer")));

        AddAuthenticationApi();

        _builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _builder.Services.AddEndpointsApiExplorer();
        _builder.Services.AddSwaggerGen(CustomSwaggerGenOptions);
    }

    private void AddAuthenticationApi()
    {
        _builder.Services.AddAuthorization();

        _builder.Services.AddIdentityApiEndpoints<CustomUser>(CustomIdentityOptions)
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    private void CustomIdentityOptions(IdentityOptions options)
    {
        if (_builder.Environment.IsDevelopment())
        {
            // Set simpler passwordRequirements for development
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        }
    }

    private void CustomSwaggerGenOptions(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>(); // Required for using access token
    }

    private void ConfigureMiddlewares()
    {
        // Configure the HTTP request pipeline.
        if (_app.Environment.IsDevelopment())
        {
            _app.UseSwagger();
            _app.UseSwaggerUI();
        }
        _app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
        
        _app.MapIdentityApi<CustomUser>();

        _app.UseHttpsRedirection();

        _app.UseAuthorization();

        _app.MapControllers();
    }

    public void Run()
    {
        _app.Run();
    }
}