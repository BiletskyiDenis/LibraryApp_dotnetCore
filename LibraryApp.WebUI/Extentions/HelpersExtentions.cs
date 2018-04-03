using Domain.Models;
using Newtonsoft.Json;

namespace LibraryApp.Extentions
{
    public static class HelpersExtentions
    {
        public static T DeserializeAsset<T>(this string data) where T : LibraryAsset
        {
            if (data == string.Empty)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
