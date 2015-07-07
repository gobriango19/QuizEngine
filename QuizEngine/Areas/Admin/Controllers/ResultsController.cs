using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizEngine.Data.Repositories;
using QuizEngine.Models;
using QuizEngine.Data;

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
                using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
                {
                    var resultRepository = new ResultRepository(dbUnitOfWork);
                    result = resultRepository.Add(result);

                    var results = resultRepository.GetResultsForQuiz(result.QuizId, false);
                    return PartialView("_ResultsTable", results);
                }
            }
            return View(result);
        }

        [HttpGet]
        [Route("edit/{resultId:long}")]
        public ActionResult EditResult(long resultId)
        {
            Result result = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                result = resultRepository.Get(resultId, false);
            }
            return View(result);
        }

        [HttpPost]
        [Route("edit/{resultId:long}")]
        public ActionResult EditResult(Result result)
        {
            if(ModelState.IsValid)
            {
                using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
                {
                    var resultRepository = new ResultRepository(dbUnitOfWork);
                    result = resultRepository.Update(result);
                }
            }
            return PartialView("_ResultForm", result);
        }

        [HttpPost]
        [Route("~/quizzes/{quizId:long}/results/delete/{resultId:long}")]
        public ActionResult DeleteResult(long resultId, long quizId)
        {
            IEnumerable<Result> results = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                resultRepository.Delete(resultId);
                results = resultRepository.GetResultsForQuiz(quizId, false);
            }
            return PartialView("_ResultsTable", results);
        }
    }
}