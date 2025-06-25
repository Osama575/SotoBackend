using SotoGeneratorAPI.Services;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// 1) Register CORS to allow your React dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("LocalDev", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")  // React
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 2) Your other services
builder.Services.AddHttpClient();
builder.Services.AddScoped<SotoGeneratorService>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = ctx =>
        {
            var errors = ctx.ModelState
                .Where(ms => ms.Value.Errors.Count > 0)
                .SelectMany(ms => ms.Value.Errors.Select(e => e.ErrorMessage))
                .ToArray();
            return new BadRequestObjectResult(new { Message = "Validation failed", Errors = errors });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 3) Exception handler (unchanged)
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        context.Response.ContentType = "application/problem+json";
        var pd = new ProblemDetails();
        switch (ex)
        {
            case HttpRequestException _:
                context.Response.StatusCode = 503;
                pd.Title = "Upstream service unavailable";
                break;
            case JsonException _:
                context.Response.StatusCode = 400;
                pd.Title = "Invalid response format";
                break;
            default:
                context.Response.StatusCode = 500;
                pd.Title = "An unexpected error occurred";
                break;
        }
        pd.Status = context.Response.StatusCode;
        pd.Detail = ex?.Message;
        await JsonSerializer.SerializeAsync(context.Response.Body, pd);
    });
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 4) **Apply CORS before routing & authorization**
app.UseCors("LocalDev");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
