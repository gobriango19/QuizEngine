using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace QuizEngine.Areas.Admin.Controllers
{
    [RouteArea("admin")]
    [RoutePrefix("auth")]
    public class AuthController : Controller
    {
        [HttpGet]
        [Route("")]
        [Route("login")]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult LogIn(string userName, string password, string returnUrl)
        {
            var adminUserName = ConfigurationManager.AppSettings["AdminLoginUserName"];
            var adminPassword = ConfigurationManager.AppSettings["AdminLoginPassword"];

            if (userName == adminUserName && password == adminPassword)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                return RedirectToAction("Index", new { area = "Admin", controller = "Quizzes" });
            }

            ViewBag.LogInFailed = true;
            return View("Index");
            //return RedirectToAction("Index", new { area = "Admin", controller = "Auth" });
        }

        [HttpGet]
        [Route("signout")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", new { area = "Admin", controller = "Auth" });
        }
    }
}