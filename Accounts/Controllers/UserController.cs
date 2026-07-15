using System;
using System.Threading.Tasks;
using Accounts.Application.Dtos;
using Accounts.Application.User;
using Accounts.Constants;
using Accounts.Enumerations;
using Accounts.Models;
using Accounts.Models.ViewModels;
using Accounts.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Accounts.Controllers
{
    public class UserController : Controller
    {

        private readonly IUserCaseUse _userCaseUse;
        private readonly IMapper _mapper;

        public UserController(IUserCaseUse userCaseUse, IMapper mapper)
        {
            this._userCaseUse = userCaseUse;
            this._mapper = mapper;
        }


        // GET: UserController
        [Authorize(AuthenticationSchemes = "CookieAuthentication")]
        public ActionResult Index()
        
        {
            Identity identity = new Identity(this.HttpContext);
            CookieUser cookieUser = identity.GetCookie("CookieAuthentication").Result;

            if (cookieUser == null || cookieUser.Id == 0)
            {

                return Redirect($"/Login/UserLogin");
            }

            var user = _userCaseUse.Get(cookieUser.Id, cookieUser.Token);

            
            
            if (user.Error.Code == ErrorCodes.confirmAccount)
            {
                return RedirectToAction("ConfirmAccount", "Login", new
                {
                    email = cookieUser.EmailUser,
                    code = "",
                    tiempo = 0
                });

            }

            var userView = _mapper.Map<UserViewModel>(user.Data);

            return View(userView);
        }

        // GET: UserController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        // GET: UserController/Create
        public ActionResult Create(string sistema)
        {
            string SystemName = "";
            int SystemId = 0;

            if (sistema == Sistems.EV.ToString())
            {
                SystemName = CGeneral.EV;
                SystemId = (int)Sistems.EV;
            }

            if (sistema == Sistems.NP.ToString())
            {
                SystemName = CGeneral.NP;
                SystemId = (int)Sistems.NP;
            }

            ViewData["SystemName"] = SystemName;
            ViewData["SystemId"] = SystemId;

            return View();
        }


        // PUBLICO: un usuario nuevo aún no tiene sesión al enviar este formulario,
        // por eso esta acción no puede requerir [Authorize] (el sign-in ocurre dentro de SaveUser).
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel userView)
        {
            try
            {
                var result = SaveUser(userView);
                if (result.Result)
                {
                    UserEnabledAViewModel userEnabled = new UserEnabledAViewModel();
                    userEnabled.Email = userView.Email;

                    //funciona
                    return RedirectToAction("ConfirmAccount", "Login", new
                    {
                        email = userEnabled.Email,
                        code = userEnabled.Code
                    });
                  
                }
                else
                {
                    ModelState.AddModelError(string.Empty, ErrorCodes.GetMessage(ErrorCodes.defaultCode));
                    return View(userView);
                }
            }
            catch(Exception ex)
            {
                return View(userView);
            }
        }

        private async Task<bool> SaveUser(UserViewModel userView)
        {
            bool band = false;
            CookieUser cooUser = new CookieUser();
            var user = _mapper.Map<UserDto>(userView);

            var respuesta = _userCaseUse.Add(user, cooUser.Token);

            if (respuesta.Data != null)
            {
                Identity identity = new Identity(this.HttpContext);
                bool result = await identity.SignInAsync(respuesta.Data.Token);
                if (result)
                {
                    band = true;
                }
                else
                {
                    band = false;
                }
            }
            else
            {
                band = false;
            }
            return band;
        }


        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            Identity identity = new Identity(this.HttpContext);
            CookieUser cookieUser = identity.GetCookie("CookieAuthentication").Result;

            if (cookieUser == null || cookieUser.Id == 0)
            {
                //ver donde se manda
               // var jsondData = new { eventDto };
              //  return Json(jsondData);
            }

            var user = _userCaseUse.Get(id, cookieUser.Token);
            var userDto = _mapper.Map<UserViewModel>(user);

            return View(userDto);
        }

        // YA FUNCIONA ESTO SE DEBE MOSTRAR ASI
        [HttpPost]
        [Authorize(AuthenticationSchemes = "CookieAuthentication")] 
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel userView)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Identity identity = new Identity(this.HttpContext);
                    CookieUser cookieUser = identity.GetCookie("CookieAuthentication").Result;

                    var user = _mapper.Map<UserDto>(userView);
                    var result = _userCaseUse.Edit(user, cookieUser.Token);

                    // return RedirectToAction("Index");
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    return View(userView);
                }
               
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        [HttpPost]
        public ActionResult CambiarPassword(ChangePasswordViewModel cambioPassword)
        {

            Identity identity = new Identity(this.HttpContext);
            CookieUser cookieUser = identity.GetCookie("CookieAuthentication").Result;

            cambioPassword.Id = cookieUser.Id;
            var changePasswordDto = _mapper.Map<ChangePasswordDto>(cambioPassword);

            var result = _userCaseUse.ChangePassword(changePasswordDto, cookieUser.Token);

           // return Json(lstCategory.Select(c => new { c.Id, c.Name }));
            return Ok(new { success = result.Data, message = "" });

        }
    }


}

//para que los datos que vienen de la vista se reciban bien los nombres de los campos de ChangePasswordViewModel
//tienen que llamarse igual a los que envia js