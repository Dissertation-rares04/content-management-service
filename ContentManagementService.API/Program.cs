using ContentManagementService.API;
using ContentManagementService.API.Authorization;
using ContentManagementService.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowedHosts", builder =>
    {
        builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
    });

    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(
            builder.Configuration.GetSection("Auth0").GetValue<string>("ClientOriginUrl"))
            .WithHeaders(new string[] {
                HeaderNames.ContentType,
                HeaderNames.Authorization,
            })
            .WithMethods("GET")
            .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.ConfigureServices(services =>
{
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var audience =
                  builder.Configuration.GetSection("Auth0").GetValue<string>("Audience");

            options.Authority =
                  $"https://{builder.Configuration.GetSection("Auth0").GetValue<string>("Domain")}/";
            options.Audience = audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuerSigningKey = true
            };
        });

    services.AddAuthorization(options =>
    {
        options.AddPolicy("read:admin-messages", policy =>
        {
            policy.Requirements.Add(new RbacRequirement("read:admin-messages"));
        });
        options.AddPolicy("delete:post", policy =>
        {
            policy.Requirements.Add(new RbacRequirement("delete:post"));
        });
        options.AddPolicy("delete:comment", policy =>
        {
            policy.Requirements.Add(new RbacRequirement("delete:comment"));
        });
    });

    services.AddSingleton<IAuthorizationHandler, RbacHandler>();

    services.ConfigureServices(builder.Configuration);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseErrorHandler();
app.UseSecureHeaders();

//app.UseHttpsRedirection();

app.MapControllers();
app.UseCors("AllowedHosts");

app.UseAuthentication();
app.UseAuthorization();

app.Run();
