using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class LibraryContext: DbContext
    {
        public LibraryContext(DbContextOptions options): base(options) {  }

        public DbSet<Book> Books { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Brochure> Brochures { get; set; }
        public DbSet<LibraryAsset> LibraryAssets { get; set; }
    }
}
