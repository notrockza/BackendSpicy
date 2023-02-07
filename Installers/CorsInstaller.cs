namespace BackendSpicy.Installers
{
    public class CorsInstaller : IInstallers
    {

        public static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public static string MyAllowAnyOrigins = "_myAllowAnyOrigins";
        public void InstallServices(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("https://www.w3schools.com",
                                                          "http://www.contoso.com")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod();
                                  });

                options.AddPolicy(name: MyAllowAnyOrigins,
                                 policy =>
                                 {
                                     policy.AllowAnyOrigin()
                                     .AllowAnyHeader()
                                     .AllowAnyMethod();
                                 });
            });
        }
    }
}
