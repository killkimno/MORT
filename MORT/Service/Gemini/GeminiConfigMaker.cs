using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MORT.Service.Gemini
{
    public class GeminiConfigMaker
    {
        public List<GeminiConfigModel> GeminiConfigModels { get; } = new();
        public GeminiConfigMaker()
        {
            InitalizeModels();
        }

        private void InitalizeModels()
        {
            GeminiConfigModels.Clear();
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Gemini2x, "gemini-2.5-flash-lite", false));
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Gemini2x, "gemini-2.5-flash", false));
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Gemini3x, "gemini-3-flash-preview", false));
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Gemini3x, "gemini-3.1-flash-lite-preview", false));
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Gemini3x, "gemini-3-flash-preview", false));
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Gemini2x, "gemini-2.5-pro", true));
            GeminiConfigModels.Add(new GeminiConfigModel(GeminiVersionType.Other, "custom", false));
        }

        public object CreateRequestBody(string modelName, string command, string requestText)
        {
            var cfg = GeminiConfigModels.First(r => r.ModelName == modelName);
            return CreateRequestBody(cfg, command, requestText);
        }

        public object CreateCustomRequestBody(string modelName, string command, string requestText)
        {
            // 1. modelName이 gemini-2로 시작하면 GeminiVersionType은 2x, 그 외에는 3x
            GeminiVersionType versionType;
            if(modelName.StartsWith("gemini-2", StringComparison.OrdinalIgnoreCase))
            {
                versionType = GeminiVersionType.Gemini2x;
            }
            else
            {
                versionType = GeminiVersionType.Gemini3x;
            }

            // 2. 모델 이름에서 pro가 포함되어 있으면 pro
            bool isPro = modelName.Contains("pro", StringComparison.OrdinalIgnoreCase);

            // 3. GeminiConfigModel 생성
            var cfg = new GeminiConfigModel(versionType, modelName, isPro);

            // 4. CreateRequestBody에 넣어 반환
            return CreateRequestBody(cfg, command, requestText);
        }

        public object CreateRequestBody(GeminiConfigModel cfg, string command, string requestText)
        {
            if(cfg == null) throw new ArgumentNullException(nameof(cfg));

            var geminiPreset = AdvencedOptionManager.GeminiPreset;
            float temperatureValue = geminiPreset.TemperatureValue / 100.0f;
            int thinkingBudgetValue = geminiPreset.ThinkingBudgetValue > 0 ? 512 : 0;
            int outputTokenLimit = geminiPreset.TokenLimitValue;

            var safetySettingsParms = new[]
{
                new { category = "HARM_CATEGORY_HARASSMENT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_HATE_SPEECH", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_SEXUALLY_EXPLICIT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" },
                new { category = "HARM_CATEGORY_CIVIC_INTEGRITY", threshold = "BLOCK_NONE" } // 선거/정치 관련 제한 완화
            };

            var systemInstruction = new
            {
                parts = new[] { new { text = command } }
            };


            // 2x 모델은 thinkingConfig 포함, 3x 모델은 thinkingConfig 제외
            if(cfg.VersionType == GeminiVersionType.Gemini2x)
            {
                var requestBody = new
                {
                    system_instruction = systemInstruction,
                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = requestText } } }
                    },
                    safetySettings = safetySettingsParms,
                    generationConfig = new
                    {
                        thinkingConfig = CreateThinkingConfigFor2x(geminiPreset.ThinkingBudgetValue, cfg.Pro),
                        temperature = temperatureValue,
                        maxOutputTokens = outputTokenLimit
                    }
                };
                return requestBody;
            }
            else if(cfg.VersionType == GeminiVersionType.Gemini3x)
            {
                var requestBody = new
                {
                    system_instruction = systemInstruction,
                    contents = new[]
                    {
                        new { role = "user", parts = new[] { new { text = requestText } } }
                    },
                    safetySettings = safetySettingsParms,
                    generationConfig = new
                    {
                        // 3x 계열은 thinkingConfig를 제외(모델/버전에 따라 다를 수 있음)
                        thinkingConfig = CreateThinkingConfigFor3x(geminiPreset.ThinkingBudgetValue, cfg.Pro),
                        temperature = temperatureValue,
                        maxOutputTokens = outputTokenLimit
                    }
                };
                return requestBody;
            }
            else
            {
                throw new NotSupportedException($"Unsupported GeminiVersionType: {cfg.VersionType}");
            }
        }

        private object CreateThinkingConfigFor2x(int rawThinking, bool pro)
        {
            // 0,2,3 -> 생략
            // 0 기본값 -모두 동적 사용
            // 1 최소값 - -1, pro는 512
            // 2 중간 / 3 최대 - 모두 동적 사고로 사용
            if(rawThinking == 0 || rawThinking == 2 || rawThinking == 3)
            {
                return new { include_thoughts = false };
            }

            int thinkingBudgetValue = -1;

            if(rawThinking == 1)
            {
                thinkingBudgetValue = pro ? 512 : 0;
            }

            return new { thinkingBudget = thinkingBudgetValue, include_thoughts = false };
        }

        private object CreateThinkingConfigFor3x(int rawThinking, bool pro)
        {
            // 0 - 기본값 모두 high다

            string thinkingLevel = rawThinking switch
            {
                0 => "HIGH",
                1 => pro ? "LOW" : "MINIMAL",
                2 => pro ? "LOW" : "MEDIUM",
                3 => "HIGH",
                _ => throw new ArgumentOutOfRangeException(nameof(rawThinking), $"Unexpected thinking level: {rawThinking}")
            };


            return new { thinkingLevel = thinkingLevel, include_thoughts = false };
        }


    }


}
