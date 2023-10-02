using LR5;
using LR5.file_logger;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseExceptionHandler((app) =>
{
    app.Run(async (context) =>
    {
        var loggerFactory = new LoggerFactory();
        loggerFactory.AddProvider(new FileLoggerProvider("./log.txt"));
        var logger = loggerFactory.CreateLogger<ILogger<Program>>();

        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var exceptionMessage = exceptionHandlerFeature?.Error.Message ??
            exceptionHandlerFeature?.Error.InnerException?.Message ??
            "";

        logger.LogError(exceptionMessage);

        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        await context.Response.WriteAsync(exceptionMessage);
        return;
    });
});

app.MapGet("/", async (HttpContext context) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.SendFileAsync("./static/index.html");
});

app.MapGet("/cookies", async (HttpContext context, ILogger<Program> logger) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append("<h1>Your cookies 🍪</h1>");
    if (context.Request.Cookies.Count != 0)
    {
        stringBuilder.Append("<ol>");
        var markupListElements = context.Request.Cookies.Select((KeyValuePair) => $"<li>{KeyValuePair.Key}={KeyValuePair.Value}</li>");
        stringBuilder.Append(String.Join("", markupListElements));
        stringBuilder.Append("</ol>");
    }
    else
    {
        stringBuilder.Append("<h3>No cookies :(</h3>");
    }

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync(stringBuilder.ToString());

    return;
});


app.MapPost("/", async (HttpContext context) =>
{
    var dto = await CookieSetterDto.DeserializeJSON(context.Request.Body);

    var cookieOptions = new CookieOptions();
    cookieOptions.Expires = dto.ExpDateTime;

    context.Response.Cookies.Append(
        dto.Key,
        dto.Value,
        cookieOptions
    );

    context.Response.ContentType = "text/html; charset=utf-8";
    await context.Response.WriteAsync($"🍪 Cookie \"{dto.Key}\" set to \"{dto.Value}\"");
});
app.Run();