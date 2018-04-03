using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IlibraryRepository Library { get; }
        int Complete();
    }
}
