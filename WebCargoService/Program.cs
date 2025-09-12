using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared;
using WebCargoService.HTTPInfrastructure;
using WebCargoService.Interfaces;
using WebCargoService.Repositories;
using WebCargoService.Services;

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
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);
    builder.Services.Configure<RouteOptions>(options => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    builder.Services.AddDbContext<WebCargoServiceDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebCargoDatabase")));
    builder.Services.AddHttpClient<IRateService, RateService>(httpClient => {
        IConfigurationSection configurationSection = builder.Configuration.GetSection("WebCargoAPI");
        httpClient.BaseAddress = new Uri(configurationSection["BaseAddress"]!);
    }).AddHttpMessageHandler<WebCargoAPIMessageHandler>();
    builder.Services.AddTransient<WebCargoAPIMessageHandler>();
    builder.Services.AddScoped<IRateRepository, RateRepository>();
}

void AddMiddleware() {
    webApplication.UseExceptionHandler("/error");
    if (builder.Environment.IsDevelopment()) {
        webApplication.UseSwagger();
        webApplication.UseCors();
    }
    webApplication.MapControllers();
}
