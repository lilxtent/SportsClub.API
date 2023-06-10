using Clients.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{

    
    options.AddPolicy(name: "mycors",
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddLogging(config => config
    .ClearProviders()
    .AddConsole()
    .SetMinimumLevel(LogLevel.Debug)
);

builder.Services.AddDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("mycors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();