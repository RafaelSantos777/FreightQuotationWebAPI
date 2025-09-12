using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Shared;
using UserService.Interfaces;
using UserService.Repositories;
using UserService.Services;

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
        .AddMicrosoftGraph(builder.Configuration.GetSection("GraphAPI"))
        .AddInMemoryTokenCaches();
    builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => options.SuppressMapClientErrors = true);
    builder.Services.Configure<RouteOptions>(options => {
        options.LowercaseUrls = true;
        options.LowercaseQueryStrings = true;
    });
    builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
    builder.Services.AddDbContext<UserServiceDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("UserDatabase")));
    builder.Services.AddSingleton(new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"),
        new ServiceBusClientOptions { TransportType = ServiceBusTransportType.AmqpWebSockets }));
    builder.Services.AddScoped<IUserQueryService, UserQueryService>();
    builder.Services.AddScoped<IDeltaLinkRepository, DeltaLinkRepository>();
    builder.Services.AddHostedService<MessageSenderService>();
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
