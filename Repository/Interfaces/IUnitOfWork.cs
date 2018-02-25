using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface IUnitOfWork : IDisposable
    {
        IlibraryRepository Library { get; }
        int Complete();
    }
}
