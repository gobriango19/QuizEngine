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
            var results = ResultRepository.GetResultsForQuiz(quizId, activeQuizOnly);

            if(results == null || results.Count == 0)
            {
                return NotFound();
            }

            return Ok(results);
        }

        [HttpGet]
        [Route("{resultId:long}")]
        public IHttpActionResult GetResult(long resultId, bool activeQuizOnly = true)
        {
            var result = ResultRepository.GetResult(resultId, activeQuizOnly);

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

            ResultRepository.AddResult(result);

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

            ResultRepository.UpdateResult(result);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{resultId:long}")]
        public void DeleteResult(long resultId)
        {
            ResultRepository.DeleteResult(resultId);
        }
    }
}
