using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using LibraryData.Models;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace LibraryServices
{
    public class DataFileService : IDataFileService
    {
        public void RestoreDataFromFile(IFormFileCollection file, ILibraryDataService libraryDataService)
        {
            var fileExt = Path.GetExtension(file[0].FileName);

            using (var stream = new MemoryStream())
            {
                file[0].CopyTo(stream);
                byte[] data = stream.ToArray();

                if (fileExt == ".xml")
                {
                    RestoreAssetFromXml(data, libraryDataService);
                }
                else
                {
                    RestoreAssetFromTxt(data, libraryDataService);
                }
            }
        }

        public byte[] TryGetFile<T>(T obj, string type) where T : LibraryAsset
        {
            if (type == "xml")
            {
                return GetXmlFile(obj);
            }

            if (type == "txt")
            {
                return GetTXTFile(obj);
            }

            throw new Exception("Incorrect file type");
        }

        public byte[] TryGetListDataFile<T>(IEnumerable<T> obj, string type) where T : LibraryAsset
        {
            if (type == "xml")
            {
                return GetXmlListFile(obj);
            }

            if (type == "txt")
            {
                return GetTXTListFile(obj);
            }

            throw new Exception("Incorrect file type");
        }

        private IEnumerable<LibraryAsset> RestoreAssetsListFromXml(XDocument document)
        {
            if (document.Elements().FirstOrDefault().Name == null
                && document.Elements().FirstOrDefault().Name != "List")
            {
                throw new Exception("Incorrect file data");
            }

            return document.Elements().Elements().Select(s => RestoreFromXml(s));
        }

        private IEnumerable<LibraryAsset> RestoreAssetsListFromTxt(string document)
        {
            var data = document.Split('#').Where((s, i) => i > 0).ToArray();
            return data.Select(s => RestoreFromTxt(s));
        }

        private XElement GetXml<T>(T obj) where T : LibraryAsset
        {
            var type = obj.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var asset = new XElement(type.Name);

            foreach (var item in fields)
            {
                if (item.Name == "Id")
                {
                    continue;
                }
                asset.Add(new XElement(item.Name, item.GetValue(obj)));
            }
            return asset;
        }

        private byte[] GetXmlFile<T>(T obj) where T : LibraryAsset
        {
            var document = new XDocument();
            document.Add(GetXml(obj));
            return Encoding.UTF8.GetBytes(document.ToString());
        }

        private byte[] GetXmlListFile<T>(IEnumerable<T> obj) where T : LibraryAsset
        {
            var document = new XDocument();
            var list = new XElement("List");
            foreach (var item in obj)
            {
                list.Add(GetXml(item));
            }
            document.Add(list);
            return Encoding.UTF8.GetBytes(document.ToString());
        }

        private string GetTXT<T>(T obj) where T : LibraryAsset
        {
            var type = obj.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var builder = new StringBuilder();

            builder.Append($"#[{type.Name}]");
            builder.Append(Environment.NewLine);
            builder.Append(Environment.NewLine);

            foreach (var item in fields)
            {
                if (item.Name == "Id")
                {
                    continue;
                }

                builder.Append($"[{item.Name}]{Environment.NewLine}{item.GetValue(obj)}");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }

        private byte[] GetTXTFile<T>(T obj) where T : LibraryAsset
        {
            return Encoding.UTF8.GetBytes(GetTXT(obj));
        }

        private byte[] GetTXTListFile<T>(IEnumerable<T> obj) where T : LibraryAsset
        {
            var document = new StringBuilder();
            document.Append("[List]");
            document.Append(Environment.NewLine);

            foreach (var item in obj)
            {
                document.Append(GetTXT(item));
            }

            return Encoding.UTF8.GetBytes(document.ToString());
        }

        private LibraryAsset RestoreFromTxt(string data)
        {
            var assetData = data.Split('[', ']').Select(s => s.Replace(Environment.NewLine, string.Empty)).ToArray();
            var asset = GetAssetFromType(assetData[1]);

            if (asset == null)
            {
                throw new Exception("Incorrect file data");
            }

            var type = asset.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                SetFieldValueTxt(asset, field, assetData);
            }

            return asset;
        }

        private LibraryAsset RestoreFromXml(XElement document)
        {
            if (document.Name == null)
            {
                throw new Exception("Incorrect file structure");
            }

            var asset = GetAssetFromType(document.Name.ToString());

            if (asset == null)
            {
                throw new Exception("Incorrect file data");
            }

            var type = asset.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                if (field.Name == "Id")
                {
                    continue;
                }

                SetFieldValueXml(asset, field, document);
            }

            return asset;
        }

        private void SetFieldValueXml(LibraryAsset asset, PropertyInfo field, XElement xElement)
        {
            foreach (var item in xElement.Elements())
            {
                if (field.Name == item.Name)
                {
                    field.SetValue(asset, Convert.ChangeType(item.Value, field.PropertyType, CultureInfo.InvariantCulture));
                    break;
                }
            }
        }

        private void SetFieldValueTxt(LibraryAsset asset, PropertyInfo field, string[] assetData)
        {
            for (int i = 0; i < assetData.Length; i++)
            {
                if (field.Name != "Id" && field.Name == assetData[i])
                {
                    field.SetValue(asset, Convert.ChangeType(assetData[i + 1], field.PropertyType));
                    break;
                }
            }
        }

        private LibraryAsset GetAssetFromType(string type)
        {
            AssetType assetType;
            try
            {
                assetType = (AssetType)Enum.Parse(typeof(AssetType),type, true);
            }
            catch (Exception)
            {
                return null;
            }

            if (assetType == AssetType.Book)
            {
                return new Book();
            }

            if (assetType == AssetType.Brochure)
            {
                return new Brochure();
            }

            if (assetType == AssetType.Journal)
            {
                return new Journal();
            }

            return null;
        }

        private void RestoreAssetFromXml(byte[] data, ILibraryDataService libraryDataService)
        {
            using (var stream = new MemoryStream(data))
            {
                var xmlDocument = XDocument.Load(stream);
                if (xmlDocument.Elements().FirstOrDefault().Name == null)
                {
                    throw new Exception("Incorrect file format");
                }

                if (xmlDocument.Elements().FirstOrDefault().Name != "List")
                {
                    var asset = RestoreFromXml(xmlDocument.Elements().FirstOrDefault());

                    libraryDataService.AddAsset(asset);
                    libraryDataService.SaveChanges();

                    return;
                }

                var assets = RestoreAssetsListFromXml(xmlDocument);

                libraryDataService.AddAssetsRange(assets);
                libraryDataService.SaveChanges();
            }
        }

        private void RestoreAssetFromTxt(byte[] data, ILibraryDataService libraryDataService)
        {
            using (var stream = new MemoryStream(data))
            {
                var txtData = string.Empty;

                using (var reader = new StreamReader(stream))
                {
                    txtData = reader.ReadToEnd();
                }

                if (txtData.Substring(1, 4) != "List")
                {
                    var asset = RestoreFromTxt(txtData);

                    libraryDataService.AddAsset(asset);
                    libraryDataService.SaveChanges();

                    return;
                }

                var assets = RestoreAssetsListFromTxt(txtData);

                libraryDataService.AddAssetsRange(assets);
                libraryDataService.SaveChanges();
            }
        }
    }
}
