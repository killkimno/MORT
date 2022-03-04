using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MORT.SettingManager;

namespace MORT
{
    public enum TranslateStatusType
    {
        None,
        Translate,
        Stop
    }
    public interface ITransform
    {
        TranslateStatusType TranslateStatusType { get; }
        bool UseTopMostOptionWhenTranslate { get;}
        void ApplyTopMost();
        void ApplyUseTopMostOptionWhenTranslate(bool useTopMostOptionWhenTranslate);
        void SetTopMost(bool topMost, bool useTopMostOptionWhenTranslate);
        void ForceUpdateText(string text);
        Skin GetSkinType();
        void ForceTransparency();
        void DoUpdate(bool isTranslating);

        void StartTrans();
        void StopTrans();
    }
}
