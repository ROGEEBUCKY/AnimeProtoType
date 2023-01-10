using AnimeProtoType.Host.Filter;
using AnimeProtoType.Host.Middlewares;
using Infrastructure.Extension;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Host;
public static class Startup
{
    public static WebApplication Start(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();

        #region Connection
        builder.Services.AddConnection(builder.Configuration.GetConnectionString("DefaultConnection"));
        #endregion Connection

        //  Unit Of Work
        builder.Services.UnitOfWork();
        // MediatR
        builder.Services.AddMediatR(typeof(Program));
        //AutoMapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(async c =>
        {
            c.OperationFilter<AuthorizeCheckOperationFilter>();

            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
                        TokenUrl = new Uri("https://localhost:5001/connect/token"),
                        Scopes = new Dictionary<string, string>
                                 {
                                     {"openid", "kchjsad"}
                                 }
                    }
                }

            });
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
        });
        builder.Services.AddApiVersioning(config => {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
        return builder.Build();
    }
}
