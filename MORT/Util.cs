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
        public static List<string> toolTipList = new List<string>();

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

        public static bool isInittoolTip = false;
        public static void ShowLog(string log)
        {
            Console.WriteLine(log.ToString());
        }

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
            if(!isInittoolTip)
            {
                toolTipList = new List<string>();
                string[] tool = Properties.Settings.Default.TOOLTIP_LIST.Split(',');

                for (int i = 0; i < tool.Length; i++)
                {
                    toolTipList.Add(tool[i]);
                    Console.WriteLine(tool[i]);
                }

                isInittoolTip = true;
            }
            


        

            string result = "";

            if(toolTipList.Count > 0)
            {
                Random r = new Random();
                int rand = r.Next(0, toolTipList.Count - 1);

                if (rand < toolTipList.Count)
                {
                    result = toolTipList[rand];
                }
            }
    

            return result;
        }


        
    }
    
}
