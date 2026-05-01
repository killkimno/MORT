using System;

namespace MORT.Model.TranslateType;

public record TranslateTypeModel(int Index, string Key, string DisplayTitle, SettingManager.TransType TransType);
