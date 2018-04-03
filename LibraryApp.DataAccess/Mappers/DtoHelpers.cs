using Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ViewModel;

namespace DataAccess.Mappers
{
    public static class DtoHelpers
    {
        /// <summary>
        /// Data transfer object
        /// </summary>
        /// <typeparam name="T">Book, Journal, Brochure</typeparam>
        /// <param name="asset"></param>
        /// <returns></returns>
        public static T DtoMap<T>(this LibraryAsset asset) where T : class, new()
        {
            if (asset == null)
            {
                return null;
            }

            var dest = new T();
            var typeSource = asset.GetType();
            var typeDest = dest.GetType();
            var fieldsSource = typeSource.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fieldsDest = typeDest.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var fieldDest in fieldsDest)
            {
                foreach (var fieldSource in fieldsSource)
                {
                    if (fieldDest.Name == fieldSource.Name)
                    {
                        fieldDest.SetValue(dest, fieldSource.GetValue(asset));
                        break;
                    }
                }
            }

            return dest;
        }

        public static IEnumerable<T> DtoMap<T>(this IEnumerable<LibraryAsset> assetsList) where T : class, new()
        {
            var dtoObjects = assetsList.Select(s => s.DtoMap<T>());

            return dtoObjects;
        }

        public static IEnumerable<RecentlyAddedViewModel> DtoRecentlyAdded(this IEnumerable<LibraryAsset> assetsList, int recentlyAddedCount = 3, int maxChars = 300)
        {
            var dtoRecentlyAdded = assetsList.Reverse().Take(recentlyAddedCount).Select(s => new RecentlyAddedViewModel()
            {
                Id = s.Id,
                Title = s.Title,
                Description = SetMaxChars(s.Description, maxChars),
                ImageUrl = s.ImageUrl
            });

            return dtoRecentlyAdded;
        }
        private static string SetMaxChars(this string rawString, int maxChars)
        {
            if (rawString == null || rawString.Length < maxChars)
            {
                return rawString;
            }

            var resultString = rawString.Substring(0, maxChars) + "...";

            return resultString;
        }
    }

}
