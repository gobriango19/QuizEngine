using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizEngine.Models;

namespace QuizEngine.Data.Interfaces
{
    interface IQuizRepository : IRepository<Quiz, long>
    {
        IEnumerable<Quiz> GetAllQuizzes(bool activeOnly = true);
        void Delete(long quizId);
    }
}
