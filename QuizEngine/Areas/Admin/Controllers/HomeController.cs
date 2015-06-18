using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizEngine.Areas.Admin.Controllers
{
    public class HomeController : BaseAdminController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Index", new { area = "Admin", controller = "Quizzes" });
        }
    }
}