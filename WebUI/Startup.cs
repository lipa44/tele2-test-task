namespace WebUI;

using DataAccess;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Middlewares;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public class Startup
{
    public Startup(IConfiguration configuration) => Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(
                Configuration.GetConnectionString("MySqlServer"),
                new MySqlServerVersion(new Version(8, 0, 27))));

        services.AddSwaggerGen(opt =>
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Tele2 Test Task", Version = "v1" }));

        services.AddScoped<ICitizenService, CitizenService>();

        // services.AddAutoMapper(typeof(Startup));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tele2 Test Task v1");
            c.RoutePrefix = string.Empty;
        });

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseMiddleware<RequestValidationMiddleware>();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}