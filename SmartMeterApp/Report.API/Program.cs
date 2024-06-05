using Microsoft.EntityFrameworkCore;
using ReportApi;
using ReportApi.BackgroundServices;
using ReportApi.Models;
using ReportApi.Services.RabbitMQService;
using ReportApi.Services.ReportService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ReportDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.Configure<RabbitMQConfiguration>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.Configure<MeterApiConfiguration>(builder.Configuration.GetSection("ConfigurationManager"));
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddHostedService<RabbitMQBackgroundService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

using(var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<ReportDbContext>();
    context.Database.Migrate();
}

app.Run();
