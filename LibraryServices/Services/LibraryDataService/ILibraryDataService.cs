using LibraryData.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LibraryServices
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
