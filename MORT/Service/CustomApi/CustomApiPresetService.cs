using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using MORT.Model.CustomApi;

namespace MORT.Service.CustomApi
{
    public class CustomApiPresetService
    {
        public List<CustomApiModel> BuiltInList { get; } = new();
        public List<CustomApiModel> AdditionalList { get; } = new();
        public List<CustomApiModel> FilePresetList { get; } = new();

        private readonly Dictionary<string, string> _filePresetSourcePaths = new(StringComparer.OrdinalIgnoreCase);
        private const string AdditionalFileName = "AdditionalList.txt";

        private readonly string _customApiDirectory;

        public CustomApiPresetService()
        {
            _customApiDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserData", "CustomApi");
            if(!Directory.Exists(_customApiDirectory))
            {
                Directory.CreateDirectory(_customApiDirectory);
            }

            LoadAdditionalFromFiles();
            LoadFilePresets();
        }

        // 추가 리스트를 파일 ./UserData/CustomApi/AdditionalList.txt 로부터 불러옵니다.
        // 경로에 폴더가 없으면 모두 생성하고, 파일이 없으면 빈 구조로 생성합니다.
        // 파일 내용은 CustomApiPresetListModel 형태의 JSON이어야 하며, Presets 목록을 AdditionalList에 채웁니다.
        public void LoadAdditionalFromFiles()
        {
            AdditionalList.Clear();

            try
            {
                // 디렉터리 보장
                Directory.CreateDirectory(_customApiDirectory);

                string filePath = Path.Combine(_customApiDirectory, AdditionalFileName);

                // 파일이 없으면 빈 모델로 생성 (Util.SaveFile 사용)
                if(!File.Exists(filePath))
                {
                    try
                    {
                        var empty = new CustomApiPresetListModel();
                        string emptyJson = JsonConvert.SerializeObject(empty, Formatting.Indented);
                        Util.SaveFile(filePath, emptyJson, false);
                    }
                    catch(Exception exCreate)
                    {
                        Util.ShowLog($"CustomApiPresetService: failed to create '{filePath}' - {exCreate.Message}");
                        // 생성 실패여도 계속해서 시도하여 읽기 시 예외 처리됨
                    }
                }

                // Util.OpenFile 로 파일 읽기
                string content = string.Empty;
                try
                {
                    using(var sr = Util.OpenFile(filePath))
                    {
                        if(sr == null)
                        {
                            Util.ShowLog($"CustomApiPresetService: OpenFile returned null for '{filePath}'");
                            return;
                        }

                        content = sr.ReadToEnd().Trim();
                        sr.Close();
                    }
                }
                catch(Exception exRead)
                {
                    Util.ShowLog($"CustomApiPresetService: failed to read '{filePath}' - {exRead.Message}");
                    return;
                }

                if(string.IsNullOrEmpty(content))
                {
                    // 빈이면 끝
                    return;
                }

                // JSON -> 모델 변환
                CustomApiPresetListModel listModel = null;
                try
                {
                    listModel = JsonConvert.DeserializeObject<CustomApiPresetListModel>(content);
                }
                catch(Exception exJson)
                {
                    Util.ShowLog($"CustomApiPresetService: failed to deserialize '{filePath}' - {exJson.Message}");
                    listModel = null;
                }

                if(listModel?.Presets != null)
                {
                    foreach(var m in listModel.Presets)
                    {
                        if(m != null)
                            AdditionalList.Add(m);
                    }
                }
            }
            catch(Exception ex)
            {
                Util.ShowLog($"CustomApiPresetService: LoadAdditionalFromFiles failed - {ex.Message}");
            }
        }

        // 추가 리스트를 저장합니다(입력 모델로 AdditionalList 교체 및 파일에 JSON 저장).
        // 저장은 Util.SaveFile을 사용합니다.
        public void SaveAdditionalList(IEnumerable<CustomApiModel> models)
        {
            if(models == null) throw new ArgumentNullException(nameof(models));

            try
            {
                Directory.CreateDirectory(_customApiDirectory);
                string filePath = Path.Combine(_customApiDirectory, AdditionalFileName);

                var listModel = new CustomApiPresetListModel();
                foreach(var m in models)
                {
                    if(m == null)
                    {
                        continue;
                    }
                    // 파일 기반 프리셋과 동일 이름은 AdditionalList.txt 에 저장하지 않음 (중복 방지 안전망)
                    if(_filePresetSourcePaths.ContainsKey(m.Name))
                    {
                        continue;
                    }
                    listModel.Presets.Add(m);
                }

                string json = JsonConvert.SerializeObject(listModel, Formatting.Indented);

                // Util.SaveFile은 append 옵션이 기본 false
                Util.SaveFile(filePath, json, false);

                // 메모리 리스트 교체
                AdditionalList.Clear();
                AdditionalList.AddRange(listModel.Presets);
            }
            catch(Exception ex)
            {
                Util.ShowLog($"CustomApiPresetService: SaveAdditionalList failed - {ex.Message}");
            }
        }

