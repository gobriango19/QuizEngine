using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web;
using QuizEngine.Utilities.BaseClasses;
using QuizEngine.Data.DbContextStateManagers;

namespace QuizEngine.Data
{
    public class DbUnitOfWork<TDbContext> : BaseDisposable
        where TDbContext : DbContext, new()
    {
        private static BaseDbContextStateManager<TDbContext> _dbContextManager = InitDbContextStateManager();
        private bool _isRoot = false;

        private static BaseDbContextStateManager<TDbContext> InitDbContextStateManager()
        {
            Type type = typeof(CallDbContextStateManager<>);
            if(HttpContext.Current != null)
            {
                type = typeof(HttpDbContextStateManager<>);
            }
            return (BaseDbContextStateManager<TDbContext>)Activator.CreateInstance(type.MakeGenericType(typeof(TDbContext)));
        }

        private DbContextState<TDbContext> AmbientDbContextState
        {
            get
            {
                return _dbContextManager.AmbientDbContextState;
            }
            set
            {
                _dbContextManager.AmbientDbContextState = value;
            }
        }

        public TDbContext DbContext
        {
            get
            {
                return AmbientDbContextState.DbContext;
            }
        }

        public DbUnitOfWork()
        {
            if(AmbientDbContextState == null)
            {
                DbContextState<TDbContext> dbContextState = new DbContextState<TDbContext>();
                AmbientDbContextState = dbContextState;
                _isRoot = true;
            }
        }

        public void SaveChanges()
        {
            if(!_isRoot)
            {
                AmbientDbContextState.NeedToSaveChanges = true;
                return;
            }

            AmbientDbContextState.SaveChangesAllowed = true;
            AmbientDbContextState.DbContext.SaveChanges();
            AmbientDbContextState.SaveChangesAllowed = false;
            AmbientDbContextState.NeedToSaveChanges = false;
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                if(AmbientDbContextState != null && _isRoot)
                {
                    if(AmbientDbContextState.NeedToSaveChanges)
                    {
                        AmbientDbContextState.SaveChangesAllowed = true;
                        AmbientDbContextState.DbContext.SaveChanges();
                        AmbientDbContextState.SaveChangesAllowed = false;
                        AmbientDbContextState.NeedToSaveChanges = false;
                    }
                    AmbientDbContextState.Dispose();
                    AmbientDbContextState = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}
