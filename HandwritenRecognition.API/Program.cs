using AutoMapper;
using HandwritenRecognition.API.BackgroundWorkers;
using HandwritenRecognition.API.Notification;
using HandwritenRecognition.Cross.Infrastructure;
using HandwritenRecognition.Data;
using HandwritenRecognition.Data.UnitOfWork;
using HandwritenRecognition.Domain.MappingConfig;
using HandwritenRecognition.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OcrDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:OcrAPIConnectionString"],
        b => b.MigrationsAssembly("HandwritenRecognition.API"));
    options.EnableSensitiveDataLogging();
});
//builder.Services.AddTransient<AsyncPolicy>();
builder.Services.AddHttpClient<IRetryingHttpClient, RetryingHttpClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OcrAPISettings:BaseUrl"] ?? string.Empty);
    client.Timeout = TimeSpan.FromMinutes(builder.Configuration.GetValue<int>("OcrAPISettings:RequestTimeOutInMinutes"));
});

//Automapper
var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new OcrDocumentProfile()));
var mapperInstance = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapperInstance);

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IOcrService, OcrService>();
builder.Services.AddTransient<IDocumentService, DocumentService>();
builder.Services.AddTransient<IRuleService, RuleService>();

builder.Services.AddSignalR();
builder.Services.AddHostedService<PythonOcrWorker>();
builder.Services.AddHostedService<DocumentFeatureExtractorWorker>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        b => b.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();
//Add signal r hubs
app.MapHub<JobsHub>("/OcrJobsHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.UseCors("CorsPolicy");
app.Run();