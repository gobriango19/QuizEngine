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
    public class QuestionRepository : IQuestionRepository
    {
        private DbUnitOfWork<QuizEngineDbContext> _dbUnitOfWork;

        public QuestionRepository(DbUnitOfWork<QuizEngineDbContext> dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        public IEnumerable<Question> GetQuestionsForQuiz(long quizId, bool activeQuizOnly = true)
        {
            /*var query = (from q in dbUnitOfWork.DbContext.Questions
                            where q.QuizId == quizId
                            from a in dbUnitOfWork.DbContext.Answers
                            .Where(a => a.QuestionId == q.QuestionId)
                            .DefaultIfEmpty()
                            select q)
                            .Include("Answers");*/

            var query = (from q in _dbUnitOfWork.DbContext.Questions
                        where q.QuizId == quizId
                        select q)
                        .Include("Answers");

            if(activeQuizOnly)
            {
                query = query.Where(q => q.Quiz.IsActive);
            }

            return query.ToList();
        }

        public Question Get(long questionId)
        {
            return Get(questionId, true);
        }

        public Question Get(long questionId, bool activeQuizOnly = true)
        {
            /*var query = (from q in dbUnitOfWork.DbContext.Questions
                        where q.QuestionId == questionId
                        from a in dbUnitOfWork.DbContext.Answers
                            .Where(a => a.QuestionId == q.QuestionId)
                            .DefaultIfEmpty()
                        select q)
                        .Include("Answers");*/

            var query = (from q in _dbUnitOfWork.DbContext.Questions
                        where q.QuestionId == questionId
                        select q)
                        .Include("Answers");

            if(activeQuizOnly)
            {
                query = query.Where(q => q.Quiz.IsActive);
            }

            return query.FirstOrDefault();
        }

        public Question Add(Question question)
        {
            // confirm questionId doesn't exist
            if(question.QuestionId != 0)
            {
                throw new ArgumentException("Provided question is invalid for add: contains questionId.");
            }

            // confirm quiz exists
            var quizRepository = new QuizRepository(_dbUnitOfWork);
            var quiz = quizRepository.Get(question.QuizId, false);
            if (quiz == null)
            {
                throw new ArgumentException("Provided question is invalid for add: cannot find quiz in database.");
            }
            _dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it

            question.CreateDate = DateTime.UtcNow;
            question.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Questions.Add(question);
            _dbUnitOfWork.SaveChanges();

            return question;
        }

        public Question Update(Question question)
        {
            // confirm questionId exists
            if(question.QuestionId == 0)
            {
                throw new ArgumentException("Provided question is invalid for update: missing questionId.");
            }

            // confirm the question exists in the DB
            var checkQuestion = Get(question.QuestionId, false);
            if(checkQuestion == null)
            {
                throw new ArgumentException("Provided question is invalid for update: cannot find question in database.");
            }

            // confirm quiz exists if it has been updated
            if(checkQuestion.QuizId != question.QuizId)
            {
                var quizRepository = new QuizRepository(_dbUnitOfWork);
                var quiz = quizRepository.Get(question.QuizId, false);
                if(quiz == null)
                {
                    throw new ArgumentException("Provided question is invalid for update: cannot find quiz in database.");
                }
                _dbUnitOfWork.DbContext.Entry(quiz).State = EntityState.Detached; // we don't need to track this, so let's detach it
            }
            _dbUnitOfWork.DbContext.Entry(checkQuestion).State = EntityState.Detached; // detach the looked up question so there is no conflict with the updated question

            question.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Entry(question).State = EntityState.Modified;
            _dbUnitOfWork.DbContext.Entry(question).Property(q => q.CreateDate).IsModified = false; // never change the created date on update
            _dbUnitOfWork.SaveChanges();

            _dbUnitOfWork.DbContext.Entry(question).Reload();

            return question;
        }

        public void Delete(long questionId)
        {
            // confirm the question exists in the DB
            var deleteQuestion = Get(questionId, false);
            if(deleteQuestion == null)
            {
                throw new ArgumentException("Provided questionId is invalid for deletion: cannot find question in database.");
            }

            Delete(deleteQuestion);
        }

        public void Delete(Question question)
        {
            // need to delete all answers associated with question
            // TODO EF doesn't automatically batch, refactor to use batch or use cascade
            var answerRepositoy = new AnswerRepository(_dbUnitOfWork);
            var deleteAnswers = answerRepositoy.GetAnswersForQuestion(question.QuestionId, false);
            foreach(var deleteAnswer in deleteAnswers)
            {
                answerRepositoy.Delete(deleteAnswer);
            }

            _dbUnitOfWork.DbContext.Entry(question).State = EntityState.Deleted;
            _dbUnitOfWork.SaveChanges();
        }
    }
}
