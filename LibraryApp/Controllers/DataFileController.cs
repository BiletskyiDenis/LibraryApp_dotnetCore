using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using LibraryApp.Models;
using LibraryData;
using LibraryServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class DataFileController : Controller
    {
        protected readonly ILibraryDataService _assets;
        protected readonly IDataFileService _fileData;

        public DataFileController(ILibraryDataService assets, IDataFileService fileData)
        {
            this._assets = assets;
            this._fileData = fileData;
        }

        [HttpPost("DownloadData")]
        public IActionResult DownloadData([FromBody]DownloadFileModel Dfile)
        {

            var asset = _assets.GetById(Dfile.Id);
            if (asset == null)
            {
                return NotFound();
            }

            byte[] downFile = _fileData.TryGetFile(asset, Dfile.Type);

            if (downFile == null)
            {
                return NotFound();
            }

            var fileName = Regex.Replace(asset.Title, @"[^a-zA-z0-9]+", String.Empty) + "." + Dfile.Type;
            return File(downFile, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost("DownloadSelected")]
        public IActionResult DownloadSelected([FromBody]DownloadSelectedModel selectedItems)
        {

            if (selectedItems == null || selectedItems.Id.Length == 0)
            {
                return BadRequest();
            }

            if (selectedItems.Id.Length == 1)
            {
                return DownloadData(new DownloadFileModel { Id = selectedItems.Id[0], Type = selectedItems.Type });
            }

            var tmpAssets = _assets.GetSelected(selectedItems.Id);

            var downFile = new byte[0];

            downFile = _fileData.TryGetListDataFile(tmpAssets, selectedItems.Type);

            if (downFile == null)
            {
                return BadRequest();
            }

            var fileName = "ListData." + selectedItems.Type;

            return File(downFile, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost("UploadData")]
        public  IActionResult Post(IFormFileCollection file)
        {
            if (file.Count == 0)
                return BadRequest();

            var fileExt = Path.GetExtension(file[0].FileName);

            try
            {
                using (var stream = new MemoryStream())
                {
                    file[0].CopyTo(stream);
                    byte[] data = stream.ToArray();

                    if (fileExt == ".xml")
                    {
                        RestoreAssetFromXml(data);
                    }
                    else
                    {
                        RestoreAssetFromTxt(data);
                    }

                }
            }
            catch (Exception e)
            {
                var rr = e;
                return BadRequest();
            }

            return Ok();
        }

        private void  RestoreAssetFromXml(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                var doc = XDocument.Load(stream);
                if (doc.Elements().FirstOrDefault().Name == null)
                    throw new Exception("Incorrect file format");

                if (doc.Elements().FirstOrDefault().Name == "List")
                {
                    var assets = _fileData.RestoreAssetsListFromXml(doc);
                    if (assets == null)
                        throw new Exception("Incorrect file data");

                    foreach (var item in assets)
                    {
                        if (item != null)
                        {
                            _assets.AddAsset(item);
                        }
                    }

                    _assets.Save();
                }
                else
                {
                    var asset = _fileData.RestoreAssetFromXml(doc);

                    if (asset == null)
                        throw new Exception("Incorrect file data");

                    _assets.AddAsset(asset);
                    _assets.Save();
                }
            }
        }

        private void RestoreAssetFromTxt(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                TextReader tr = new StreamReader(stream);
                var txtData = tr.ReadToEnd();
                if (txtData.Substring(1, 4) == "List")
                {
                    var assets = _fileData.RestoreAssetsListFromTxt(txtData);
                    if (assets == null)
                        throw new Exception("Incorrect file data");

                    foreach (var item in assets)
                    {
                        if (item != null)
                        {
                            _assets.AddAsset(item);
                        }
                    }

                    _assets.Save();
                }
                else
                {
                    var asset = _fileData.RestoreAssetFromTxt(txtData);
                    if (asset == null)
                        throw new Exception("Incorrect file data");

                    _assets.AddAsset(asset);
                    _assets.Save();
                }
            }
        }
    }
}