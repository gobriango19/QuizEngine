using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using QuizEngine.Utilities.BaseClasses;

namespace QuizEngine.Data
{
    public class DbContextState<TDbContext> : BaseDisposable
        where TDbContext : DbContext, new()
    {
        internal bool SaveChangesAllowed { get; set; }

        public TDbContext DbContext { get; private set; }

        public bool NeedToSaveChanges { get; set; }

        private void PreventSaveChanges(object sender, EventArgs eventArgs)
        {
            if(!SaveChangesAllowed)
            {
                throw new InvalidOperationException(
                    "Do not call SaveChanges() on a DbContext object owned by a DbContextState instance. " +
                    "Call DbUnitOfWork.SaveChanges() instead."
                    );
            }
        }

        public DbContextState()
        {
            TDbContext dbContext = new TDbContext();
            DbContext = dbContext;

            ((IObjectContextAdapter)DbContext).ObjectContext.SavingChanges
                += PreventSaveChanges;
            
            SaveChangesAllowed = false;
            NeedToSaveChanges = false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    ((IObjectContextAdapter)DbContext).ObjectContext.SavingChanges
                        -= PreventSaveChanges;

                    DbContext.Dispose();
                    DbContext = null;
                }
            }

            base.Dispose(disposing);
        }
    }
}
