using Job_Scheduler_webAPI.JobFactory;
using Job_Scheduler_webAPI.Jobs;
using Job_Scheduler_webAPI.Models;
using Job_Scheduler_webAPI.Schedular;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Job_Scheduler_webAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Job_Scheduler_webAPIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Job_Scheduler_webAPIContext") ?? throw new InvalidOperationException("Connection string 'Job_Scheduler_webAPIContext' not found.")));

//builder.Services.AddDbContext<Job_Scheduler_webAPIContext>(
//       options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

// Add services to the container.
builder.Services.AddControllers();
//builder.Services.AddTransient<ICountriesCrud,CountriesCrud>();
//builder.Services.AddTransient<DapperDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IJobFactory, MyJobFactory>();
builder.Services.AddSingleton<ISchedulerFactory,StdSchedulerFactory>();
builder.Services.AddSingleton<NotificationJob>();
builder.Services.AddSingleton(new JobMetadata(Guid.NewGuid() ,typeof(NotificationJob),"Notify Job", "0 */1 * ? * *"));
builder.Services.AddHostedService<MySchedular>();
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

