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
    [RoutePrefix("questions")]
    public class QuestionsController : BaseAdminController
    {
        [HttpPost]
        [Route("add")]
        public ActionResult AddQuestion(Question question)
        {
            if(ModelState.IsValid)
            {
                IEnumerable<Question> questions = null;
                using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
                {
                    var questionRepository = new QuestionRepository(dbUnitOfWork);
                    question = questionRepository.Add(question);
                    questions = questionRepository.GetQuestionsForQuiz(question.QuizId, false);
                }
                return PartialView("_QuestionsTable", questions);
            }
            return View(question);
        }

        [HttpGet]
        [Route("edit/{questionId:long}")]
        public ActionResult EditQuestion(long questionId)
        {
            Question question = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var questionRepository = new QuestionRepository(dbUnitOfWork);
                question = questionRepository.Get(questionId, false);
            }
            return View(question);
        }

        [HttpPost]
        [Route("edit/{questionId:long}")]
        public ActionResult EditQuestion(Question question)
        {
            if(ModelState.IsValid)
            {
                using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
                {
                    var questionRepository = new QuestionRepository(dbUnitOfWork);
                    question = questionRepository.Update(question);
                }
            }
            return PartialView("_QuestionForm", question);
        }

        [HttpPost]
        [Route("~/quizzes/{quizId:long}/questions/delete/{questionId:long}")]
        public ActionResult DeleteQuestion(long questionId, long quizId)
        {
            IEnumerable<Question> questions = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var questionRepository = new QuestionRepository(dbUnitOfWork);
                questionRepository.Delete(questionId);
                questions = questionRepository.GetQuestionsForQuiz(quizId, false);
            }
            return PartialView("_QuestionsTable", questions);
        }
    }
}