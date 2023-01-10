using Host;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Start();

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
