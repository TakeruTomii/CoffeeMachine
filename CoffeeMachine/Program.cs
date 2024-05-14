using CoffeeMachine.Brewer;
using CoffeeMachine.Brewer.Interface;
using CoffeeMachine.Infrastracuture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IBrewService, BrewService>();
builder.Services.AddTransient<ICoffeeTimer, CoffeeTimer>();
builder.Services.AddSingleton<ICoffeeBrewer, CoffeeBrewer>();
builder.Services.AddSingleton<IHttpClientService, HttpClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
