using System.Collections.Generic;
using System.Xml.Linq;
using LibraryData.Models;

namespace LibraryServices
{
    public interface IDataFileService
    {
        byte[] GetTXTFile<T>(T obj) where T : LibraryAsset;
        byte[] GetTXTListFile<T>(IEnumerable<T> obj) where T : LibraryAsset;
        byte[] GetXmlFile<T>(T obj) where T : LibraryAsset;
        byte[] GetXmlListFile<T>(IEnumerable<T> obj) where T : LibraryAsset;
        LibraryAsset RestoreAssetFromTxt(string doc);
        LibraryAsset RestoreAssetFromXml(XDocument doc);
        IEnumerable<LibraryAsset> RestoreAssetsListFromTxt(string doc);
        IEnumerable<LibraryAsset> RestoreAssetsListFromXml(XDocument doc);
        byte[] TryGetFile<T>(T obj, string type) where T : LibraryAsset;
        byte[] TryGetListDataFile<T>(IEnumerable<T> obj, string type) where T : LibraryAsset;
    }
}