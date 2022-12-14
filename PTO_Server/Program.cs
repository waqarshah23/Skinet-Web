using Microsoft.EntityFrameworkCore;
using NLog;
using PTO_Server.Extensions;
using PTO_Server.Extensions.AuthToken;
using PTO_Server.Extensions.Logger;
//using PTO_Server.Middleware;
using Infrastructure.Middleware;
using Infrastructure.Data;
using PTO_Server.Repository;
using PTO_Server.Repository.UserAuth;
using Core.Interfaces;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "/nlog.config.txt"));
builder.Services.ConfigureCors();
builder.Services.ConfigureJWTBearer();
builder.Services.AddMvc();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddScoped<IUserAuth, UserAuth>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("mysqlconnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.All });
app.UseCors("CorsPolicy");
app.UseRouting();
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
