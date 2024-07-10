using PruebaTecnica.Test.API.Helpers;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificationOrigins = "Permisos";


builder.Services.AddCors(options =>
{
	options.AddPolicy(name: MyAllowSpecificationOrigins,
						policy =>
						{
                            policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:44382", "http://localhost:5181","https://localhost:7215", "https://localhost:7165", "");
						});
});
// Configuraci�n de Rate Limiting
builder.Services.AddOptions();
builder.Services.AddMemoryCache();

// Carga la configuraci�n de limitaci�n de tasa desde appsettings.json
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimitPolicies"));

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
builder.Services.AddApplicationInsightsTelemetry();

builder.Logging.AddFilter<ApplicationInsightsLoggerProvider>("PruebaTecnica.Test.Api", LogLevel.Trace);

builder.Services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
                .ConfigureApiBehaviorOptions(options => { options.SuppressModelStateInvalidFilter = true; } ); //Suprime las validaciones de modelstate (dataannotation) antes de entrar al endpoint para darles un manejo personalizado

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Api Key debe aparecer en el header",
        Type = SecuritySchemeType.ApiKey,
        Name = "TestApiKey",
        In = ParameterLocation.Header,
        Scheme = "ApiKey"
    });
    var key = new OpenApiSecurityScheme()
    {
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "ApiKey"
        },
        In = ParameterLocation.Header
    };
    var requirement = new OpenApiSecurityRequirement
                    {
                             { key, new List<string>() }
                    };
    c.AddSecurityRequirement(requirement);
}
);
builder.Services.AddHostedService<CacheService>();

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.AddServerHeader = false;
});

var app = builder.Build();

// Swagger para ambientes diferentes de produccion
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors(MyAllowSpecificationOrigins);
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseIpRateLimiting();
app.UseClientRateLimiting();
app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();



app.Run();