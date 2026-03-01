using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Service.Gemini
{
    public record GeminiConfigModel(GeminiVersionType VersionType, string ModelName, bool Pro);

    public enum GeminiVersionType
    {
        Gemini2x,
        Gemini3x,
        Other

    }
}
