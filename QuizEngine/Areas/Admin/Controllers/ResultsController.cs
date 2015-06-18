using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizEngine.Data.Repositories;
using QuizEngine.Models;

namespace QuizEngine.Areas.Admin.Controllers
{
    [RouteArea("admin")]
    [RoutePrefix("results")]
    public class ResultsController : BaseAdminController
    {
        [HttpPost]
        [Route("add")]
        public ActionResult AddResult(Result result)
        {
            if(ModelState.IsValid)
            {
                result = ResultRepository.AddResult(result);

                var results = ResultRepository.GetResultsForQuiz(result.QuizId, false);
                return PartialView("_ResultsTable", results);
            }

            return View(result);
        }

        [HttpGet]
        [Route("edit/{resultId:long}")]
        public ActionResult EditResult(long resultId)
        {
            var result = ResultRepository.GetResult(resultId, false);

            return View(result);
        }

        [HttpPost]
        [Route("edit/{resultId:long}")]
        public ActionResult EditResult(Result result)
        {
            if(ModelState.IsValid)
            {
                result = ResultRepository.UpdateResult(result);
            }

            return PartialView("_ResultForm", result);
        }

        [HttpPost]
        [Route("~/quizzes/{quizId:long}/results/delete/{resultId:long}")]
        public ActionResult DeleteResult(long resultId, long quizId)
        {
            ResultRepository.DeleteResult(resultId);

            var results = ResultRepository.GetResultsForQuiz(quizId, false);

            return PartialView("_ResultsTable", results);
        }
    }
}