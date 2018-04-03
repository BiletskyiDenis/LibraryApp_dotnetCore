using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Repository
{
    public interface IlibraryRepository:IRepository<LibraryAsset>
    {
        IEnumerable<Book> GetBooks();
        IEnumerable<Journal> GetJournals();
        IEnumerable<Brochure> GetBrochures();
        LibraryAsset GetById(int? id);
    }
}
