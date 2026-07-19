using Azure.Messaging.ServiceBus;
using DispatchMicroservice.Handlers;
using DispatchMicroservice.Hubs;
using DispatchMicroservice.Middleware;
using DispatchMicroservice.Services;
using DispatchMicroservice.Validation.Validator;
using DispatchMicroservice.Workers;
using DispatchProject.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Constants;
using Shared.Deserialization;
using Shared.DTO.Contracts.ApiResponses;
using Shared.Interfaces;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.AllowTrailingCommas = true;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressMapClientErrors = true;

        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices
                .GetRequiredService<ILogger<Program>>();

            logger.LogWarning(
                "A model validation error: {ModelState}",
                context.ModelState
            );

            var fields = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    x =>
                    {
                        var fieldName = x.Key.Split('.').Last();

                        return JsonNamingPolicy.CamelCase.ConvertName(fieldName);
                    },
                    x => x.Value!.Errors
                        .Select(error => error.ErrorMessage)
                        .FirstOrDefault(errorMessage => !string.IsNullOrWhiteSpace(errorMessage))
                        ?? ResponseCodes.Validation.InvalidRequest
                );

            return new BadRequestObjectResult(
                ApiResponseFactory.Validation(fields)
            );
        };
    });


builder.Services.AddSignalR();

builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(builder.Configuration.GetConnectionString("AzureServicePrimaryKey")));
builder.Services.AddSingleton<ServiceBusPublisher>();
builder.Services.AddSingleton<Validator>();
builder.Services.AddSingleton<RecipientValidator>();

builder.Services.AddScoped<LogResponseHandler>();
builder.Services.AddHostedService<LogResponseWorker>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<NotificationOperationService>();

builder.Services.AddScoped(
    typeof(IDeserializer<>),
    typeof(JsonMessageDeserializer<>));

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "http://localhost:8080")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        //policy.WithOrigins("http://localhost:5173") // for testing
        //      .AllowAnyHeader()
        //      .AllowAnyMethod()
        //      .AllowCredentials();
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequiredUniqueChars = 0;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.TryGetValue("access_token", out var token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

app.UseStatusCodePages(async statusCodeContext =>
{
    var httpContext = statusCodeContext.HttpContext;

    if (httpContext.Response.HasStarted)
    {
        return;
    }

    var code = httpContext.Response.StatusCode switch
    {
        StatusCodes.Status400BadRequest =>
            ResponseCodes.Http.BadRequest,

        StatusCodes.Status401Unauthorized =>
            ResponseCodes.Auth.Unauthorized,

        StatusCodes.Status403Forbidden =>
            ResponseCodes.Auth.Forbidden,

        StatusCodes.Status404NotFound =>
            ResponseCodes.Http.NotFound,

        StatusCodes.Status405MethodNotAllowed =>
            ResponseCodes.Http.MethodNotAllowed,

        StatusCodes.Status415UnsupportedMediaType =>
            ResponseCodes.Http.UnsupportedMediaType,

        _ => ResponseCodes.System.UnexpectedError
    };

    httpContext.Response.ContentType = "application/json";

    await httpContext.Response.WriteAsJsonAsync(
        ApiResponse.Fail(code)
    );
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();
app.UseMiddleware<OperationContextMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");
app.Run();

