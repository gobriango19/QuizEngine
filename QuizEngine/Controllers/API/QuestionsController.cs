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
    [RoutePrefix("api/questions")]
    public class QuestionsController : ApiController
    {
        [HttpGet]
        [Route("~/api/quizzes/{quizId:long}/questions")]
        public IHttpActionResult GetQuestions(long quizId, bool activeQuizOnly = true)
        {
            var questions = QuestionRepository.GetQuestionsForQuiz(quizId, activeQuizOnly);

            if(questions == null || questions.Count == 0)
            {
                return NotFound();
            }

            return Ok(questions);
        }

        [HttpGet]
        [Route("{questionId:long}")]
        public IHttpActionResult GetQuestion(long questionId, bool activeQuizOnly = true)
        {
            var question = QuestionRepository.GetQuestion(questionId, activeQuizOnly);

            if(question == null)
            {
                return NotFound();
            }

            return Ok(question);
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult PostQuestion(Question question)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }

            QuestionRepository.AddQuestion(question);

            return Ok(question);
        }

        [HttpPut]
        [Route("{questionId:long}")]
        public IHttpActionResult PutQuestion(Question question)
        {
            if(!ModelState.IsValid)
            {
                var errorMessage = ModelState.GenerateErrorMessage();
                return BadRequest(errorMessage);
            }

            QuestionRepository.UpdateQuestion(question);

            return Ok(question);
        }

        [HttpDelete]
        [Route("{questionId:long}")]
        public void DeleteQuestion(long questionId)
        {
            QuestionRepository.DeleteQuestion(questionId);
        }
    }
}
