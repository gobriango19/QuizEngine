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
    public class AnswersController : BaseAdminController
    {
        [HttpPost]
        [Route("add")]
        public ActionResult AddAnswer(Answer answer)
        {
            if(ModelState.IsValid)
            {
                using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
                {
                    var answerRepository = new AnswerRepository(dbUnitOfWork);
                    answer = answerRepository.Add(answer);
                    var answers = answerRepository.GetAnswersForQuestion(answer.QuestionId, false);
                    return PartialView("_AnswersTable", answers);
                }
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
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answer = answerRepository.Get(answerId, false);
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
                using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
                {
                    var answerRepository = new AnswerRepository(dbUnitOfWork);
                    answer = answerRepository.Update(answer);
                }
            }
            return PartialView("_AnswerForm", answer);
        }

        [HttpPost]
        [Route("~/questions/{questionId:long}/delete/{answerId:long}")]
        public ActionResult DeleteAnswer(long answerId, long questionId)
        {
            IEnumerable<Answer> answers = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answerRepository.Delete(answerId);
                answers = answerRepository.GetAnswersForQuestion(questionId, false);
            }
            return PartialView("_AnswersTable", answers);
        }
    }
}