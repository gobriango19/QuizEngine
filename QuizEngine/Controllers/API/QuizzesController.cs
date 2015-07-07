using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using QuizEngine.Models;
using QuizEngine.Data;
using QuizEngine.Data.Repositories;
using QuizEngine.Utilities.Extensions;

namespace QuizEngine.Controllers.API
{
    [RoutePrefix("api/quizzes")]
    public class QuizzesController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetQuizzes(bool activeOnly = true)
        {
            IEnumerable<Quiz> quizzes = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quizzes = quizRepository.GetAllQuizzes(activeOnly);
            }
            if (quizzes == null || quizzes.ToList().Count == 0)
            {
                return NotFound();
            }
            return Ok(quizzes);
        }

        [HttpGet]
        [Route("{quizId:long}")]
        public IHttpActionResult GetQuiz(long quizId, bool activeOnly = true)
        {
            Quiz quiz = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quiz = quizRepository.Get(quizId, activeOnly);
            }
            if(quiz == null)
            {
                return NotFound();
            }
            return Ok(quiz);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostQuiz(Quiz quiz)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quizRepository.Add(quiz);
            }
            return Ok(quiz);
        }

        [HttpPut]
        [Route("{quizId:long}")]
        public IHttpActionResult PutQuiz(Quiz quiz)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quizRepository.Update(quiz);
            }
            return Ok(quiz);
        }

        [HttpDelete]
        [Route("{quizId:long}")]
        public void DeleteQuiz(long quizId)
        {
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var quizRepository = new QuizRepository(dbUnitOfWork);
                quizRepository.Delete(quizId);
            }
        }
    }
}