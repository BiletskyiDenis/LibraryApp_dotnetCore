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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    [Produces("application/json")]
    [Route("api/Asset")]
    public class AssetController : Controller
    {
        protected readonly ILibraryDataService _assets;

        public AssetController(ILibraryDataService assets)
        {
            this._assets = assets;
        }

        [HttpGet("RecentlyAdded/{type}")]
        public IActionResult RecentlyAdded(string type)
        {
            var assetType = type.ToLower();

            if (assetType == "book")
            {
                return Json(_assets.GetBooks().DtoRecentlyAdded());
            }

            if (assetType == "journal")
            {
                return Json(_assets.GetJournals().DtoRecentlyAdded());
            }

            if (assetType == "brochure")
            {
                return Json(_assets.GetBrochures().DtoRecentlyAdded());
            }

            return NotFound();
        }

        [HttpGet("Details/{id}")]
        public IActionResult Details(int id)
        {
            var asset = _assets.GetById(id);
            if (asset == null)
            {
                return NotFound();
            }
            var type = _assets.GetType(id).ToString().ToLower();

            var dto = asset.Dto<DtoDetailModel>();
            dto.Type = type;
            return Json(dto);
        }

        // GET: api/Asset
        [HttpGet]
        public IActionResult Get()
        {
            var assetModels = _assets.GetAll();
            var listingResult = assetModels.Select(
                result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    Author = _assets.GetAuthor(result.Id),
                    ImageUrl = result.ImageUrl,
                    Price = result.Price,
                    NumberOfCopies = result.NumbersOfCopies,
                    Type = _assets.GetType(result.Id).ToString(),
                    Publisher = result.Publisher
                });

            return Json(listingResult);
        }
    }
}