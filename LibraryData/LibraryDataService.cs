using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryData
{
    public class LibraryDataService : ILibraryDataService
    {
        private LibraryContext context;
        public LibraryDataService(LibraryContext context)
        {
            this.context = context;
        }

        public void AddAsset(LibraryAsset asset)
        {
            context.LibraryAssets.Add(asset);
        }

        public bool DeleteAsset(LibraryAsset asset)
        {
            if (asset == null)
            {
                return false;
            }

            context.LibraryAssets.Remove(asset);

            return true;
        }

        public IEnumerable<LibraryAsset> GetAll()
        {
            return context.LibraryAssets.ToList();
        }

        public IEnumerable<string> GetAllTypes()
        {
            return Enum.GetNames(typeof(AssetType));
        }

        public string GetAuthor(int id)
        {
            var isBook = context.LibraryAssets.OfType<Book>()
                                .Where(d => d.Id == id).Any();

            if (isBook)
            {
                return context.Books.FirstOrDefault(b => b.Id == id).Author;
            }

            return string.Empty;
        }

        public IEnumerable<Book> GetBooks()
        {
            return context.Books.ToList();
        }

        public IEnumerable<Brochure> GetBrochures()
        {
            return context.Brochures.ToList();
        }

        public LibraryAsset GetById(int? id)
        {
            return GetAll().FirstOrDefault(asset => asset.Id == id);
        }

        public string GetFrequency(int id)
        {
            if (context.Journals.Any(j => j.Id == id))
            {
                return context.Journals.FirstOrDefault(j => j.Id == id).Frequency;
            }
            return string.Empty;
        }

        public string GetISBN(int id)
        {
            var isBook = context.LibraryAssets.OfType<Book>()
                                .Where(d => d.Id == id).Any();

            if (isBook)
            {
                return context.Books.FirstOrDefault(b => b.Id == id).ISBN;
            }

            return string.Empty;
        }

        public IEnumerable<Journal> GetJournals()
        {
            return context.Journals.ToList();
        }

        public int GetPages(int id)
        {
            var isBook = context.LibraryAssets.OfType<Book>()
                                .Where(d => d.Id == id).Any();

            if (isBook)
            {
                return context.Books.FirstOrDefault(b => b.Id == id).Pages;
            }

            return 0;
        }

        public IEnumerable<LibraryAsset> GetSelected(int[] selected)
        {
            return GetAll().Where(s => selected.Contains(s.Id)).ToList();
        }

        public AssetType GetType(int? id)
        {
            var isBook = context.LibraryAssets.OfType<Book>()
            .Where(d => d.Id == id);

            var isJournal = context.LibraryAssets.OfType<Journal>()
            .Where(d => d.Id == id);

            var isBrochure = context.LibraryAssets.OfType<Brochure>()
            .Where(d => d.Id == id);

            if (isBook.Any())
                return AssetType.Book;

            if (isJournal.Any())
                return AssetType.Journal;

            return AssetType.Brochure;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void UpdateAsset(LibraryAsset asset)
        {
            context.Entry(asset).State = EntityState.Modified;
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }
    }
}
