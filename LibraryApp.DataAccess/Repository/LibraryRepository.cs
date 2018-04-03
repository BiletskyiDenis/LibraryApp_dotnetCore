using DataAccess.Context;
using Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repository
{
    public class LibraryRepository : Repository<LibraryAsset>, IlibraryRepository
    {
        public LibraryRepository(LibraryContext context) : base(context)
        {
        }

        public IEnumerable<Book> GetBooks()
        {
            return LibraryContext.Books.ToList();
        }

        public IEnumerable<Brochure> GetBrochures()
        {
            return LibraryContext.Brochures.ToList();
        }

        public LibraryAsset GetById(int? id)
        {
            return GetAll().FirstOrDefault(asset => asset.Id == id);
        }

        public IEnumerable<Journal> GetJournals()
        {
            return LibraryContext.Journals.ToList();
        }

        public LibraryContext LibraryContext
        {
            get { return _libraryContext as LibraryContext; }
        }
    }
}
