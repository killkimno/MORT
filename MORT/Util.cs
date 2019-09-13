using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MORT
{
    class Util
    {
        public static List<string> toolTipList = new List<string>()
        { "다중 모니터를 사용할 경우 모든 모니터의 dpi 설정값이 같아야 합니다",
          "WIN OCR은 윈도우10에서 사용할 수 있습니다",
          "이미지 보정에서 RGB로 추출은 폰트가 완벽한 단색일 때만 사용합니다",
          "원하는 OCR 영역을 설정 후 한 번만 추출하기 원하면 스냅샷을 이용하면 됩니다\r\n단축키(기본): ctrl+shift+A",
          "현재 설정된 OCR 영역을 한 번만 추출하기 원하면 한 번만 번역을 이용하면 됩니다\r\n단축키(기본): ctrl+shift+C",
          "OCR 영역을 빠르게 추가하고 싶을 땐 빠른 OCR 영역을 사용하시면 됩니다\r\n적용을 누르지 않아도 바로 추가되지만 설정에 저장되진 않습니다\r\n단축키(기본): ctrl+shift+X",
          "교정 사전을 이용한 추출한 문장을 자동으로 수정한 단어로 바꿀 수 있습니다\r\n단축키(기본): ctrl+shift+S",
          "단축키 설정은 도움말 탭에서 설정할 수 있습니다",
          "일본어를 추출할 땐 텍스트->부가설정->OCR 결과 공백 제거를 꼭 활성화해주셔야 합니다",
          "클립보드에 저장 기능을 사용하면 OCR 결과를 자동으로 클립보드에 저장합니다.\r\n다른 후커와 이용할 때 유용합니다",
          "여러분의 후원금은 저에게 큰 도움이 됩니다"
        };

        public static float dpiX = -1;
        public static float dpiY = -1;
        public static float dpiMulti = 1;
        public const float BASE_DPI = 96;

        public const int OCR_FORM_BORDER = 3;
        public const int OCR_FORM_SECOND_BORDER = 8;
        public const int OCR_FORM_TITLEBAR = 20;

        public static int ocrFormBorder = 3;
        public static int ocrformSecondBorder = 8;
        public static int ocrFormTitleBar = 20;

        public static void SetDPI(float dpiX, float dpiY)
        {
            Util.dpiX = dpiX;
            Util.dpiY = dpiY;

            dpiMulti = GetDpiMulti();

            ocrFormBorder = (int)(OCR_FORM_BORDER * dpiMulti);
            ocrformSecondBorder = (int)(OCR_FORM_SECOND_BORDER * dpiMulti);
            ocrFormTitleBar = (int)(OCR_FORM_TITLEBAR * dpiMulti);
        }

        public static int GetBorderWidth()
        {
            int result = 0;

            result = (int)(SystemInformation.FrameBorderSize.Width * GetDpiMulti());
          

            return result;
        }


        public static int GetTitlebarHeight()
        {
            int result = 0;

            result = (int)(( SystemInformation.CaptionHeight + GetBorderWidth()) * GetDpiMulti());
          
            
            return result;
        }

        public static float GetDpiMulti()
        {
            float result = 1;

            result = dpiX / BASE_DPI;
        
            return result;
        }

        //툴팁 관련.
        public static string GetToolTip()
        {
            string result = "";

            Random r = new Random();
            int rand = r.Next(0, toolTipList.Count -1);

            if(rand < toolTipList.Count)
            {
                result = toolTipList[rand];
            }

            return result;
        }


        
    }
    
}
