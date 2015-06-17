using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace QuizEngine.Data.DbContextStateManagers
{
    public abstract class BaseDbContextStateManager<TDbContext>
        where TDbContext : DbContext, new()
    {
        public const string ContextKeyFormat = "_DbContextState:_{0}_";

        protected string ContextKey
        {
            get
            {
                Type dbContextType = typeof(TDbContext);
                return string.Format(ContextKeyFormat, dbContextType.FullName);
            }
        }

        public abstract DbContextState<TDbContext> AmbientDbContextState { get; set; }
    }
}
