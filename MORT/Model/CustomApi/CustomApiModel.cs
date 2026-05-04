using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Model.CustomApi
{
    [Serializable]
    public record CustomApiModel(string Name, string Url, string Information, string Request, string Response, List<string> Headers);

    [Serializable]
    public class CustomApiPresetListModel
    {
        public List<CustomApiModel> Presets { get; set; } = new();

    }
}
