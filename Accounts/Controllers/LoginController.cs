using System;
using System.Threading.Tasks;
using Accounts.Application.Login;
//using Caliburn.Micro;
using Accounts.Constants;
using Accounts.Enumerations;
using Accounts.Models;
using Accounts.Models.ViewModels;
using Accounts.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Accounts.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILoginCaseUse _loginCaseUse;
        private readonly IMapper _mapper;

        public LoginController(IHttpContextAccessor httpcontextAccessor, ILoginCaseUse loginCaseUse, IMapper mapper)
        {
            //this._context = context;
            var dfdf = httpcontextAccessor.HttpContext.Request.Headers["Accept-Language"].ToString();
            //var dfd = _context.Request.Headers["Accept-Language"].ToString();

            this._loginCaseUse = loginCaseUse;
            this._mapper = mapper;
        }

        //PUBLICO viene del sistema que está tratando de ingresar pero no tiene las credenciales
        [HttpGet]
        //public ActionResult Ingreso(string sistema)

        public ActionResult UserLogin(string sistema)
        {
            LoginViewModel userViewModel = new LoginViewModel { Sistema = sistema };

            string SystemName = "";

            if (sistema == Sistems.EV.ToString())
                SystemName = CGeneral.EV;

            if (sistema == Sistems.NP.ToString())
                SystemName = CGeneral.NP;

            ViewData["SistemaLogin"] = SystemName;

            ViewData["DescLink"] = sistema ?? "";

            return View(userViewModel);
        }


        //PUBLICO  cuando el usuario mete sus credenciales y se va a loguear
        [HttpPost]
        public async Task<ActionResult> UserLogin([Bind] LoginViewModel userView)
        {

            if (userView.Password == "" || userView.Password == null)
            {
                ModelState.AddModelError("", ErrorCodes.GetMessage(ErrorCodes.missingData));
                return View(userView);
            }

            if (ModelState.IsValid)
            {
                CookieUser cooUser = new CookieUser();

                try
                {

                    var userLog = new UserLogin() { User = userView.Email, Password = userView.Password, Program = (Application.Enumerations.ProgramName)1 };
                    var resultLogin = _loginCaseUse.Login(userLog);

                    //bool result = await identity.SignInAsync(user.UserName, user.Password);
                    if (resultLogin != null)
                    {
                            Identity identity = new Identity(this.HttpContext);
                            bool result = await identity.SignInAsync(resultLogin.Data);

                        if (result)
                        {
                            //para acceder a una área específica
                            //  return RedirectToAction("Index", "Home", new { area = "admin" });
                            //  return RedirectToAction("Index", "Home");
                            // return Redirect("http://eventos.buscadme.org");

                            if (resultLogin.Error.Code == ErrorCodes.confirmAccount)
                            {
                                return RedirectToAction("ConfirmAccount", "Login", new
                                {
                                    email = userView.Email,
                                    code = "",
                                    tiempo = 0
                                });

                            }
                            else if (string.Equals(userView.Sistema, Sistems.EV.ToString(), StringComparison.OrdinalIgnoreCase))
                            {
                                return Redirect("http://eventos.buscadme.org/perfiles-asociados");
                            }
                            else
                            {
                                return RedirectToAction("Index", "User");
                            }

                        }
                            else
                            {
                                ModelState.AddModelError(string.Empty, ErrorCodes.GetMessage(ErrorCodes.defaultCode));
                                return View(userView);
                            }

                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodes.GetMessage(ErrorCodes.missingData));
                        return View(userView);
                    }

                }
                catch (Exception ex)
                {

                    //ModelState.AddModelError(string.Empty, ErrorCodes.GetMessage(ex.Message));

                    var resp = UGeneral.Message(ex.Message);
                    ModelState.AddModelError(resp.Item1, resp.Item2);

                    if (resp.Item1 == ErrorCodes.confirmAccount)
                    {
                        ViewData["DescLink"] = "Confirmar Cuenta";
                    }
                    else if (resp.Item1 == ErrorCodes.userBlocked)
                    {
                        ViewData["DescLink"] = "Desbloquear Cuenta";
                    }

                    return View(userView);
                }

            }
            else
            {
                ModelState.AddModelError("", ErrorCodes.GetMessage(ErrorCodes.missingData));
                return View(userView);
            }
        }


     
        [HttpGet]
        public ActionResult GetToken(string pProgram, string pEmail)
        {
            UserEnabledAViewModel userEnabled = new UserEnabledAViewModel();

            userEnabled.System = pProgram;
            userEnabled.Email = pEmail;


            return View(userEnabled);
        }


        public IActionResult RequestToken(string email)
        {
            try {

                var result = _loginCaseUse.RequestToken(email);

                return Json(result);

            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }


        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        [HttpGet]
        public ActionResult ConfirmAccount(string email, string code, int tiempo = 60)
        {
            UserEnabledAViewModel userEnabledAViewModel = new UserEnabledAViewModel();

            userEnabledAViewModel.Email = email;
            userEnabledAViewModel.Code = code;

            ViewData["Tiempo"] = tiempo;

            return View(userEnabledAViewModel);
        } 

        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        [HttpPost("Login/ConfirmAccount")]
        public ActionResult ConfirmAccount(UserEnabledAViewModel userEnabledAView)
        {
            UserEnabledAccount userEnabled = new UserEnabledAccount();

            if (ModelState.IsValid)
            {
                userEnabled = _mapper.Map<UserEnabledAccount>(userEnabledAView);

                var result = _loginCaseUse.EnabledAccount(userEnabled);

                if (result)
                {
                    if(userEnabledAView.System != null && userEnabledAView.System == "EV")
                    //redirige al sistema de eventos
                        return Redirect("http://events.buscadme.net/Profile");
                    else
                        return RedirectToAction("Index", "User");
                }
                else
                {
                    return View(userEnabledAView);
                }

            }
            else
            {
                return View(userEnabledAView);
            }

        }



        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        [HttpGet("Login/ReenviarCodigo")]
        public ActionResult ReenviarCodigo(string email)
        {
            //UserEnabledAccount userEnabled = new UserEnabledAccount();

            Identity identity = new Identity(this.HttpContext);
            CookieUser cookieUser = identity.GetCookie("CookieAuthentication").Result;

            //if (ModelState.IsValid)
            //{
            //userEnabled = _mapper.Map<UserEnabledAccount>(userEnabledAView);

            var result = _loginCaseUse.RequestToken(cookieUser.EmailUser);


            return Json(result);

        }

        
        [HttpGet("Login/ReenviarCodigoForgot")]
        public ActionResult ReenviarCodigoForgot(string email)
        {

            var result = _loginCaseUse.RequestToken(email);

            return Json(result);
        }


        [HttpGet]
        public IActionResult ForgottenPassword(string email)
        {

           return View();
        }

        [HttpPost]
        public IActionResult ForgottenPasswordRecover(string email)
        {
            //dame el contenido de la accion de la vista 

            var result = _loginCaseUse.ForgottenPassword(email);


            return Json (result);
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string email)
        {
            //dame el contenido de la accion de la vista 

            var result = _loginCaseUse.ResetPassword(token, email);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {

            // 1. Limpiar el contexto de autenticación de ASP.NET Core
            await HttpContext.SignOutAsync("CookieAuthentication");

            // 2. Borrado forzado por fecha pasada
            //var cookieOptions = new CookieOptions
            //{
            //    Path = "/",
            //    Expires = DateTime.UtcNow.AddDays(-1), // Fecha pasada
            //    HttpOnly = true,
            //    Secure = true // Asegúrate que coincida con tu configuración global
            //};

            // 2. Borrar explícitamente la cookie del navegador (por seguridad)
            Response.Cookies.Delete("example", new CookieOptions
            {
                Domain = ".buscadme.org",
                Path = "/"
            });

            // 3. Redirigir al login
            return RedirectToAction("UserLogin", "Login");
        }

    }
}
