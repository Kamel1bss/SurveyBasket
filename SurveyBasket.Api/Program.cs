using SurveyBasket.Api.Middlewares;
using SurveyBasket.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// keyed service
builder.Services.AddKeyedScoped<IOperatingSystem, MacOS>("MAC");
builder.Services.AddKeyedScoped<IOperatingSystem, Linux>("LINUX");

var app = builder.Build();

// Configure the HTTP request pipeline. (middlewares)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "V1"));
}

app.UseCustomMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
