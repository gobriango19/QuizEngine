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
    [RoutePrefix("quizzes")]
    public class QuizzesController : BaseAdminController
    {
        [HttpGet]
        [Route("~/admin")]
        [Route("")]
        public ActionResult Index()
        {
            var quizzes = QuizRepository.GetAllQuizzes(false);

            return View(quizzes);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult AddQuiz(Quiz quiz)
        {
            if(ModelState.IsValid)
            {
                quiz = QuizRepository.AddQuiz(quiz);
            }

            var quizzes = QuizRepository.GetAllQuizzes(false);

            return PartialView("_QuizzesTable", quizzes);
        }

        [HttpGet]
        [Route("edit/{quizId:long}")]
        public ActionResult EditQuiz(long quizId)
        {
            var quiz = QuizRepository.GetQuiz(quizId, false);

            var questions = QuestionRepository.GetQuestionsForQuiz(quizId, false);
            quiz.Questions = questions;

            var results = ResultRepository.GetResultsForQuiz(quizId, false);
            quiz.Results = results;

            return View(quiz);
        }

        [HttpPost]
        [Route("edit/{quizId:long}")]
        public ActionResult EditQuiz(Quiz quiz)
        {
            if(ModelState.IsValid)
            {
                quiz = QuizRepository.UpdateQuiz(quiz);
            }

            var questions = QuestionRepository.GetQuestionsForQuiz(quiz.QuizId, false);
            quiz.Questions = questions;

            var results = ResultRepository.GetResultsForQuiz(quiz.QuizId, false);
            quiz.Results = results;

            return PartialView("_QuizForm", quiz);
        }

        [HttpPost]
        [Route("delete/{quizId:long}")]
        public ActionResult DeleteQuiz(long quizId)
        {
            QuizRepository.DeleteQuiz(quizId);

            var quizzes = QuizRepository.GetAllQuizzes(false);

            return PartialView("_QuizzesTable", quizzes);
        }
    }
}