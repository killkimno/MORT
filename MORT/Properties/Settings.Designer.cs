﻿//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 도구를 사용하여 생성되었습니다.
//     런타임 버전:4.0.30319.42000
//
//     파일 내용을 변경하면 잘못된 동작이 발생할 수 있으며, 코드를 다시 생성하면
//     이러한 변경 내용이 손실됩니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MORT.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.7.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1.259V")]
        public string MORT_VERSION {
            get {
                return ((string)(this["MORT_VERSION"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("MORT {0}\r\n레이어 번역창")]
        public string LAYER_TEXT {
            get {
                return ((string)(this["LAYER_TEXT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("1258")]
        public int MORT_VERSION_VALUE {
            get {
                return ((int)(this["MORT_VERSION_VALUE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"MORT를 처음 쓰시면 그 외-> MORT 사용법을 확인해 주세요,
OCR 영역에서 특정 부위만 추출을 제외하고 싶으면 제외 영역을 사용하시면 됩니다,
WIN OCR은 윈도우10에서 사용할 수 있습니다,
이미지 보정에서 RGB로 추출은 폰트가 완벽한 단색일 때만 사용합니다,
원하는 OCR 영역을 설정 후 한 번만 추출하기 원하면 스냅샷을 이용하면 됩니다
단축키(기본): ctrl+shift+A,
현재 설정된 OCR 영역을 한 번만 추출하기 원하면 한 번만 번역을 이용하면 됩니다
단축키(기본): ctrl+shift+C,
OCR 영역을 빠르게 추가하고 싶을 땐 빠른 OCR 영역을 사용하시면 됩니다
적용을 누르지 않아도 바로 추가되지만 설정에 저장되진 않습니다
단축키(기본): ctrl+shift+X,
교정 사전을 이용한 추출한 문장을 자동으로 수정한 단어로 바꿀 수 있습니다
단축키(기본): ctrl+shift+S,
단축키 설정은 그 외 탭에서 설정할 수 있습니다,
일본어를 추출할 땐 텍스트->부가설정->OCR 결과 공백 제거를 꼭 활성화해주셔야 합니다,
클립보드에 저장 기능을 사용하면 OCR 결과를 자동으로 클립보드에 저장합니다.
다른 후커와 이용할 때 유용합니다,
여러분의 후원금은 저에게 큰 도움이 됩니다,
토스로도 후원하실 수 있습니다,
사용자 설정 파일은 UserData 폴더에 있습니다,
부가설정 -> 설정 검색하기를 이용해 게임 설정을 검색할 수 있습니다,
강제 투명화 유지를 활성화 하면 한 번만 번역하기나 스냅샷 처리 후에도 번역창이 투명 상태로 유지됩니다
활성화 법 : 레이어 번역창 -> 오른쪽 클릭 -> 강제 투명화 유지,
설정 불러오기 단축키는 고급 설정에서 설정할 수 있습니다,
고급 설정에서 설정 불러오기 단축키, 개인 번역집을 설정할 수 있습니다,
고급 설정은 부가설정탭 -> 고급 설정에 있습니다,
고급 설정 -> 교정 사전 탭에서 교정사전 처리 횟수를 설정할 수 있습니다,
클립보드 텍스트를 번역하고 싶으면 고급 설정 -> 번역 설정 -> 클립보드에서 설정할 수 있습니다,
구글 OCR은 성능이 가장 좋으나 스냅샷 / 한 번만 번역하기에서만 이용할 수 있습니다,
구글 OCR의 사용량은 실제 사용량과 다를 수 있습니다. 수시로 구글 콘솔에서 확인하셔야 합니다")]
        public string TOOLTIP_LIST {
            get {
                return ((string)(this["TOOLTIP_LIST"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("추출한 OCR 문장 중 일부를 교정사전에 등록된 단어로 변경합니다.")]
        public string TOOLTIP_DIC {
            get {
                return ((string)(this["TOOLTIP_DIC"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("활성화시 완벽히 일치하는 단어만 교정합니다.\r\n\r\n예 : ocr 문장 - it possible to it poss  / 교정사전 : it poss" +
            " -> it home\r\n활성화 시 : it possible to it home\r\n비활성화 시 : it homeible to it home")]
        public string TOOLTIP_WORDDIC {
            get {
                return ((string)(this["TOOLTIP_WORDDIC"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("추출한 OCR문장과 번역 결과를 ocrResult.txt에 db형식으로 저장합니다")]
        public string TOOLTIP_OCRSAVE {
            get {
                return ((string)(this["TOOLTIP_OCRSAVE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("활성화시 번역창에 OCR문장을 표시합니다. 비활성화시 번역된 문장만 나옵니다.")]
        public string TOOLTIP_SHOW_OCR_RESULT {
            get {
                return ((string)(this["TOOLTIP_SHOW_OCR_RESULT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("추출한 OCR문장을 클립보드에 저장합니다")]
        public string TOOLTIP_CLIPBOARD {
            get {
                return ((string)(this["TOOLTIP_CLIPBOARD"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("추출할 문장의 폰트색이 완벽한 단색일 때 사용합니다.")]
        public string TOOLTIP_RGB {
            get {
                return ((string)(this["TOOLTIP_RGB"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("색 영역을 이용해 추출한 문장의 폰트색을 지정합니다.")]
        public string TOOLTIP_HSV {
            get {
                return ((string)(this["TOOLTIP_HSV"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2024 03 20")]
        public string MORT_RELEASE {
            get {
                return ((string)(this["MORT_RELEASE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DB 번역문 검색시 OCR 추출 문장에 포함 된 번역문을 모두 가져옵니다.")]
        public string TOOLTIP_MULTI_DB {
            get {
                return ((string)(this["TOOLTIP_MULTI_DB"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("\r\nMonkeyhead\'s OCR Realtime TransLate {0}\r\n제작자 : 몽키해드\r\n로고제작 : 김마손\r\n블로그 :\r\n몽키해드 : " +
            "https://blog.naver.com/killkimno\r\n김마손 : http://blog.naver.com/sabon2000\r\n       " +
            " ")]
        public string BASIC_TEXT {
            get {
                return ((string)(this["BASIC_TEXT"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OCR 영역")]
        public string UI_OCR_AREA_TITLE {
            get {
                return ((string)(this["UI_OCR_AREA_TITLE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("빠른 OCR 영역")]
        public string UI_OCR_QUICK_AREA {
            get {
                return ((string)(this["UI_OCR_QUICK_AREA"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("제외 영역")]
        public string UI_OCR_EXCEPTION_AREA_TITLE {
            get {
                return ((string)(this["UI_OCR_EXCEPTION_AREA_TITLE"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("단축키 실패! - 설정 파일이 없습니다 - {0}")]
        public string FAIL_HOTKEY_OPEN_SETTING_FILE {
            get {
                return ((string)(this["FAIL_HOTKEY_OPEN_SETTING_FILE"]));
            }
        }
    }
}
