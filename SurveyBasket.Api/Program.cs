using Microsoft.EntityFrameworkCore;
using SurveyBasket.Api;
using SurveyBasket.Api.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline. (middlewares)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
