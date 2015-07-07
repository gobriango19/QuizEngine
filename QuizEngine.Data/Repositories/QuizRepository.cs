using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuizEngine.Models;
using QuizEngine.Data.Interfaces;

namespace QuizEngine.Data.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private DbUnitOfWork<QuizEngineDbContext> _dbUnitOfWork;

        public QuizRepository(DbUnitOfWork<QuizEngineDbContext> dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        public IEnumerable<Quiz> GetAllQuizzes(bool activeOnly = true)
        {
            IQueryable<Quiz> query = from q in _dbUnitOfWork.DbContext.Quizzes
                                     orderby q.QuizId descending
                                     select q;

            if(activeOnly)
            {
                query = query.Where(q => q.IsActive);
            }

            return query.ToList();
        }

        public Quiz Get(long quizId)
        {
            return Get(quizId, true);
        }

        public Quiz Get(long quizId, bool activeOnly = true)
        {
            var query = from q in _dbUnitOfWork.DbContext.Quizzes
                        where q.QuizId == quizId
                        select q;

            if(activeOnly)
            {
                query = query.Where(q => q.IsActive);
            }

            return query.FirstOrDefault();

        }

        public Quiz Add(Quiz quiz)
        {
            // confirm quizId doesn't exist
            if (quiz.QuizId != 0)
            {
                throw new ArgumentException("Provided quiz is invalid for add: contains quizId.");
            }

            quiz.CreateDate = DateTime.UtcNow;
            quiz.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Quizzes.Add(quiz);
            _dbUnitOfWork.SaveChanges();

            return quiz;
        }

        public Quiz Update(Quiz quiz)
        {
            // confirm quizId exists
            if (quiz.QuizId == 0)
            {
                throw new ArgumentException("Provided quiz is invalid for update: missing quizId.");
            }

            // confirm the quiz exists in the DB
            var checkQuiz = Get(quiz.QuizId, false);
            if (checkQuiz == null)
            {
                throw new ArgumentException("Provided quiz is invalid for update: cannot find quiz in database.");
            }
            _dbUnitOfWork.DbContext.Entry(checkQuiz).State = EntityState.Detached; // detach the looked up quiz so there is no conflict with updated quiz

            quiz.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Modified;
            _dbUnitOfWork.DbContext.Entry(quiz).Property(q => q.CreateDate).IsModified = false; // never change the created date on update
            _dbUnitOfWork.SaveChanges();

            _dbUnitOfWork.DbContext.Entry(quiz).Reload();

            return quiz;
        }

        public void Delete(long quizId)
        {
            // confirm the quiz exists in the DB
            var deleteQuiz = Get(quizId, false);
            if (deleteQuiz == null)
            {
                throw new ArgumentException("Provided quizId is invalid for deletion: cannot find quiz in database.");
            }

            Delete(deleteQuiz);
        }

        public void Delete(Quiz quiz)
        {
            // need to delete all questions associated with the quiz
            // TODO EF doesn't automatically batch, refactor to use batch or use cascade
            var questionRepository = new QuestionRepository(_dbUnitOfWork);
            var deleteQuestions = questionRepository.GetQuestionsForQuiz(quiz.QuizId, false);
            foreach(var deleteQuestion in deleteQuestions)
            {
                questionRepository.Delete(deleteQuestion);
            }

            // need to delete all results associated with the quiz
            // TODO EF doesn't automatically batch, refactor to use batch or use cascade
            var resultRepository = new ResultRepository(_dbUnitOfWork);
            var deleteResults = resultRepository.GetResultsForQuiz(quiz.QuizId, false);
            foreach(var deleteResult in deleteResults)
            {
                resultRepository.Delete(deleteResult);
            }

            _dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Deleted;
            _dbUnitOfWork.SaveChanges();                
        }
    }
}
