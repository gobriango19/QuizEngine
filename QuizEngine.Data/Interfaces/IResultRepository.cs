using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizEngine.Models;

namespace QuizEngine.Data.Interfaces
{
    interface IResultRepository : IRepository<Result, long>
    {
        IEnumerable<Result> GetResultsForQuiz(long resultId, bool activeQuizOnly = true);
        Result Get(long resultId, bool activeQuizOnly = true);
        void Delete(long resultId);
    }
}
