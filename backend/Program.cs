using backend.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy
                .WithOrigins("http://localhost:3000") // frontend
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials(); // SignalR needs this
        });
});

builder.Services.AddControllers();

builder.Services.AddSignalR();
builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<LinesHub>("/lineshub");

app.Run();
