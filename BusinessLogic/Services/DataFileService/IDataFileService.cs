using System.Collections.Generic;
using System.Xml.Linq;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace BusinessLogic.services
{
    public interface IDataFileService
    {
        void RestoreDataFromFile(IFormFileCollection file, ILibraryDataService libraryDataService);
        byte[] TryGetFile<T>(T asset, string type) where T : LibraryAsset;
        byte[] TryGetListDataFile<T>(IEnumerable<T> assetList, string type) where T : LibraryAsset;
    }
}