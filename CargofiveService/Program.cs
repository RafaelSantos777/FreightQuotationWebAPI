using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared;
using CargofiveService.Interfaces;
using CargofiveService.Services;

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
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => {
        options.SuppressMapClientErrors = true;
    });
    builder.Services.Configure<RouteOptions>(options => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    builder.Services.AddHttpClient<IRateService, RateService>(ConfigureCargofiveHTTPClient);
    builder.Services.AddHttpClient<ISeaportService, SeaportService>(ConfigureCargofiveHTTPClient);
    builder.Services.AddHostedService<SeaportSynchronizationService>();
    return;

    void ConfigureCargofiveHTTPClient(HttpClient httpClient) {
        IConfigurationSection configurationSection = builder.Configuration.GetSection("CargofiveAPI");
        httpClient.DefaultRequestHeaders.Add("x-api-key", configurationSection["APIKey"]);
        httpClient.BaseAddress = new Uri(configurationSection["BaseAddress"]!);
    }
}

void AddMiddleware() {
    webApplication.UseExceptionHandler("/error");
    if (builder.Environment.IsDevelopment()) {
        webApplication.UseSwagger();
        webApplication.UseCors();
    }
    webApplication.MapControllers();
}
