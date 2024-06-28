using SimpleDemo.Interface.Infrastructure;
using SimpleDemo.Interface.IRepositories;
using SimpleDemo.Repository.Implements;
using SimpleDemo.Repository;
using SimpleDemo.Service;

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

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            var startup = app.Services.GetService<Startup>();
            if(startup != null)
            {
                startup.Configure(app);
                startup.InitWork();
                startup.TestDb();
            }
            app.Run();
        }
    }
}
