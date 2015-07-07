using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizEngine.Data.Interfaces
{
    public interface IRepository<TEntity, TKey> where TEntity : class
    {
        TEntity Get(TKey id);
        TEntity Add(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
    }
}
