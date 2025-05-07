using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoManagementApplication.Application.Interface;
using TodoManagementApplication.Application.Mapping;
using TodoManagementApplication.Domain.Data;
using TodoManagementApplication.Infrastructure.Repository;

namespace TodoManagementApplication.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("TodoConnectionString"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            builder.Services.AddControllers();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Todo Management API",
                    Version = "v1"
                });
            });

            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("frontendMVC", p =>
                {
                    p.AllowAnyOrigin()       //WithOrigins("http://localhost:1345")
                     .AllowAnyHeader()
                     .AllowAnyMethod();
                });
            });

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API v1"));
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("frontendMVC");
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