        // 강제 재로딩 API
        public void RefreshAdditional()
        {
            LoadAdditionalFromFiles();
            LoadFilePresets();
        }

        // 풀 목록 반환(내장 + 파일 기반 + 추가). 파일 기반과 동일 이름의 추가 리스트 항목은 제외.
        public IReadOnlyList<CustomApiModel> GetAllPresets()
        {
            var all = new List<CustomApiModel>(BuiltInList.Count + FilePresetList.Count + AdditionalList.Count);
            all.AddRange(BuiltInList);
            all.AddRange(FilePresetList);

            foreach(var m in AdditionalList)
            {
                if(m == null)
                {
                    continue;
                }
                if(_filePresetSourcePaths.ContainsKey(m.Name))
                {
                    continue;
                }
                all.Add(m);
            }
            return all.AsReadOnly();
        }

        // 이름으로 검색 (파일 기반 우선, 없으면 추가 리스트). null 가능
        public CustomApiModel? FindAdditionalByName(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return null;
            }

            foreach(var m in FilePresetList)
            {
                if(string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return m;
                }
            }
            foreach(var m in AdditionalList)
            {
                if(string.Equals(m.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return m;
                }
            }
            return null;
        }

        public bool IsFilePreset(string name)
        {
            if(string.IsNullOrEmpty(name))
            {
                return false;
            }
            return _filePresetSourcePaths.ContainsKey(name);
        }

        // UserData/CustomApi 폴더에서 AdditionalList.txt 외의 .txt 파일을 각각 단일 CustomApiModel 로 로드
        public void LoadFilePresets()
        {
            FilePresetList.Clear();
            _filePresetSourcePaths.Clear();

            try
            {
                Directory.CreateDirectory(_customApiDirectory);

                string[] files;
                try
                {
                    files = Directory.GetFiles(_customApiDirectory, "*.txt");
                }
                catch(Exception exList)
                {
                    Util.ShowLog($"CustomApiPresetService: failed to enumerate '{_customApiDirectory}' - {exList.Message}");
                    return;
                }

                foreach(var filePath in files)
                {
                    string fileName = Path.GetFileName(filePath);
                    if(string.Equals(fileName, AdditionalFileName, StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    string content = string.Empty;
                    try
                    {
                        using(var sr = Util.OpenFile(filePath))
                        {
                            if(sr == null)
                            {
                                Util.ShowLog($"CustomApiPresetService: OpenFile returned null for '{filePath}'");
                                continue;
                            }
                            content = sr.ReadToEnd().Trim();
                            sr.Close();
                        }
                    }
                    catch(Exception exRead)
                    {
                        Util.ShowLog($"CustomApiPresetService: failed to read '{filePath}' - {exRead.Message}");
                        continue;
                    }

                    if(string.IsNullOrEmpty(content))
                    {
                        continue;
                    }

                    CustomApiModel model = null;
                    try
                    {
                        model = JsonConvert.DeserializeObject<CustomApiModel>(content);
                    }
                    catch(Exception exJson)
                    {
                        Util.ShowLog($"CustomApiPresetService: failed to deserialize '{filePath}' - {exJson.Message}");
                        continue;
                    }

                    if(model == null || string.IsNullOrEmpty(model.Name))
                    {
                        continue;
                    }

                    if(_filePresetSourcePaths.ContainsKey(model.Name))
                    {
                        Util.ShowLog($"CustomApiPresetService: duplicated file preset name '{model.Name}' in '{filePath}' - skipped");
                        continue;
                    }

                    FilePresetList.Add(model);
                    _filePresetSourcePaths[model.Name] = filePath;
                }
            }
            catch(Exception ex)
            {
                Util.ShowLog($"CustomApiPresetService: LoadFilePresets failed - {ex.Message}");
            }
        }

        // 파일 기반 프리셋의 변경사항을 원본 파일에 다시 쓰기
        public void SaveFilePreset(CustomApiModel model)
        {
            if(model == null)
            {
                return;
            }
            if(!_filePresetSourcePaths.TryGetValue(model.Name, out var filePath))
            {
                Util.ShowLog($"CustomApiPresetService: SaveFilePreset - source path not found for '{model.Name}'");
                return;
            }

            try
            {
                string json = JsonConvert.SerializeObject(model, Formatting.Indented);
                Util.SaveFile(filePath, json, false);

                for(int i = 0; i < FilePresetList.Count; i++)
                {
                    if(string.Equals(FilePresetList[i].Name, model.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        FilePresetList[i] = model;
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Util.ShowLog($"CustomApiPresetService: SaveFilePreset failed for '{filePath}' - {ex.Message}");
            }
        }
    }
}