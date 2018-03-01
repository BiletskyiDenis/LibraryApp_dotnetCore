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

        public string GetAuthor(int id)
        {
            var asset = LibraryContext.Books.Where(b => b.Id == id).FirstOrDefault();

            if (asset == null)
            {
                return string.Empty;
            }

            return asset.Author;
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

        public string GetFrequency(int id)
        {
            var asset = LibraryContext.Journals.Where(j => j.Id == id).FirstOrDefault();
            if (asset == null)
            {
                return string.Empty;
            }
            return asset.Frequency;
        }

        public string GetISBN(int id)
        {
            var asset = LibraryContext.Books.Where(j => j.Id == id).FirstOrDefault();
            if (asset == null)
            {
                return string.Empty;
            }
            return asset.ISBN;
        }

        public IEnumerable<Journal> GetJournals()
        {
            return LibraryContext.Journals.ToList();
        }

        public int GetPages(int id)
        {
            var asset = LibraryContext.Books.Where(j => j.Id == id).FirstOrDefault();
            if (asset == null)
            {
                return 0;
            }
            return asset.Pages;
        }

        public LibraryContext LibraryContext
        {
            get { return _libraryContext as LibraryContext; }
        }
    }
}
