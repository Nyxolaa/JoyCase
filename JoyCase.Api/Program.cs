using FluentValidation;
using JoyCase.Application.Common;
using JoyCase.Application.User.Validator;
using JoyCase.Data;
using JoyCase.Data.Repository;
using JoyCase.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog.Sinks.MSSqlServer;
using Serilog;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// jwt konfigurasyonu
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddDbContext<JoyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient();

// serilog konfigu
var logDB = builder.Configuration.GetConnectionString("LogDb");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console() // Konsola loglama
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Günlük log dosyası
    .WriteTo.MSSqlServer(
        connectionString: logDB,
        sinkOptions: new MSSqlServerSinkOptions { TableName = "Logs", AutoCreateSqlTable = false }
        //,
        //columnOptions: new ColumnOptions
        //{
        //    AdditionalColumns = new List<SqlColumn>
        //    {
        //        new SqlColumn("LogLevel", SqlDbType.NVarChar),
        //        new SqlColumn("Message", SqlDbType.NVarChar),
        //        new SqlColumn("Exception", SqlDbType.NVarChar),
        //        new SqlColumn("Timestamp", SqlDbType.DateTime2)
        //    }
        //}
    )
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// fluent validation 
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // swagger icin jwt authorization ayarlari
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
     {
         {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                     {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                     }
             },
             Array.Empty<string>()
         }
     });
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IJoyDbContext, JoyDbContext>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));


var app = builder.Build();
app.UseSerilogRequestLogging();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // jwt authentication
app.UseAuthorization(); // yetkilendirme

app.MapControllers();

app.Run();
