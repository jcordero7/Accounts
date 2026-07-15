using Accounts.Application.User;
using Accounts.Application.Enumerations;
using Accounts.Constants;
using Accounts.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Accounts.Application.Login;

namespace Accounts.Utilities
{
    public class Identity
    {
        #region properties
        public Exception LastError { get; private set; }
        #endregion

        #region constructor
        private HttpContext Context;

        public Identity(HttpContext context)
        {
            this.Context = context;
          
        }
        #endregion constructor


        #region login actions

        ////ORIGINAL
        //public async Task<bool> SignInAsync(string token)
        //{
        //    bool result = false;

        //    //try
        //    //{
         
        //        CookieUser cooUser = new CookieUser();
        //        var handler = new JwtSecurityTokenHandler();
        //        var tokenDec = handler.ReadJwtToken(token);

        //        var fdf = tokenDec.Claims.ToList();

        //        cooUser.Id = int.Parse(tokenDec.Claims.Where(x => x.Type == "Id").First().Value);
        //        cooUser.EmailUser = tokenDec.Claims.Where(x => x.Type == "User").First().Value;
        //    // cooUser.Name = tokenDec.Claims.Where(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").First().Value;


        //    // OBTENER EL CLAIM DE ROL (usa la clave corta "role" que generaste)

        //    ////var roleClaim = tokenDec.Claims.FirstOrDefault(x => x.Type == "role");
            
        //    // Si usaste ClaimTypes.Role, usa: tokenDec.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

        //    var roleClaim = tokenDec.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

        //     List<Claim> lstRole = tokenDec.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();


        //    cooUser.Token = token;

        //        //await this.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,

        //        await this.Context.SignInAsync("CookieAuthentication",
        //        new ClaimsPrincipal(SetUserData(cooUser, lstRole)),
        //        new AuthenticationProperties
        //        {
        //               //validar si la cookie debe persistir
        //               //original
        //               //IsPersistent = false,

        //               //en true la cookie persiste aun despues de que se cierra el navegador
        //               IsPersistent = true,
        //               //ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        //               ExpiresUtc = DateTime.UtcNow.AddDays(60)
        //        });

        //        result = true;


        //    //}
        //    //else
        //    //{
        //    //    //if (user.Blocked) this.LastError = new Exception("This user login are blocked. Please contact us. " + user.LastError);
        //    //    //else this.LastError = new Exception("User name or password is not valid. " + user.LastError);
        //    //    result = false;
        //    //}

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    this.LastError = ex;
        //    //    result = false;
        //    //}

        //    return result;
        //}


        //public async Task<bool> SignInAsync2(string token)
        //{
        //    bool result = false;

        //    CookieUser cooUser = new CookieUser();
        //    var handler = new JwtSecurityTokenHandler();
        //    var tokenDec = handler.ReadJwtToken(token);

        //    cooUser.Id = int.Parse(tokenDec.Claims.Where(x => x.Type == "Id").First().Value);
        //    cooUser.EmailUser = tokenDec.Claims.Where(x => x.Type == "User").First().Value;

        //    //BASICAMENTE SOLO TRAE UN ROL GENERAL QUE ES PARA QUE EN EL SISTEMA DE EVENTOS TENGA AUTORIZACION Y CONSULTAR LOS ROLES ESPECIFICOS DEL USUARIO
        //    List<Claim> lstRole = tokenDec.Claims.Where(x => x.Type == ClaimTypes.Role).ToList();

        //    cooUser.Token = token;

        //    await this.Context.SignInAsync("CookieAuthentication",
        //    new ClaimsPrincipal(SetUserData(cooUser, lstRole)),
        //    new AuthenticationProperties
        //    {
        //        //en true la cookie persiste aun despues de que se cierra el navegador
        //        IsPersistent = true,
        //        //ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        //        ExpiresUtc = DateTime.UtcNow.AddDays(60)
        //    });

        //    result = true;

        //    return result;
        //}

