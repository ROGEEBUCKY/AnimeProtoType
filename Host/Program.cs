using AnimeProtoType.Host.Filter;
using AnimeProtoType.Host.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
    {
    app.UseSwagger();
    //app.UseSwaggerUI();

    app.UseSwaggerUI(options =>
    {
        ///****
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var description in provider.ApiVersionDescriptions)
            {
            options.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToString()
                );
            }

        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();

        options.OAuthClientId("interactive"); // client id in Identity Server
        options.OAuthClientSecret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0"); // client Secret from Identity Server
        options.OAuthAppName("AnimeProtoType");
        options.OAuthScopeSeparator(" ");
        options.OAuthUsePkce(); // Only applied to Authorizationcode flow

    });
    }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
