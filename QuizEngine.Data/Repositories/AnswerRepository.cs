using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using QuizEngine.Models;

namespace QuizEngine.Data.Repositories
{
    public static class AnswerRepository
    {
        public static List<Answer> GetAnswersForQuestion(long questionId, bool activeQuizOnly = true)
        {
            List<Answer> answers = null;

            var question = QuestionRepository.GetQuestion(questionId, activeQuizOnly);
            if(question != null)
            {
                answers = question.Answers.ToList();
            }

            return answers;
        }

        public static Answer GetAnswer(long answerId, bool activeQuizOnly = true)
        {
            Answer answer = null;

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                var query = from a in dbUnitOfWork.DbContext.Answers
                            where a.AnswerId == answerId
                            select a;

                if(activeQuizOnly)
                {
                    query = query.Where(a => a.Question.Quiz.IsActive);
                }

                answer = query.FirstOrDefault();
            }

            return answer;
        }

        public static Answer AddAnswer(Answer answer)
        {
            // confirm answerId doesn't exist
            if(answer.AnswerId != 0)
            {
                throw new ArgumentException("Provided answer is invalid for add: contains answerId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm question exists
                var question = QuestionRepository.GetQuestion(answer.QuestionId, false);
                if (question == null)
                {
                    throw new ArgumentException("Provided answer is invalid for add: questionId is an invalid reference.");
                }
                dbUnitOfWork.DbContext.Entry(question).State = EntityState.Detached; // we don't need to track this, so let's detach it

                answer.CreateDate = DateTime.UtcNow;
                answer.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Answers.Add(answer);
                dbUnitOfWork.SaveChanges();
            }

            return answer;
        }

        public static Answer UpdateAnswer(Answer answer)
        {
            // confirm answerId exists
            if(answer.AnswerId == 0)
            {
                throw new ArgumentException("Provided answer is invalid for update: missing answerId.");
            }

            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm answer exists in the DB
                var checkAnswer = GetAnswer(answer.AnswerId, false);
                if(checkAnswer == null)
                {
                    throw new ArgumentException("Provided answer is invalid for update: cannot find answer in database.");
                }

                // confirm question exists if it has been updated
                if (checkAnswer.QuestionId != answer.QuestionId)
                {
                    var question = QuestionRepository.GetQuestion(answer.QuestionId, false);
                    if (question == null)
                    {
                        throw new ArgumentException("Provided answer is invalid for add: questionId is an invalid reference.");
                    }
                    dbUnitOfWork.DbContext.Entry(question).State = EntityState.Detached; // we don't need to track this, so let's detach it
                }
                dbUnitOfWork.DbContext.Entry(checkAnswer).State = EntityState.Detached; // detach the looked up answer so there is no conflict with the updated answer

                answer.UpdateDate = DateTime.UtcNow;

                dbUnitOfWork.DbContext.Entry(answer).State = EntityState.Modified;
                dbUnitOfWork.DbContext.Entry(answer).Property(a => a.CreateDate).IsModified = false; // never change the created date on update
                dbUnitOfWork.SaveChanges();

                dbUnitOfWork.DbContext.Entry(answer).Reload();
            }

            return answer;
        }

        public static void DeleteAnswer(long answerId)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                // confirm the answer exists in the DB
                var deleteAnswer = GetAnswer(answerId, false);
                if(deleteAnswer == null)
                {
                    throw new ArgumentException("Provided answerId is invalid for deletion: cannot find answer in database.");
                }

                DeleteAnswer(deleteAnswer);
            }
        }

        public static void DeleteAnswer(Answer answer)
        {
            using(var dbUnitOfWork = new DbUnitOfWork<QuizEngineDbContext>())
            {
                dbUnitOfWork.DbContext.Entry(answer).State = EntityState.Deleted;
                dbUnitOfWork.SaveChanges();
            }
        }
    }
}
