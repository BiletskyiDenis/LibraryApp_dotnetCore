using System;
using BusinessLogic.services;
using DataAccess.Mappers;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using ViewModel;

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
            var asset = _libraryDataService.GetAssetsFromType(assetType).DtoRecentlyAdded();

            return Json(asset);
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var asset = _libraryDataService.GetAsset(id);

            if (asset == null)
            {
                return NotFound();
            }

            var dtoAsset = asset.DtoMap<DetailViewModel>();
            dtoAsset.Type = _libraryDataService.GetType(id).ToString().ToLower();

            return Json(dtoAsset);
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