using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuizEngine.Data.Repositories;
using QuizEngine.Data;
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
            IEnumerable<Quiz> quizzes = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quizzes = quizRepository.GetAllQuizzes(false);
            }
            return View(quizzes);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult AddQuiz(Quiz quiz)
        {
            IEnumerable<Quiz> quizzes = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);

                if (ModelState.IsValid)
                {
                    quiz = quizRepository.Add(quiz);
                }

                quizzes = quizRepository.GetAllQuizzes(false);
            }
            return PartialView("_QuizzesTable", quizzes);
        }

        [HttpGet]
        [Route("edit/{quizId:long}")]
        public ActionResult EditQuiz(long quizId)
        {
            Quiz quiz = null;
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quiz = quizRepository.Get(quizId, false);

                var questionRepository = new QuestionRepository(dbUnitOfWork);
                var questions = questionRepository.GetQuestionsForQuiz(quizId, false);
                quiz.Questions = new List<Question>(questions);

                var resultRepository = new ResultRepository(dbUnitOfWork);
                var results = resultRepository.GetResultsForQuiz(quizId, false);
                quiz.Results = new List<Result>(results);
            }
            return View(quiz);
        }

        [HttpPost]
        [Route("edit/{quizId:long}")]
        public ActionResult EditQuiz(Quiz quiz)
        {
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                if (ModelState.IsValid)
                {
                    var quizRepository = new QuizRepository(dbUnitOfWork);
                    quiz = quizRepository.Update(quiz);
                }

                var questionRepository = new QuestionRepository(dbUnitOfWork);
                var questions = questionRepository.GetQuestionsForQuiz(quiz.QuizId, false);
                quiz.Questions = new List<Question>(questions);

                var resultRepository = new ResultRepository(dbUnitOfWork);
                var results = resultRepository.GetResultsForQuiz(quiz.QuizId, false);
                quiz.Results = new List<Result>(results);
            }
            return PartialView("_QuizForm", quiz);
        }

        [HttpPost]
        [Route("delete/{quizId:long}")]
        public ActionResult DeleteQuiz(long quizId)
        {
            IEnumerable<Quiz> quizzes = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quizRepository.Delete(quizId);
                quizzes = quizRepository.GetAllQuizzes(false);
            }
            return PartialView("_QuizzesTable", quizzes);
        }
    }
}