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
    [RoutePrefix("api/results")]
    public class ResultsController : ApiController
    {
        [HttpGet]
        [Route("~/api/quizzes/{quizId:long}/results")]
        public IHttpActionResult GetResults(long quizId, bool activeQuizOnly = true)
        {
            IEnumerable<Result> results = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                results = resultRepository.GetResultsForQuiz(quizId, activeQuizOnly);
            }
            if(results == null || results.ToList().Count == 0)
            {
                return NotFound();
            }
            return Ok(results);
        }

        [HttpGet]
        [Route("{resultId:long}")]
        public IHttpActionResult GetResult(long resultId, bool activeQuizOnly = true)
        {
            Result result = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                result = resultRepository.Get(resultId, activeQuizOnly);
            }
            if(result == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostResult(Result result)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                resultRepository.Add(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Route("{resultId:long}")]
        public IHttpActionResult PutResult(Result result)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                resultRepository.Update(result);
            }
            return Ok(result);
        }

        [HttpDelete]
        [Route("{resultId:long}")]
        public void DeleteResult(long resultId)
        {
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var resultRepository = new ResultRepository(dbUnitOfWork);
                resultRepository.Delete(resultId);
            }
        }
    }
}
