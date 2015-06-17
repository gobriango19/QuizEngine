using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Runtime.Remoting.Messaging;

namespace QuizEngine.Data.DbContextStateManagers
{
    class CallDbContextStateManager<TDbContext> : BaseDbContextStateManager<TDbContext>
        where TDbContext : DbContext, new()
    {
        public override DbContextState<TDbContext> AmbientDbContextState
        {
            get
            {
                return CallContext.GetData(ContextKey) as DbContextState<TDbContext>;
            }
            set
            {
                CallContext.SetData(ContextKey, value);
            }
        }
    }
}
