using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MORT.SettingManager;

namespace MORT
{
    public interface ITransform
    {
        void ForceUpdateText(string text);
        Skin GetSkinType();
        void ForceTransparency();
        void DoUpdate(bool isTranslating);
    }
}
