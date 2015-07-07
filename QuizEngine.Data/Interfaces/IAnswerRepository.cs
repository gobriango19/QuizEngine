using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizEngine.Models;

namespace QuizEngine.Data.Interfaces
{
    public interface IAnswerRepository : IRepository<Answer, long>
    {
        IEnumerable<Answer> GetAnswersForQuestion(long questionId, bool activeQuizOnly = true);
        Answer Get(long answerId, bool activeQuizOnly = true);
        void Delete(long answerId);
    }
}
