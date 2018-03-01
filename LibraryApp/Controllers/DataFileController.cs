using System;
using System.Text.RegularExpressions;
using BusinessLogic.services;
using LibraryApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api")]
    public class DataFileController : Controller
    {
        protected readonly ILibraryDataService _libraryDataService;
        protected readonly IDataFileService _dataFileService;

        public DataFileController(ILibraryDataService libraryDataService, IDataFileService dataFileService)
        {
            this._libraryDataService = libraryDataService;
            this._dataFileService = dataFileService;
        }

        [HttpPost("DownloadData")]
        public IActionResult DownloadData([FromBody]DownloadFileModel downloadFile)
        {
            var asset = _libraryDataService.GetAsset(downloadFile.Id);
            if (asset == null)
            {
                return NotFound();
            }

            byte[] fileData = _dataFileService.TryGetFile(asset, downloadFile.Type);

            if (fileData == null)
            {
                return NotFound();
            }

            var fileName = Regex.Replace(asset.Title, @"[^a-zA-z0-9]+", String.Empty) + "." + downloadFile.Type;
            return File(fileData, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
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

            var tmpAssets = _libraryDataService.GetSelected(selectedItems.Id);

            var fileData = new byte[0];

            fileData = _dataFileService.TryGetListDataFile(tmpAssets, selectedItems.Type);

            if (fileData == null)
            {
                return BadRequest();
            }

            var fileName = "ListData." + selectedItems.Type;

            return File(fileData, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost("UploadData")]
        public IActionResult Post(IFormFileCollection file)
        {
            if (file.Count == 0)
            {
                return BadRequest();
            }

            try
            {
                _dataFileService.RestoreDataFromFile(file,_libraryDataService);
            }

            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}