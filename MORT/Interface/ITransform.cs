using System;
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

        void ApplyWarningMessage(string message, DateTime dtRemainTime);
        void ClearWarningMessage();

        void ApplyRTL(bool enableRTL);

        int TaskIndex { get; }
    }
}
