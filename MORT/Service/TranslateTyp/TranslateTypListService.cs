using System.Collections.Generic;
using System.Linq;
using MORT.Model.TranslateType;

namespace MORT.Service.TranslateTyp;

public class TranslateTypListService
{
    private readonly List<TranslateTypeModel> _modelList = new List<TranslateTypeModel>();

    public List<string> GetTitles() => _modelList.Select(r => r.DisplayTitle).ToList();

    public void Initialize()
    {
        _modelList.Clear();

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE GOOGLE",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE GOOGLE"),
            SettingManager.TransType.google));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE DB",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE DB"),
            SettingManager.TransType.db));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE PAPAGO WEB",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE PAPAGO WEB"),
            SettingManager.TransType.papago_web));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE NAVER",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE NAVER"),
            SettingManager.TransType.naver));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE GOOGLE SHEET",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE GOOGLE SHEET"),
            SettingManager.TransType.google_url));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE DEEPL",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE DEEPL"),
            SettingManager.TransType.deepl));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE DEEPLAPI",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE DEEPLAPI"),
            SettingManager.TransType.deeplApi));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE GEMINI API",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE GEMINI API"),
            SettingManager.TransType.gemini));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE EZTRANS",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE EZTRANS"),
            SettingManager.TransType.ezTrans));

        _modelList.Add(new TranslateTypeModel(
            "TRANSLATE CUSTOM API",
            LocalizeManager.LocalizeManager.GetLocalizeString("TRANSLATE CUSTOM API"),
            SettingManager.TransType.customApi));
    }

    public SettingManager.TransType GetTransType(int index) => _modelList[index].TransType;
}