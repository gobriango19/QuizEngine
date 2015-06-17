using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizEngine.Utilities.BaseClasses
{
    public abstract class BaseDisposable : IDisposable
    {
        private bool Disposed { get; set; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                Disposed = true;
            }
        }

        ~BaseDisposable()
        {
            Dispose(false);
        }
    }
}
