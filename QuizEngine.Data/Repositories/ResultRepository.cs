using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuizEngine.Models;

namespace QuizEngine.Data.Repositories
{
    public static class ResultRepository
    {
        public static List<Result> GetResultsForQuiz(long quizId, bool activeQuizOnly = true)
        {
            List<Result> results = null;

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var query = from r in dbUnitOfWork.DbContext.Results
                            where r.QuizId == quizId
                            select r;

                if(activeQuizOnly)
                {
                    query = query.Where(r => r.Quiz.IsActive);
                }

                results = query.ToList();
            }

            return results;
        }

        public static Result GetResult(long resultId, bool activeQuizOnly = true)
        {
            Result result = null;

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var query = from r in dbUnitOfWork.DbContext.Results
                            where r.ResultId == resultId
                            select r;

                if(activeQuizOnly)
                {
                    query = query.Where(r => r.Quiz.IsActive);
                }

                result = query.FirstOrDefault();
            }

            return result;
        }

        public static Result AddResult(Result result)
        {
            // confirm resultId doesn't exist
            if(result.ResultId != 0)
            {
                throw new ArgumentException("Provided result is invalid for add: contains resultId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm quiz exists
                var quiz = QuizRepository.GetQuiz(result.QuizId, false);
                if(quiz == null)
                {
                    throw new ArgumentException("Provided result is invalid for add: cannot find quiz in database.");
                }
                dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it

                result.CreateDate = DateTime.UtcNow;
                result.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Results.Add(result);
                dbUnitOfWork.SaveChanges();
            }

            return result;
        }

        public static Result UpdateResult(Result result)
        {
            // confirm resultId exists
            if(result.ResultId == 0)
            {
                throw new ArgumentException("Provided result is invalid for update: missing resultId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm result exists in the DB
                var checkResult = GetResult(result.ResultId, false);
                if(checkResult == null)
                {
                    throw new ArgumentException("Provided result is invalid for update: cannot find result in database.");
                }
                // confirm quiz exists if it has been updated
                if(checkResult.QuizId != result.QuizId)
                {
                    var quiz = QuizRepository.GetQuiz(result.QuizId, false);
                    if(quiz == null)
                    {
                        throw new ArgumentException("Provided result is invalid for update: cannot find quiz in database.");
                    }
                    dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it
                }
                dbUnitOfWork.DbContext.Entry(checkResult).State = EntityState.Detached; // detach the looked up result so there is no conflict with updated result

                result.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Entry(result).State = EntityState.Modified;
                dbUnitOfWork.DbContext.Entry(result).Property(r => r.CreateDate).IsModified = false; // never change the created date on update
                dbUnitOfWork.SaveChanges();

                dbUnitOfWork.DbContext.Entry(result).Reload();
            }

            return result;
        }

        public static void DeleteResult(long resultId)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm result exists in the DB
                var deleteResult = GetResult(resultId, false);
                if(deleteResult == null)
                {
                    throw new ArgumentException("Provided resultId is invalid for deletion: cannot find result in database.");
                }

                DeleteResult(deleteResult);
            }
        }

        public static void DeleteResult(Result result)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                dbUnitOfWork.DbContext.Entry(result).State = EntityState.Deleted;
                dbUnitOfWork.SaveChanges();                
            }
        }
    }
}
