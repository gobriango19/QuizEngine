using QuizEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizEngine.Data.Interfaces
{
    interface IQuestionRepository : IRepository<Question, long>
    {
        IEnumerable<Question> GetQuestionsForQuiz(long quizId, bool activeQuizOnly = true);
        Question Get(long questionId, bool activeQuizOnly = true);
        void Delete(long questionId);
    }
}
