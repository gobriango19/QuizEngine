using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuizEngine.Models;

namespace QuizEngine.Data.Repositories
{
    public static class QuizRepository
    {
        public static List<Quiz> GetAllQuizzes(bool activeOnly = true)
        {
            List<Quiz> quizzes = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                IQueryable<Quiz> query = from q in dbUnitOfWork.DbContext.Quizzes
                                         orderby q.QuizId descending
                                         select q;

                if(activeOnly)
                {
                    query = query.Where(q => q.IsActive);
                }
                    
                quizzes = query.ToList();
            }
            return quizzes;
        }

        public static Quiz GetQuiz(long quizId, bool activeOnly = true)
        {
            Quiz quiz = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var query = from q in dbUnitOfWork.DbContext.Quizzes
                            where q.QuizId == quizId
                            select q;

                if(activeOnly)
                {
                    query = query.Where(q => q.IsActive);
                }

                quiz = query.FirstOrDefault();
            }

            return quiz;
        }

        public static Quiz AddQuiz(Quiz quiz)
        {
            // confirm quizId doesn't exist
            if (quiz.QuizId != 0)
            {
                throw new ArgumentException("Provided quiz is invalid for add: contains quizId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                quiz.CreateDate = DateTime.UtcNow;
                quiz.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Quizzes.Add(quiz);
                dbUnitOfWork.SaveChanges();
            }

            return quiz;
        }

        public static Quiz UpdateQuiz(Quiz quiz)
        {
            // confirm quizId exists
            if (quiz.QuizId == 0)
            {
                throw new ArgumentException("Provided quiz is invalid for update: missing quizId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm the quiz exists in the DB
                var checkQuiz = GetQuiz(quiz.QuizId, false);
                if (checkQuiz == null)
                {
                    throw new ArgumentException("Provided quiz is invalid for update: cannot find quiz in database.");
                }
                dbUnitOfWork.DbContext.Entry(checkQuiz).State = EntityState.Detached; // detach the looked up quiz so there is no conflict with updated quiz

                quiz.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Modified;
                dbUnitOfWork.DbContext.Entry(quiz).Property(q => q.CreateDate).IsModified = false; // never change the created date on update
                dbUnitOfWork.SaveChanges();

                dbUnitOfWork.DbContext.Entry(quiz).Reload();
            }

            return quiz;
        }

        public static void DeleteQuiz(long quizId)
        {
            using (var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm the quiz exists in the DB
                var deleteQuiz = GetQuiz(quizId, false);
                if (deleteQuiz == null)
                {
                    throw new ArgumentException("Provided quizId is invalid for deletion: cannot find quiz in database.");
                }

                DeleteQuiz(deleteQuiz);
            }
        }

        public static void DeleteQuiz(Quiz quiz)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // need to delete all questions associated with the quiz
                var deleteQuestions = QuestionRepository.GetQuestionsForQuiz(quiz.QuizId, false);
                foreach(var deleteQuestion in deleteQuestions)
                {
                    QuestionRepository.DeleteQuestion(deleteQuestion);
                }

                // need to delete all results associated with the quiz
                var deleteResults = ResultRepository.GetResultsForQuiz(quiz.QuizId, false);
                foreach(var deleteResult in deleteResults)
                {
                    ResultRepository.DeleteResult(deleteResult);
                }

                dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Deleted;
                dbUnitOfWork.SaveChanges();                
            }
        }
    }
}
