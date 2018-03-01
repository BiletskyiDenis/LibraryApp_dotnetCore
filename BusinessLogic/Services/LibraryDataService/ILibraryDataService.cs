using System.Collections.Generic;
using Domain.Enums;
using Domain.Models;
using ViewModel;

namespace BusinessLogic.services
{
    public interface ILibraryDataService
    {
        IEnumerable<AssetViewModel> GelAllAssets();
        LibraryAsset GetAsset(int id);
        IEnumerable<LibraryAsset> GetSelected(int[] selected);
        IEnumerable<LibraryAsset> GetAssetsFromType(AssetType type);
        void UpdateAsset(LibraryAsset asset);
        void AddAsset(LibraryAsset asset);
        void AddAssetsRange(IEnumerable<LibraryAsset> assets);
        void RemoveAsset(LibraryAsset asset);
        void RemoveAsset(int id);
        AssetType GetType(int? id);
        void SaveChanges();
    }
}
