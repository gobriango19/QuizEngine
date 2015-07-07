using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuizEngine.Models;

namespace QuizEngine.Data.Repositories
{
    public class ResultRepository
    {
        private DbUnitOfWork<QuizEngineDbContext> _dbUnitOfWork;

        public ResultRepository(DbUnitOfWork<QuizEngineDbContext> dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        public IEnumerable<Result> GetResultsForQuiz(long quizId, bool activeQuizOnly = true)
        {
            var query = from r in _dbUnitOfWork.DbContext.Results
                        where r.QuizId == quizId
                        select r;

            if(activeQuizOnly)
            {
                query = query.Where(r => r.Quiz.IsActive);
            }

            return query.ToList();
        }

        public Result Get(long resultId)
        {
            return Get(resultId, true);
        }

        public Result Get(long resultId, bool activeQuizOnly = true)
        {
            var query = from r in _dbUnitOfWork.DbContext.Results
                        where r.ResultId == resultId
                        select r;

            if(activeQuizOnly)
            {
                query = query.Where(r => r.Quiz.IsActive);
            }

            return query.FirstOrDefault();
        }

        public Result Add(Result result)
        {
            // confirm resultId doesn't exist
            if(result.ResultId != 0)
            {
                throw new ArgumentException("Provided result is invalid for add: contains resultId.");
            }

            // confirm quiz exists
            var quizRepository = new QuizRepository(_dbUnitOfWork);
            var quiz = quizRepository.Get(result.QuizId, false);
            if(quiz == null)
            {
                throw new ArgumentException("Provided result is invalid for add: cannot find quiz in database.");
            }
            _dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it

            result.CreateDate = DateTime.UtcNow;
            result.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Results.Add(result);
            _dbUnitOfWork.SaveChanges();

            return result;
        }

        public Result Update(Result result)
        {
            // confirm resultId exists
            if(result.ResultId == 0)
            {
                throw new ArgumentException("Provided result is invalid for update: missing resultId.");
            }

            // confirm result exists in the DB
            var checkResult = Get(result.ResultId, false);
            if(checkResult == null)
            {
                throw new ArgumentException("Provided result is invalid for update: cannot find result in database.");
            }
            // confirm quiz exists if it has been updated
            if(checkResult.QuizId != result.QuizId)
            {
                var quizRepository = new QuizRepository(_dbUnitOfWork);
                var quiz = quizRepository.Get(result.QuizId, false);
                if(quiz == null)
                {
                    throw new ArgumentException("Provided result is invalid for update: cannot find quiz in database.");
                }
                _dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it
            }
            _dbUnitOfWork.DbContext.Entry(checkResult).State = EntityState.Detached; // detach the looked up result so there is no conflict with updated result

            result.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Entry(result).State = EntityState.Modified;
            _dbUnitOfWork.DbContext.Entry(result).Property(r => r.CreateDate).IsModified = false; // never change the created date on update
            _dbUnitOfWork.SaveChanges();

            _dbUnitOfWork.DbContext.Entry(result).Reload();

            return result;
        }

        public void Delete(long resultId)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm result exists in the DB
                var deleteResult = Get(resultId, false);
                if(deleteResult == null)
                {
                    throw new ArgumentException("Provided resultId is invalid for deletion: cannot find result in database.");
                }

                Delete(deleteResult);
            }
        }

        public void Delete(Result result)
        {
            _dbUnitOfWork.DbContext.Entry(result).State = EntityState.Deleted;
            _dbUnitOfWork.SaveChanges();                
        }
    }
}
