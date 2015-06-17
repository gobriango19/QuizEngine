using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuizEngine.Models;

namespace QuizEngine.Data.Repositories
{
    public static class QuestionRepository
    {
        public static List<Question> GetQuestionsForQuiz(long quizId, bool activeQuizOnly = true)
        {
            List<Question> questions = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                /*var query = (from q in dbUnitOfWork.DbContext.Questions
                             where q.QuizId == quizId
                             from a in dbUnitOfWork.DbContext.Answers
                                .Where(a => a.QuestionId == q.QuestionId)
                                .DefaultIfEmpty()
                             select q)
                             .Include("Answers");*/

                var query = (from q in dbUnitOfWork.DbContext.Questions
                             where q.QuizId == quizId
                             select q)
                            .Include("Answers");

                if(activeQuizOnly)
                {
                    query = query.Where(q => q.Quiz.IsActive == true);
                }

                questions = query.ToList();
            }

            return questions;
        }

        public static Question GetQuestion(long questionId, bool activeQuizOnly = true)
        {
            Question question = null;
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                /*var query = (from q in dbUnitOfWork.DbContext.Questions
                            where q.QuestionId == questionId
                            from a in dbUnitOfWork.DbContext.Answers
                                .Where(a => a.QuestionId == q.QuestionId)
                                .DefaultIfEmpty()
                            select q)
                            .Include("Answers");*/

                var query = (from q in dbUnitOfWork.DbContext.Questions
                            where q.QuestionId == questionId
                            select q)
                            .Include("Answers");

                if(activeQuizOnly)
                {
                    query = query.Where(q => q.Quiz.IsActive == true);
                }

                question = query.FirstOrDefault();
            }

            return question;
        }

        public static Question AddQuestion(Question question)
        {
            // confirm questionId doesn't exist
            if(question.QuestionId != 0)
            {
                throw new ArgumentException("Provided question is invalid for add: contains questionId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm quiz exists
                var quiz = QuizRepository.GetQuiz(question.QuizId, false);
                if (quiz == null)
                {
                    throw new ArgumentException("Provided question is invalid for add: cannot find quiz in database.");
                }
                dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it

                question.CreateDate = DateTime.UtcNow;
                question.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Questions.Add(question);
                dbUnitOfWork.SaveChanges();
            }

            return question;
        }

        public static Question UpdateQuestion(Question question)
        {
            // confirm questionId exists
            if(question.QuestionId == 0)
            {
                throw new ArgumentException("Provided question is invalid for update: missing questionId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm the question exists in the DB
                var checkQuestion = GetQuestion(question.QuestionId, false);
                if(checkQuestion == null)
                {
                    throw new ArgumentException("Provided question is invalid for update: cannot find question in database.");
                }

                // confirm quiz exists if it has been updated
                if(checkQuestion.QuizId != question.QuizId)
                {
                    var quiz = QuizRepository.GetQuiz(question.QuizId, false);
                    if(quiz == null)
                    {
                        throw new ArgumentException("Provided question is invalid for update: cannot find quiz in database.");
                    }
                    dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it
                }
                dbUnitOfWork.DbContext.Entry(checkQuestion).State = EntityState.Detached; // detach the looked up question so there is no conflict with the updated question

                question.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Entry(question).State = EntityState.Modified;
                dbUnitOfWork.DbContext.Entry(question).Property(q => q.CreateDate).IsModified = false; // never change the created date on update
                dbUnitOfWork.SaveChanges();

                dbUnitOfWork.DbContext.Entry(question).Reload();
            }

            return question;
        }

        public static void DeleteQuestion(long questionId)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm the question exists in the DB
                var deleteQuestion = GetQuestion(questionId, false);
                if(deleteQuestion == null)
                {
                    throw new ArgumentException("Provided questionId is invalid for deletion: cannot find question in database.");
                }

                DeleteQuestion(deleteQuestion);
            }
        }

        public static void DeleteQuestion(Question question)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // need to delete all answers associated with question
                var deleteAnswers = AnswerRepository.GetAnswersForQuestion(question.QuestionId, false);
                foreach(var deleteAnswer in deleteAnswers)
                {
                    AnswerRepository.DeleteAnswer(deleteAnswer);
                }

                dbUnitOfWork.DbContext.Entry(question).State = EntityState.Deleted;
                dbUnitOfWork.SaveChanges();
            }
        }
    }
}
