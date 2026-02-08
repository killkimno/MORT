using System;

namespace MORT.SettingData;

// 실제로 JSON에 저장/로드될 값(직렬화 대상)
[Serializable]
public sealed class GeminiPresetValue
{
    public int Temperature { get; set; }
    public int ThinkingBudget { get; set; }
    public int TokenLimit { get; set; }

    // JsonSettingData<T> 제약(where T : class, new()) 때문에 필요
    public GeminiPresetValue()
    {
    }

    public GeminiPresetValue(int temperature, int thinkingBudget, int tokenLimit)
    {
        Temperature = temperature;
        ThinkingBudget = thinkingBudget;
        TokenLimit = tokenLimit;
    }
}