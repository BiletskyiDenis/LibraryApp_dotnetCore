using LibraryData;
using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class LibraryDataService : ILibraryDataService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LibraryDataService(LibraryContext context)
        {
            this._unitOfWork = new UnitOfWork(context);
        }

        public void AddAsset(LibraryAsset asset)
        {
            _unitOfWork.Library.Add(asset);
        }

        public void AddAssetsRange(IEnumerable<LibraryAsset> assets)
        {
            _unitOfWork.Library.AddRange(assets);
        }

        public IEnumerable<AssetViewModel> GelAllAssets()
        {
            var assetModels = _unitOfWork.Library.GetAll();
            var result = assetModels.Select(
                asset => new AssetViewModel
                {
                    Id = asset.Id,
                    Title = asset.Title,
                    Author = _unitOfWork.Library.GetAuthor(asset.Id),
                    ImageUrl = asset.ImageUrl,
                    Price = asset.Price,
                    NumberOfCopies = asset.NumbersOfCopies,
                    Type = GetType(asset.Id).ToString(),
                    Publisher = asset.Publisher
                });
            return result;
        }

        public IEnumerable<LibraryAsset> GetAssetsFromType(AssetType type)
        {
            if (type == AssetType.Book)
            {
                return _unitOfWork.Library.GetBooks();
            }

            if (type == AssetType.Brochure)
            {
                return _unitOfWork.Library.GetBrochures();
            }

            if (type == AssetType.Journal)
            {
                return _unitOfWork.Library.GetJournals();
            }
            return null;
        }

        public void RemoveAsset(LibraryAsset asset)
        {
            _unitOfWork.Library.Remove(asset);
        }

        public void RemoveAsset(int id)
        {
            var asset = _unitOfWork.Library.GetById(id);
            _unitOfWork.Library.Remove(asset);
        }

        public LibraryAsset GetAsset(int id)
        {
            return _unitOfWork.Library.GetById(id);
        }

        public void UpdateAsset(LibraryAsset asset)
        {
            _unitOfWork.Library.Update(asset);
        }

        public AssetType GetType(int? id)
        {
            var asset = _unitOfWork.Library.GetById(id);

            if (asset is Book)
            {
                return AssetType.Book;
            }

            if (asset is Journal)
            {
                return AssetType.Journal;
            }

            return AssetType.Brochure;
        }

        public IEnumerable<LibraryAsset> GetSelected(int[] selected)
        {
            return _unitOfWork.Library.GetAll().Where(s => selected.Contains(s.Id)).ToList();
        }

        public IEnumerable<string> GetAllTypes()
        {
            return Enum.GetNames(typeof(AssetType));
        }

        public void SaveChanges()
        {
            _unitOfWork.Complete();
        }

    }
}
