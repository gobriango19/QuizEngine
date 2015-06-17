using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;

namespace QuizEngine.Data.DbContextStateManagers
{
    public class HttpDbContextStateManager<TDbContext> : BaseDbContextStateManager<TDbContext>
        where TDbContext : DbContext, new()
    {
        public override DbContextState<TDbContext> AmbientDbContextState
        {
            get
            {
                return HttpContext.Current.Items[ContextKey] as DbContextState<TDbContext>;
            }
            set
            {
                HttpContext.Current.Items[ContextKey] = value;
            }
        }
    }
}
