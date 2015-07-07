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
    public class AnswerRepository : IAnswerRepository
    {
        private DbUnitOfWork<QuizEngineDbContext> _dbUnitOfWork;

        public AnswerRepository(DbUnitOfWork<QuizEngineDbContext> dbUnitOfWork)
        {
            _dbUnitOfWork = dbUnitOfWork;
        }

        public IEnumerable<Answer> GetAnswersForQuestion(long questionId, bool activeQuizOnly = true)
        {
            IEnumerable<Answer> answers = null;

            var questionRepository = new QuestionRepository(_dbUnitOfWork);
            var question = questionRepository.Get(questionId, activeQuizOnly);
            if(question != null)
            {
                answers = question.Answers.ToList();
            }

            return answers;
        }

        public Answer Get(long answerId)
        {
            return Get(answerId, true);
        }

        public Answer Get(long answerId, bool activeQuizOnly = true)
        {
            var query = from a in _dbUnitOfWork.DbContext.Answers
                        where a.AnswerId == answerId
                        select a;

            if(activeQuizOnly)
            {
                query = query.Where(a => a.Question.Quiz.IsActive);
            }

            return query.FirstOrDefault();
        }

        public Answer Add(Answer answer)
        {
            // confirm answerId doesn't exist
            if(answer.AnswerId != 0)
            {
                throw new ArgumentException("Provided answer is invalid for add: contains answerId.");
            }

            // confirm question exists
            var questionRepository = new QuestionRepository(_dbUnitOfWork);
            var question = questionRepository.Get(answer.QuestionId, false);
            if (question == null)
            {
                throw new ArgumentException("Provided answer is invalid for add: questionId is an invalid reference.");
            }
            _dbUnitOfWork.DbContext.Entry(question).State = EntityState.Detached; // we don't need to track this, so let's detach it

            answer.CreateDate = DateTime.UtcNow;
            answer.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Answers.Add(answer);
            _dbUnitOfWork.SaveChanges();

            return answer;
        }

        public Answer Update(Answer answer)
        {
            // confirm answerId exists
            if(answer.AnswerId == 0)
            {
                throw new ArgumentException("Provided answer is invalid for update: missing answerId.");
            }

            // confirm answer exists in the DB
            var checkAnswer = Get(answer.AnswerId, false);
            if(checkAnswer == null)
            {
                throw new ArgumentException("Provided answer is invalid for update: cannot find answer in database.");
            }

            // confirm question exists if it has been updated
            if (checkAnswer.QuestionId != answer.QuestionId)
            {
                var questionRepository = new QuestionRepository(_dbUnitOfWork);
                var question = questionRepository.Get(answer.QuestionId, false);
                if (question == null)
                {
                    throw new ArgumentException("Provided answer is invalid for add: questionId is an invalid reference.");
                }
                _dbUnitOfWork.DbContext.Entry(question).State = EntityState.Detached; // we don't need to track this, so let's detach it
            }
            _dbUnitOfWork.DbContext.Entry(checkAnswer).State = EntityState.Detached; // detach the looked up answer so there is no conflict with the updated answer

            answer.UpdateDate = DateTime.UtcNow;

            _dbUnitOfWork.DbContext.Entry(answer).State = EntityState.Modified;
            _dbUnitOfWork.DbContext.Entry(answer).Property(a => a.CreateDate).IsModified = false; // never change the created date on update
            _dbUnitOfWork.SaveChanges();

            _dbUnitOfWork.DbContext.Entry(answer).Reload();

            return answer;
        }

        public void Delete(long answerId)
        {
            // confirm the answer exists in the DB
            var deleteAnswer = Get(answerId, false);
            if(deleteAnswer == null)
            {
                throw new ArgumentException("Provided answerId is invalid for deletion: cannot find answer in database.");
            }

            Delete(deleteAnswer);
        }

        public void Delete(Answer answer)
        {
            _dbUnitOfWork.DbContext.Entry(answer).State = EntityState.Deleted;
            _dbUnitOfWork.SaveChanges();
        }
    }
}
