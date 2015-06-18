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
    [RoutePrefix("questions")]
    public class QuestionsController : BaseAdminController
    {
        [HttpPost]
        [Route("add")]
        public ActionResult AddQuestion(Question question)
        {
            if(ModelState.IsValid)
            {
                question = QuestionRepository.AddQuestion(question);

                var questions = QuestionRepository.GetQuestionsForQuiz(question.QuizId, false);

                return PartialView("_QuestionsTable", questions);
            }

            return View(question);
        }

        [HttpGet]
        [Route("edit/{questionId:long}")]
        public ActionResult EditQuestion(long questionId)
        {
            var question = QuestionRepository.GetQuestion(questionId, false);

            return View(question);
        }

        [HttpPost]
        [Route("edit/{questionId:long}")]
        public ActionResult EditQuestion(Question question)
        {
            if(ModelState.IsValid)
            {
                question = QuestionRepository.UpdateQuestion(question);
            }

            return PartialView("_QuestionForm", question);
        }

        [HttpPost]
        [Route("~/quizzes/{quizId:long}/questions/delete/{questionId:long}")]
        public ActionResult DeleteQuestion(long questionId, long quizId)
        {
            QuestionRepository.DeleteQuestion(questionId);

            var questions = QuestionRepository.GetQuestionsForQuiz(quizId, false);

            return PartialView("_QuestionsTable", questions);
        }
    }
}