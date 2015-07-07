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
using QuizEngine.Utilities.ExceptionFilters;

namespace QuizEngine.Controllers.API
{
    [RoutePrefix("api/answers")]
    public class AnswersController : ApiController
    {
        [HttpGet]
        [Route("~/api/questions/{questionId:long}/answers")]
        public IHttpActionResult GetAnswers(long questionId, bool activeQuizOnly = true)
        {
            IEnumerable<Answer> answers = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answers = answerRepository.GetAnswersForQuestion(questionId, activeQuizOnly);
            }
            if(answers == null || answers.ToList().Count == 0)
            {
                return NotFound();
            }
            return Ok(answers);
        }

        [HttpGet]
        [Route("{answerId:long}")]
        public IHttpActionResult GetAnswer(long answerId, bool activeQuizOnly = true)
        {
            Answer answer = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answer = answerRepository.Get(answerId, activeQuizOnly);
            }
            if(answer == null)
            {
                return NotFound();
            }
            return Ok(answer);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostAnswer(Answer answer)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answerRepository.Add(answer);
            }
            return Ok(answer);
        }

        [HttpPut]
        [Route("{answerId:long}")]
        public IHttpActionResult PutAnswer(Answer answer)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answerRepository.Update(answer);
            }
            return Ok(answer);
        }

        [HttpDelete]
        [Route("{answerId:long}")]
        public void DeleteAnswer(long answerId)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var answerRepository = new AnswerRepository(dbUnitOfWork);
                answerRepository.Delete(answerId);
            }
        }
    }
}
