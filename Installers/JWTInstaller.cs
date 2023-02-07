using BackendSpicy.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BackendSpicy.Installers

{
    public class JWTInstaller : IInstallers
    {
        public void InstallServices(WebApplicationBuilder builder)
        {
            //web jwt bearer core 6
            //อธิบาย https://devahoy.com/blog/2016/07/understanding-token-and-jwt-create-authentication-with-hapijs/
            //JWT https://www.freecodespot.com/blog/jwt-authentication-in-dotnet-core/
            var jwtSetting = new JwtSetting();
            //builder.Configuration.Bind(nameof(jwtSetting), jwtSetting);
            builder.Services.AddSingleton(jwtSetting);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddJwtBearer(options =>
                  {
                      options.TokenValidationParameters =
                          new TokenValidationParameters
                          {
                              ValidateIssuer = false,
                              ValidIssuer = "notrockza",
                              ValidateAudience = true,
                              ValidAudience = jwtSetting.Audience,
                              ValidateLifetime = true,
                              ValidateIssuerSigningKey = true,


                              ValidateActor = false,

                              IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.Key)),
                              ClockSkew = TimeSpan.Zero //ละยะเวลา
                         };
                  });
        }
    }
}
