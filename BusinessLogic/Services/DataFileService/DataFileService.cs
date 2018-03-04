using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml.Linq;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Domain.Models;
using Domain.Enums;

namespace BusinessLogic.services
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
                    return;
                }

                if (fileExt == ".txt")
                {
                    RestoreAssetFromTxt(data, libraryDataService);
                    return;
                }

                throw new Exception("Incorrect file type");
            }
        }

        public byte[] TryGetFile<T>(T asset, string type) where T : LibraryAsset
        {
            if (type == "xml")
            {
                var xmlFile = GetXmlFile(asset);
                return xmlFile;
            }

            if (type == "txt")
            {
                var txtFile = GetTXTFile(asset);
                return txtFile;
            }

            throw new Exception("Incorrect file type");
        }

        public byte[] TryGetListDataFile<T>(IEnumerable<T> assetsList, string type) where T : LibraryAsset
        {
            if (type == "xml")
            {
                var xmlFile = GetXmlListFile(assetsList);
                return xmlFile;
            }

            if (type == "txt")
            {
                var txtFile = GetTXTListFile(assetsList);
                return txtFile;
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
            var restoredAssets = document.Elements().Elements().Select(s => RestoreFromXml(s));

            return restoredAssets;
        }

        private IEnumerable<LibraryAsset> RestoreAssetsListFromTxt(string document)
        {
            var txtData = document.Split('#').Where((s, i) => i > 0).ToArray();
            var restoredAssets = txtData.Select(s => RestoreFromTxt(s));

            return restoredAssets;
        }

        private XElement GetXml<T>(T asset) where T : LibraryAsset
        {
            var type = asset.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var xmlAsset = new XElement(type.Name);

            foreach (var item in fields)
            {
                if (item.Name == "Id")
                {
                    continue;
                }
                xmlAsset.Add(new XElement(item.Name, item.GetValue(asset)));
            }
            return xmlAsset;
        }

        private byte[] GetXmlFile<T>(T asset) where T : LibraryAsset
        {
            var xmlDocument = new XDocument();
            xmlDocument.Add(GetXml(asset));
            var xmlFile = Encoding.UTF8.GetBytes(xmlDocument.ToString());
            return xmlFile;
        }

        private byte[] GetXmlListFile<T>(IEnumerable<T> assetsList) where T : LibraryAsset
        {
            var xmlDocument = new XDocument();
            var list = new XElement("List");
            foreach (var asset in assetsList)
            {
                list.Add(GetXml(asset));
            }
            xmlDocument.Add(list);
            var xmlFile = Encoding.UTF8.GetBytes(xmlDocument.ToString());
            return xmlFile;
        }

        private string GetTXT<T>(T asset) where T : LibraryAsset
        {
            var type = asset.GetType();
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

                builder.Append($"[{item.Name}]{Environment.NewLine}{item.GetValue(asset)}");
                builder.Append(Environment.NewLine);
                builder.Append(Environment.NewLine);
            }
            var txtFile = builder.ToString();
            return txtFile;
        }

        private byte[] GetTXTFile<T>(T asset) where T : LibraryAsset
        {
            var txtFile = Encoding.UTF8.GetBytes(GetTXT(asset));
            return txtFile;
        }

        private byte[] GetTXTListFile<T>(IEnumerable<T> assetsList) where T : LibraryAsset
        {
            var document = new StringBuilder();
            document.Append("[List]");
            document.Append(Environment.NewLine);

            foreach (var item in assetsList)
            {
                document.Append(GetTXT(item));
            }

            var txtFile = Encoding.UTF8.GetBytes(document.ToString());
            return txtFile;
        }

        private LibraryAsset RestoreFromTxt(string txtDocument)
        {
            var txtData = txtDocument.Split('[', ']').Select(s => s.Replace(Environment.NewLine, string.Empty)).ToArray();
            var asset = GetAssetFromType(txtData[1]);

            if (asset == null)
            {
                throw new Exception("Incorrect file data");
            }

            var type = asset.GetType();
            var fields = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                SetFieldValueTxt(asset, field, txtData);
            }

            return asset;
        }

        private LibraryAsset RestoreFromXml(XElement xmlDocument)
        {
            if (xmlDocument.Name == null)
            {
                throw new Exception("Incorrect file structure");
            }

            var asset = GetAssetFromType(xmlDocument.Name.ToString());

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

                SetFieldValueXml(asset, field, xmlDocument);
            }

            return asset;
        }

        private void SetFieldValueXml(LibraryAsset asset, PropertyInfo field, XElement xmlDocument)
        {
            foreach (var xElement in xmlDocument.Elements())
            {
                if (field.Name == xElement.Name)
                {
                    field.SetValue(asset, Convert.ChangeType(xElement.Value, field.PropertyType, CultureInfo.InvariantCulture));
                    break;
                }
            }
        }

        private void SetFieldValueTxt(LibraryAsset asset, PropertyInfo field, string[] txtData)
        {
            for (int i = 0; i < txtData.Length; i++)
            {
                if (field.Name != "Id" && field.Name == txtData[i])
                {
                    field.SetValue(asset, Convert.ChangeType(txtData[i + 1], field.PropertyType));
                    break;
                }
            }
        }

        private LibraryAsset GetAssetFromType(string type)
        {
            AssetType assetType;
            try
            {
                assetType = (AssetType)Enum.Parse(typeof(AssetType), type, true);
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
