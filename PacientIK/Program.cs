using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using PacientIK.Application;
using PacientIK.Endpoitns;
using PacientIK.Infrastructure;
using PacientIK.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<JwtService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddAuthExtension(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p => p
        .WithOrigins(
            "https://localhost:7211",
            "http://localhost:5211",
            "https://localhost:7000",
            "https://pacientikwebsite.onrender.com" 
        )
        .SetIsOriginAllowed(_ => true) 
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    );
});

builder.Services.AddOutputCache(o =>
{
    o.MaximumBodySize = 4 * 1024 * 1024;
    o.SizeLimit = 64 * 1024 * 1024;
    o.DefaultExpirationTimeSpan = TimeSpan.FromMinutes(5);
});
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
   
}

app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireCors("AllowAll");

app.MapGroup("api/user").RequireCors("AllowAll").UserMapEndpoiint();
app.MapGroup("api/login").RequireCors("AllowAll").LoginMapEndoint();
app.MapGroup("api/doc").RequireCors("AllowAll").ReportMapEndpoint();
app.MapGroup("api/spec").RequireCors("AllowAll").SpecMapEndoint();
app.MapGroup("api/lech").RequireCors("AllowAll").LechMapEndpoint();

app.Run();
