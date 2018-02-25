using System.Collections.Generic;
using System.Xml.Linq;
using LibraryData.Models;
using Microsoft.AspNetCore.Http;

namespace LibraryServices
{
    public interface IDataFileService
    {
        void RestoreDataFromFile(IFormFileCollection file, ILibraryDataService libraryDataService);
        byte[] TryGetFile<T>(T obj, string type) where T : LibraryAsset;
        byte[] TryGetListDataFile<T>(IEnumerable<T> obj, string type) where T : LibraryAsset;
    }
}