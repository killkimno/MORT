using System;

namespace MORT.SettingData;

// 실제로 JSON에 저장/로드될 값(직렬화 대상)
// TODO : 프리셋 값은 메이커나 어디에서 따로 해야 한다
[Serializable]
public sealed class GeminiPresetValue
{
    public int Temperature { get; set; }
    public int ThinkingBudge { get; set; }
    public int TokenLimit { get; set; }
    public int TemperatureValue
    {
        get
        {
            if(PresetType == CustomPreset)
            {
                return Temperature;
            }
            return TemperaturePresetValue(PresetType);
        }
    }
    public int ThinkingBudgetValue
    {
        get
        {
            if(PresetType == CustomPreset)
            {
                return ThinkingBudge;
            }

            return ThinkingBudgetPresetValue(PresetType);
        }
    }
    public int TokenLimitValue
    {
        get
        {
            if(PresetType == CustomPreset)
            {
                return TokenLimit;
            }

            return TokenLimitPresetValue(PresetType);
        }
    }
    public int PresetType { get; set; }

    public const int DefaultPreset = 0;
    public const int LowPreset = 1;
    public const int CustomPreset = 2;

    public static int TokenLimitPresetValue(int presetType)
    {
        return presetType switch
        {
            DefaultPreset => 4000,
            LowPreset => 2000,
            _ => throw new ArgumentOutOfRangeException(nameof(presetType), "Invalid preset type")
        };
    }

    public static int ThinkingBudgetPresetValue(int presetType)
    {
        return presetType switch
        {
            DefaultPreset => 0,
            LowPreset => 1,
            _ => throw new ArgumentOutOfRangeException(nameof(presetType), "Invalid preset type")
        };
    }

    public static int TemperaturePresetValue(int presetType)
    {
        return presetType switch
        {
            DefaultPreset => 20,
            LowPreset => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(presetType), "Invalid preset type")
        };
    }

    // JsonSettingData<T> 제약(where T : class, new()) 때문에 필요
    public GeminiPresetValue()
    {
    }

    public GeminiPresetValue(int temperature, int thinkingBudget, int tokenLimit, int presetType)
    {
        Temperature = temperature;
        ThinkingBudge = thinkingBudget;
        TokenLimit = tokenLimit;
        PresetType = presetType;
    }
}