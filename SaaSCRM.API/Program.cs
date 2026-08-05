using Microsoft.EntityFrameworkCore;
using SaaSCRM.Application.Interfaces;
using SaaSCRM.Application.Services;
using SaaSCRM.Infrastructure.Persistence;
using SaaSCRM.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddScoped<TenantService>();

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

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
