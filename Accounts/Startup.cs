using System;
using System.Collections.Generic;
using System.IO;
using Accounts.Application.CaseUses;
using Accounts.Application.Login;
using Accounts.Application.Login.Interfaces;
using Accounts.Application.User;
using Accounts.Application.User.Interfaces;
using Accounts.Controllers;
using Accounts.Infrastructure.Login;
using Accounts.Infrastructure.User;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace Accounts
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IUserCaseUse, UserCaseUse>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<ILoginCaseUse, LoginCaseUse>();
            services.AddScoped<ILoginRepository, LoginRepository>();


            //SOLO PRUEBA
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDataProtection()
           .PersistKeysToFileSystem(new DirectoryInfo(@"C:\KeyRing"))
           .SetApplicationName("buscadme");
            // .ProtectKeysWithCertificate("thumbprint");


            services.AddAuthentication(options =>
            {
                // CAMBIO CLAVE: Usamos JWT como esquema predeterminado para las llamadas internas,
                // pero la autenticaci�n primaria del frontend ser� la Cookie.
                options.DefaultScheme = "ContextJwtAuthentication";
                options.DefaultChallengeScheme = "CookieAuthentication";
                options.DefaultSignInScheme = "CookieAuthentication";
            })

          // 1. Configuraci�n del SSO (Cookie de Identidad Global)
          // El sistema la lee para saber qui�n es el usuario.
          .AddCookie("CookieAuthentication", config =>
          {
              config.Cookie.Name = "nombrecookie";
              config.Cookie.Domain = ".dominio.org"; // Sigue siendo el dominio ra�z para SSO

              config.LoginPath = "/Login/UserLogin";

              ////**************************************nuevas configuraciones **********************************************
              config.Cookie.HttpOnly = true;        // Protecci�n contra XSS
              // config.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS obligatorio
              config.Cookie.SameSite = SameSiteMode.Lax; // Protecci�n contra CSRF


              // Define el comportamiento de expiraci�n aqu� tambi�n, si pasa mas de la mitad del tiempo de vida
              //de la cookie emite una nueva cookie
              config.SlidingExpiration = true;
              // Opcional: define la ruta ra�z para que la cookie sea v�lida en todo el sitio
              config.Cookie.Path = "/";
              ////**************************************nuevas configuraciones **********************************************

              // config.AccessDeniedPath = "/error/AccessDenied";
          });

            services.AddAntiforgery(options =>
            {
                // 1. Obligatorio: Debe tener el mismo dominio ra�z que tu SSO
                options.Cookie.Domain = ".buscadme.org";

                // 2. Recomendado: El mismo nombre base o personalizado para evitar colisiones
                options.Cookie.Name = "antiforgery_example";

                // 3. Emparejar el comportamiento con tu cookie de autenticaci�n
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.Path = "/";

                // Si en producci�n usas HTTPS, descomenta esto tambi�n en la autenticaci�n:
                // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; 
            });


            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
              //  app.UseExceptionHandler("/Home/Error");

                app.UseExceptionHandler("/Error/General");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.

                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // �qui�n eres t�?  
            app.UseAuthentication();

            // �te permiten?  
            app.UseAuthorization();


            //RESPALDO FUNCIONA CORRECTAMENTE
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

        }

        private DirectoryInfo GetKeyRingDirInfo()
        {
            var startupAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            var applicationBasePath = System.AppContext.BaseDirectory;
            var directoryInfo = new DirectoryInfo(applicationBasePath);
            do
            {
                directoryInfo = directoryInfo.Parent;

                var keyRingDirectoryInfo = new DirectoryInfo(Path.Combine(directoryInfo.FullName, "KeyRing"));
                if (keyRingDirectoryInfo.Exists)
                {
                    return keyRingDirectoryInfo;
                }
            }
            while (directoryInfo.Parent != null);

            throw new Exception($"KeyRing folder could not be located using the application root {applicationBasePath}.");
        }

    }
}
