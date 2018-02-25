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
        void UpdateAsset(LibraryAsset asset, bool permanentSaveChanges = true);
        void AddAsset(LibraryAsset asset, bool permanentSaveChanges = true);
        void AddAssetsRange(IEnumerable<LibraryAsset> assets, bool permanentSaveChanges = true);
        void RemoveAsset(LibraryAsset asset, bool permanentSaveChanges = true);
        void RemoveAsset(int id, bool permanentSaveChanges = true);
        AssetType GetType(int? id);
        void SaveChanges();
    }
}
