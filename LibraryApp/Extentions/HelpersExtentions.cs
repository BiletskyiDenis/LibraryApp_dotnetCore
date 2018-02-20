using LibraryData.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryApp.Extentions
{
    public static class HelpersExtentions
    {

        public static T GetAsset<T>(this string data) where T : LibraryAsset
        {
            if (data==string.Empty)
                return null;

            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
