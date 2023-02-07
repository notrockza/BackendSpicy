namespace BackendSpicy.Installers
{
    public class ControlInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
        }

    }
}
