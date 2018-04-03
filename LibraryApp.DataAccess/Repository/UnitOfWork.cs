using DataAccess.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryContext _libraryContext;

        public IlibraryRepository Library { get; private set; }

        public UnitOfWork(LibraryContext context)
        {
            this._libraryContext = context;
            this.Library = new LibraryRepository(context);
        }

        public int Complete()
        {
            return _libraryContext.SaveChanges();
        }

        public void Dispose()
        {
            _libraryContext.Dispose();
        }
    }
}
