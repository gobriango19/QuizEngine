using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizEngine.Models;
using QuizEngine.SDK.Interfaces;

namespace QuizEngine.SDK.Providers
{
    public class QuizEngineApiProvider : IQuizEngineProvider
    {
        private RestApiClient _restApiClient;

        public QuizEngineApiProvider(string baseUrl)
        {
            if(string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("Cannot instantiate REST API client: baseUrl is empty/null.");
            }

            _restApiClient = new RestApiClient(baseUrl);
        }

        #region Quiz
        public ApiResponse<List<Quiz>> GetAllQuizzes(bool activeOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeOnly", activeOnly.ToString()) };
            return _restApiClient.Get<List<Quiz>>("api/quizzes", parameters);
        }

        public ApiResponse<Quiz> GetQuiz(long quizId, bool activeOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeOnly", activeOnly.ToString()) };
            var relativePath = string.Format("api/quizzes/{0}", quizId);
            return _restApiClient.Get<Quiz>(relativePath, parameters);
        }

        public ApiResponse<Quiz> AddQuiz(Quiz quiz)
        {
            return _restApiClient.Post<Quiz>("api/quizzes", null, quiz);
        }

        public ApiResponse<Quiz> UpdateQuiz(Quiz quiz)
        {
            var relativePath = string.Format("api/quizzes/{0}", quiz.QuizId);
            return _restApiClient.Put<Quiz>(relativePath, null, quiz);
        }

        public ApiResponse<Quiz> DeleteQuiz(long quizId)
        {
            var relativePath = string.Format("api/quizzes/{0}", quizId);
            return _restApiClient.Delete<Quiz>(relativePath, null);
        }
        #endregion

        #region Question
        public ApiResponse<List<Question>> GetQuestionsForQuiz(long quizId, bool activeQuizOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeQuizOnly", activeQuizOnly.ToString()) };
            return _restApiClient.Get<List<Question>>("api/questions", parameters);
        }

        public ApiResponse<Question> GetQuestion(long questionId, bool activeQuizOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeQuizOnly", activeQuizOnly.ToString()) };
            var relativePath = string.Format("api/questions/{0}", questionId);
            return _restApiClient.Get<Question>(relativePath, parameters);
        }

        public ApiResponse<Question> AddQuestion(Question question)
        {
            return _restApiClient.Post<Question>("api/questions", null, question);
        }

        public ApiResponse<Question> UpdateQuestion(Question question)
        {
            var relativePath = string.Format("api/questions/{0}", question.QuestionId);
            return _restApiClient.Put<Question>(relativePath, null, question);
        }

        public ApiResponse<Question> DeleteQuestion(long questionId)
        {
            var relativePath = string.Format("api/questions/{0}", questionId);
            return _restApiClient.Delete<Question>(relativePath, null);
        }
        #endregion

        #region Answer
        public ApiResponse<List<Answer>> GetAnswersForQuestion(long questionId, bool activeQuizOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeQuizOnly", activeQuizOnly.ToString()) };
            return _restApiClient.Get<List<Answer>>("api/answers", parameters);
        }

        public ApiResponse<Answer> GetAnswer(long answerId, bool activeQuizOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeQuizOnly", activeQuizOnly.ToString()) };
            var relativePath = string.Format("api/answers/{0}", answerId);
            return _restApiClient.Get<Answer>(relativePath, parameters);
        }

        public ApiResponse<Answer> AddAnswer(Answer answer)
        {
            return _restApiClient.Post<Answer>("api/answers", null, answer);
        }

        public ApiResponse<Answer> UpdateAnswer(Answer answer)
        {
            var relativePath = string.Format("api/answers/{0}", answer.AnswerId);
            return _restApiClient.Put<Answer>(relativePath, null, answer);
        }

        public ApiResponse<Answer> DeleteAnswer(long answerId)
        {
            var relativePath = string.Format("api/answers/{0}", answerId);
            return _restApiClient.Delete<Answer>(relativePath, null);
        }
        #endregion

        #region Result
        public ApiResponse<List<Result>> GetResultsForQuiz(long quizId, bool activeQuizOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeQuizOnly", activeQuizOnly.ToString()) };
            return _restApiClient.Get<List<Result>>("api/results", parameters);
        }

        public ApiResponse<Result> GetResult(long resultId, bool activeQuizOnly = true)
        {
            var parameters = new KeyValuePair<string, string>[] { new KeyValuePair<string, string>("activeQuizOnly", activeQuizOnly.ToString()) };
            var relativePath = string.Format("api/results/{0}", resultId);
            return _restApiClient.Get<Result>(relativePath, parameters);
        }

        public ApiResponse<Result> AddResult(Result result)
        {
            return _restApiClient.Post<Result>("api/results", null, result);
        }

        public ApiResponse<Result> UpdateResult(Result result)
        {
            var relativePath = string.Format("api/results/{0}", result.ResultId);
            return _restApiClient.Put<Result>(relativePath, null, result);
        }

        public ApiResponse<Result> DeleteResult(long resultId)
        {
            var relativePath = string.Format("api/results/{0}", resultId);
            return _restApiClient.Delete<Result>(relativePath, null);
        }
        #endregion
    }
}
