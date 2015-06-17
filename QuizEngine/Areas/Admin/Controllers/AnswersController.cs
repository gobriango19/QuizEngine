using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizEngine.Data;
using QuizEngine.Data.Repositories;
using QuizEngine.Models;

namespace QuizEngine.Areas.Admin.Controllers
{
    [RouteArea("admin")]
    [RoutePrefix("answers")]
    public class AnswersController : Controller
    {
        [HttpPost]
        [Route("add")]
        public ActionResult AddAnswer(Answer answer)
        {
            if(ModelState.IsValid)
            {
                answer = AnswerRepository.AddAnswer(answer);

                var answers = AnswerRepository.GetAnswersForQuestion(answer.QuestionId, false);
                
                return PartialView("_AnswersTable", answers);
            }

            return View(answer);
        }

        [HttpGet]
        [Route("edit/{answerId:long}")]
        public ActionResult EditAnswer(long answerId)
        {
            Answer answer = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                answer = AnswerRepository.GetAnswer(answerId, false);
                // explicit load question for navigation
                dbUnitOfWork.DbContext.Entry(answer).Reference(a => a.Question).Load();
            }

            return View(answer);
        }

        [HttpPost]
        [Route("edit/{answerId:long}")]
        public ActionResult EditAnswer(Answer answer)
        {
            if(ModelState.IsValid)
            {
                answer = AnswerRepository.UpdateAnswer(answer);
            }

            return PartialView("_AnswerForm", answer);
        }

        [HttpPost]
        [Route("~/questions/{questionId:long}/delete/{answerId:long}")]
        public ActionResult DeleteAnswer(long answerId, long questionId)
        {
            AnswerRepository.DeleteAnswer(answerId);

            var answers = AnswerRepository.GetAnswersForQuestion(questionId, false);

            return PartialView("_AnswersTable", answers);
        }
    }
}