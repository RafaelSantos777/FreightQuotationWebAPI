using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using QuotationService.Repositories;
using QuotationService.Interfaces;
using QuotationService.Services;
using Shared;

[assembly: ApiController]

ApplicationCulture.SetCulture();
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
AddServices();
WebApplication webApplication = builder.Build();
AddMiddleware();
webApplication.Run();
return;

void AddServices() {
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    if (builder.Environment.IsProduction())
        builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVaultURL"]!), new DefaultAzureCredential());
    builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "EntraID")
        .EnableTokenAcquisitionToCallDownstreamApi()
        .AddInMemoryTokenCaches();
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);
    builder.Services.Configure<RouteOptions>(options => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    builder.Services.AddDbContext<QuotationServiceDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("QuotationDatabase")));
    builder.Services.AddSingleton(new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"),
        new ServiceBusClientOptions { TransportType = ServiceBusTransportType.AmqpWebSockets }));
    builder.Services.AddHttpClient<WebCargoServiceClient>(client => {
        client.BaseAddress = new Uri(builder.Configuration["WebCargoServiceURL"]!);
    });
    builder.Services.AddHttpClient<CargofiveServiceClient>(client => {
        client.BaseAddress = new Uri(builder.Configuration["CargofiveServiceURL"]!);
    });
    builder.Services.AddScoped<IAirQuotationService, AirQuotationService>();
    builder.Services.AddScoped<ISeaQuotationService, SeaQuotationService>();
    builder.Services.AddScoped<ILocationService, LocationService>();
    builder.Services.AddScoped<IAirQuoteRepository, AirQuoteRepository>();
    builder.Services.AddScoped<ILocationRepository, LocationRepository>();
    builder.Services.AddScoped<ISpecialHandlingCodeRepository, SpecialHandlingCodeRepository>();
    builder.Services.AddSingleton<ISpecialHandlingCodeCache, SpecialHandlingCodeCache>();
    builder.Services.AddHostedService<DataFilesImportService>();
    builder.Services.AddHostedService<MessageReceiverService>();
}

void AddMiddleware() {
    webApplication.UseExceptionHandler("/error");
    if (builder.Environment.IsDevelopment()) {
        webApplication.UseSwagger();
        webApplication.UseCors();
    }
    webApplication.UseAuthorization();
    webApplication.MapControllers();
}
