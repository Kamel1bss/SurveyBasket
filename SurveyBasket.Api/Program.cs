using SurveyBasket.Api;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies(); 

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
