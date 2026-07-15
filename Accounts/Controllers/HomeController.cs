using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Reflection;
using Accounts.Utilities;
using Accounts.Application.User;
using Accounts.Models.ViewModels;

namespace Accounts.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataProtector _dataProtector;
     

        public HomeController(ILogger<HomeController> logger, IDataProtectionProvider dataProte)
        {
            var protectorPurpose = "Para usar el contenido";
            _logger = logger;
            _dataProtector = dataProte.CreateProtector(protectorPurpose);
        }

        public IActionResult Index()
       
        {
            return Redirect($"/User/Index");
        }

        [Authorize]
        public async Task<ActionResult> Users()
        {
            Identity identity = new Identity(this.HttpContext);
            var cookies = await identity.GetCookie("CookieAuthentication");


            if (cookies == null || cookies.Id == 0)
            {
                return Redirect($"/Login/UserLogin");
            }


            var uses = new UserViewModel();
            return View(uses.GetUsers());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
