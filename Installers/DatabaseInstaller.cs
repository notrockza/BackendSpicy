using BackendSpicy.Models;
using Microsoft.EntityFrameworkCore;

namespace BackendSpicy.Installers
{
    public class DatabaseInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            //------------------------ 
            // builder.Configuration.GetConnectionString เอาไว้อ่านค่าใน appsettings
            // Services มาจาก WebApplicationBuilder
            //var connectionString = builder.Configuration.GetConnectionString("DatabaseContext");
            //builder.Services.AddDbContext<DatabaseContext>(options =>
            //    options.UseSqlServer(connectionString)
            //);
            //------------------------

                //custom DI

            var connectionString = builder.Configuration.GetConnectionString("DatabaseContext");
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString)
            );

        }
    }
}