        public async Task<bool> SignInAsync(string token)
        {
            if (string.IsNullOrEmpty(token) || !new JwtSecurityTokenHandler().CanReadToken(token))
                return false;

            var tokenDecoded = new JwtSecurityTokenHandler().ReadJwtToken(token);

            var claims = new List<Claim>();
            var jwtClaims = tokenDecoded.Claims;

            // Extraer Claims Esenciales (ID, Email)
            claims.Add(new Claim(ClaimTypes.NameIdentifier, jwtClaims.FirstOrDefault(c => c.Type == "Id")?.Value ?? ""));
            claims.Add(new Claim(ClaimTypes.Email, jwtClaims.FirstOrDefault(c => c.Type == "User")?.Value ?? ""));

            // Incluir Rol General
            claims.AddRange(jwtClaims.Where(x => x.Type == ClaimTypes.Role));

            // 💡 PASO CRUCIAL: Guardar la cadena JWT completa como un Claim
            claims.Add(new Claim("AuthJwtBearer", token));

            // Crear el Principal
            var appIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
            var principal = new ClaimsPrincipal(appIdentity);

            // Iniciar Sesión (Crear la Cookie de Sesión Encriptada)
            await this.Context.SignInAsync("CookieAuthentication",
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                   // AllowRefresh = true,
                    
                    ExpiresUtc = DateTime.UtcNow.AddDays(60)
                });

            return true;
        }


        //private ClaimsIdentity SetUserData(CookieUser user, string roleName = null)

        private ClaimsIdentity SetUserData(CookieUser user, List<Claim> lstRole)
        {
            List<Claim> claims = new List<Claim>();
            //use reflexion to get dynamic the properties about the user object
            PropertyInfo[] properties = typeof(CookieUser).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                //get property name
                string myType = property.Name;
                //get value of the property
                string myValue = property.GetValue(user).ToString();
                claims.Add(new Claim(myType, myValue));
            }


            lstRole.ForEach(x =>
            {
                claims.Add(new Claim(ClaimTypes.Role, x.Value));
            });


            //claims.Add(new Claim(ClaimTypes.Role, roleName));


            claims.Add(new Claim("Accion", "CP"));

            //CP - (crear perfil)
            //CU - (Crear usuario)

            // ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
            return claimsIdentity;
        }


        //private ClaimsIdentity SetUserData(CookieUser user)
        //{
        //    List<Claim> claims = new List<Claim>();
        //    //use reflexion to get dynamic the properties about the user object
        //    PropertyInfo[] properties = typeof(CookieUser).GetProperties();
        //    foreach (PropertyInfo property in properties)
        //    {
        //        //get property name
        //        string myType = property.Name;
        //        //get value of the property
        //        string myValue = property.GetValue(user).ToString();
        //        claims.Add(new Claim(myType, myValue));
        //    }

        //    claims.Add(new Claim("Accion", "CP"));

        //    //CP - (crear perfil)
        //    //CU - (Crear usuario)

        //    // ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "CookieAuthentication");
        //    return claimsIdentity;
        //}


        /// <summary>
        /// ORIGINAL
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        //public async Task<bool> SignInAsync(string username, string password)
        //{
        //    bool result;
        //    try
        //    {
        //       // CookieUser user = new CookieUser(username, password, this.Context);
        //        CookieUser user = new CookieUser(username, password);
        //        if (user.Id > 0)
        //        {
        //            //await this.Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
        //                 await this.Context.SignInAsync("CookieAuthentication",

        //                new ClaimsPrincipal(SetUserData(user)),
        //                new AuthenticationProperties
        //                {
        //                    IsPersistent = false,
        //                    ExpiresUtc = DateTime.UtcNow.AddMinutes(60)
        //                });
        //            result = true;
        //        }
        //        else
        //        {
        //            //if (user.Blocked) this.LastError = new Exception("This user login are blocked. Please contact us. " + user.LastError);
        //            //else this.LastError = new Exception("User name or password is not valid. " + user.LastError);
        //            result = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.LastError = ex;
        //        result = false;
        //    }
        //    return result;
        //}


        public async Task SignOutAsync()
        {
            // await this.Context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await this.Context.SignOutAsync("CookieAuthentication");
        }
        #endregion

        #region async calls
        /// 

        /// Get the claims about the active user and return the user model
        /// 
        /// 
        public async Task<CookieUser> GetCookieold()
        {
            // AuthenticateResult x = await this.Context.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticateResult x = await this.Context.AuthenticateAsync("CookieAuthentication");

            //original
            // CookieUser user = new CookieUser(this.Context);
            CookieUser user = new CookieUser();

            if (x.Succeeded && x.Principal != null)
            {
                //use reflexion to fill the object
                //Type t = Assembly.GetExecutingAssembly().GetType("Accounts.Models.CookieUser");

                // Es mejor usar typeof(CookieUser) que buscarlo por string para evitar errores de dedo
                Type t = typeof(CookieUser);

                foreach (Claim item in x.Principal.Claims)
                {
                    PropertyInfo property = t.GetProperty(item.Type);


                    // VALIDACIÓN CRUCIAL: Solo si la propiedad existe y se puede escribir
                    if (property != null && property.CanWrite)
                    {
                        try
                        {
                            Type tipo = property.PropertyType;
                            // Convertimos el valor del claim al tipo de la propiedad
                            var convertedValue = Convert.ChangeType(item.Value, tipo);
                            property.SetValue(user, convertedValue);
                        }
                        catch (Exception)
                        {
                            // Opcional: Manejar errores de conversión de tipos
                        }
                    }





                    //convert to the correct property type
                   // Type tipo = property.PropertyType;
                   // property.SetValue(user, Convert.ChangeType(item.Value, tipo));
                }
            }

            return user;
        }


        public async Task<CookieUser> GetCookie(string schemeName)
        {
            CookieUser user = new CookieUser();
            AuthenticateResult key = await this.Context.AuthenticateAsync(schemeName);

            if (key.Succeeded && key.Principal != null)
            {
                Type t = typeof(CookieUser);

                foreach (Claim item in key.Principal.Claims)
                {
                    // --- MAPEO DE CLAVES ---
                    string propertyName = item.Type switch
                    {
                        // 1. Mapeo del ID
                        ClaimTypes.NameIdentifier => "Id",
                        "Id" => "Id",

                        // 2. Mapeo del Email
                        ClaimTypes.Email => "EmailUser",
                        "User" => "EmailUser",

                        // 3. Mapeo del ROL (URL Larga de Microsoft)
                        //  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role" => "RoleName",
                        ClaimTypes.Role => "RoleName",
                        "role" => "RoleName",

                        // 4. Mapeo del token (Nombre exacto que ves en tu lista)
                        "AuthJwtBearer" => "Token",
                        "Token" => "Token",

                        // 5. Mapeo del id profile (Nombre exacto que ves en tu lista)
                        "Profile" => "Profile",
                        // "Profile" => "Profile",

                        _ => item.Type
                    };

                    PropertyInfo property = t.GetProperty(propertyName);

                    if (property != null)
                    {
                        try
                        {
                            Type tipo = property.PropertyType;
                            // Convertimos el string del claim al tipo de la propiedad (int, string, etc.)
                            var value = Convert.ChangeType(item.Value, tipo);
                            property.SetValue(user, value);
                        }
                        catch
                        {
                            // Si falla la conversión (ej. un null), no rompe el flujo
                        }
                    }
                }
            }
            return user;
        }






        #endregion async calls


        #region User Data
        /// 
        /// Get the user name
        /// 
        /// 
        //public string GetUserName()
        //{
        //    return this.GetUserAsync().Result.Name;
        //}


        /// Get the register date
        /// 
        /// 
        //public DateTime GetRegisterDate()
        //{
        //    return this.GetUserAsync().Result.RegisterIn;
        //}


        /// Get the user id
        /// 
        /// 
        public int GetId()
        {
            return this.GetCookie("CookieAuthentication").Result.Id;
        }

        #endregion User Data
    }

  

    public enum ProgramName
    {
        Events = 1,
        Naturopaty = 2
    }

}
