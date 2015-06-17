using QuizEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizEngine.SDK.Interfaces
{
    public interface IQuizEngineProvider
    {
        #region Quiz
        ApiResponse<List<Quiz>> GetAllQuizzes(bool activeOnly = true);
        ApiResponse<Quiz> GetQuiz(long quizId, bool activeOnly = true);
        ApiResponse<Quiz> AddQuiz(Quiz quiz);
        ApiResponse<Quiz> UpdateQuiz(Quiz quiz);
        ApiResponse<Quiz> DeleteQuiz(long quizId);
        #endregion

        #region Question
        ApiResponse<List<Question>> GetQuestionsForQuiz(long quizId, bool activeQuizOnly = true);
        ApiResponse<Question> GetQuestion(long questionId, bool activeQuizOnly = true);
        ApiResponse<Question> AddQuestion(Question question);
        ApiResponse<Question> UpdateQuestion(Question question);
        ApiResponse<Question> DeleteQuestion(long questionId);
        #endregion

        #region Answer
        ApiResponse<List<Answer>> GetAnswersForQuestion(long questionId, bool activeQuizOnly = true);
        ApiResponse<Answer> GetAnswer(long answerId, bool activeQuizOnly = true);
        ApiResponse<Answer> AddAnswer(Answer answer);
        ApiResponse<Answer> UpdateAnswer(Answer answer);
        ApiResponse<Answer> DeleteAnswer(long answerId);
        #endregion

        #region Result
        ApiResponse<List<Result>> GetResultsForQuiz(long quizId, bool activeQuizOnly = true);
        ApiResponse<Result> GetResult(long resultId, bool activeQuizOnly = true);
        ApiResponse<Result> AddResult(Result result);
        ApiResponse<Result> UpdateResult(Result result);
        ApiResponse<Result> DeleteResult(long resultId);
        #endregion
    }
}
