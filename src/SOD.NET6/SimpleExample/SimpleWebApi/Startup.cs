using Microsoft.AspNetCore.Builder;
using PWMIS.DataProvider.Data;
using SimpleDemo.Service;

namespace SimpleWebApi
{
    public class Startup
    {
        IConfiguration _configuration;
        IServiceProvider _provider;

        public void Configure(IApplicationBuilder app)
        {
            _provider = app.ApplicationServices;
            _configuration = _provider.GetRequiredService<IConfiguration>();

            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        }

        public void TestDb()
        {
            AdoHelper sqlHelper = AdoHelper.CreateHelper("SimpleDB");
            Console.WriteLine(sqlHelper.ConnectionString);
        }

        public void InitWork()
        {
            using (var scope = _provider.CreateScope())
            {
                var testSrv = scope.ServiceProvider.GetService<TestService>();
                testSrv?.TestUow();
            }

        }
    }
}
