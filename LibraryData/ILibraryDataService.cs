using System.Collections.Generic;
using LibraryData.Models;

namespace LibraryData
{
    public interface ILibraryDataService
    {
        void AddAsset(LibraryAsset asset);
        bool DeleteAsset(LibraryAsset asset);
        void Dispose();
        IEnumerable<LibraryAsset> GetAll();
        IEnumerable<string> GetAllTypes();
        string GetAuthor(int id);
        IEnumerable<Book> GetBooks();
        IEnumerable<Brochure> GetBrochures();
        LibraryAsset GetById(int? id);
        string GetFrequency(int id);
        string GetISBN(int id);
        IEnumerable<Journal> GetJournals();
        int GetPages(int id);
        IEnumerable<LibraryAsset> GetSelected(int[] selected);
        AssetType GetType(int? id);
        void Save();
        void UpdateAsset(LibraryAsset asset);
    }
}