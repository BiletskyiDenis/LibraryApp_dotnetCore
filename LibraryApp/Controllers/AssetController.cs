using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using LibraryApp.Extentions;
using LibraryApp.Models;
using LibraryApp.Models.Catalog;
using LibraryData;
using LibraryData.Models;
using LibraryServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Asset")]
    public class AssetController : Controller
    {
        protected readonly ILibraryDataService _libraryDataService;

        public AssetController(ILibraryDataService libraryDataService)
        {
            this._libraryDataService = libraryDataService;
        }

        [HttpGet("RecentlyAdded/{type}")]
        public IActionResult RecentlyAdded(string type)
        {
            AssetType assetType;
            Enum.TryParse(type, true, out assetType);
            var asset = _libraryDataService.GetAssetsFromType(assetType);

            return Json(asset.DtoRecentlyAdded());
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var asset = _libraryDataService.GetAsset(id);

            if (asset == null)
            {
                return NotFound();
            }

            var dto = asset.Dto<DetailViewModel>();
            dto.Type = _libraryDataService.GetType(id).ToString().ToLower();

            return Json(dto);
        }

        // GET: api/Asset
        [HttpGet]
        public IActionResult Get()
        {
            var assets = _libraryDataService.GelAllAssets();
            return Json(assets);
        }
    }
}