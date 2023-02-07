using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;

namespace BackendSpicy.Installers
{
    public class ServiceInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {  
            //builder.Services.AddTransient<IProductSercice, ProductService>();
            //ใช้ AutoRefac ลงทะเบียนโดยอัตโนมัติกรณีมีหลายๆ Service
            //----------------------- จะได้ไม่ต้องลงทะเบียนหลายอัน ---------------------------
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(containerBuilder =>
            {
                containerBuilder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                //- จะค้นหาชื่อไฟล์ที่ลงท้ายด้วย Service แล้วจะ DI โดยอัตโนมัติ
                .Where(t => t.Name.EndsWith("Service") || t.Name.EndsWith("Test"))
                .AsImplementedInterfaces();

            }));
            //-------------------------------------------------------------------
        }
    }
}
