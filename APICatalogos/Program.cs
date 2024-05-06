using APICatalogos.Context;
using APICatalogos.Dtos.Mappings;
using APICatalogos.Extesions;
using APICatalogos.Filters;
using APICatalogos.Logging;
using APICatalogos.Repositories;
using APICatalogos.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// deixar o path e a url em minuscula
builder.Services.AddRouting(options => options.LowercaseUrls = true);
// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(ApiExceptionFilter));
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
}).AddNewtonsoftJson();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

var valor1 = builder.Configuration["chave1"];
var valor2 = builder.Configuration["secao1:chave2"];


builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(mySqlConnection, ServerVersion.
                    AutoDetect(mySqlConnection)));


builder.Services.AddScoped<ApiLoggingFilter>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{
    LogLevel = LogLevel.Information
}));

builder.Services.AddAutoMapper(typeof(ProdutoDTOMappingProfile));

builder.Services.AddTransient<IMeuServico, MeuServico>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.DisableImplicitFromServicesParameters = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ConfigurationExceptionHandler();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
