using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface.IRepositories;
using SimpleDemo.Repository.Implements;
using SimpleDemo.Repository;
using SimpleDemo.Service;
using SimpleDemo.Interface.IServices;
using SimpleDemo.Service.Implements;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace SimpleWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<Startup>();
            builder.Services.AddTransient<ISimpleServiceProvider, SimpleServiceProvider>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IRepositoryProvider, RepositoryProvider>();
            builder.Services.AddScoped<IUowManager, SimpleDbContext>();
            builder.Services.AddTransient<IEquipmentRep, EquipmentRep>();
            builder.Services.AddTransient<ITestRep, TestRep>();
            builder.Services.AddTransient<TestService>();
            builder.Services.AddTransient<IEquipmentService, EquipmentService>();

            builder.Services.AddControllers(options =>
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true
            );

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SOD Simple Web Api",
                    Version = "v1",
                    Description = "SOD框架Web应用示例"
                });

                // Set the comments path for the Swagger JSON and UI.
                // 设置注释
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseAuthorization();

            app.MapControllers();

            var startup = app.Services.GetService<Startup>();
            if(startup != null)
            {
                startup.Configure(app);
                //startup.InitWork();
                startup.TestDb();
            }
            app.Run();
        }
    }
}
