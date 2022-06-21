using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MORT.LocalizeManager;

namespace MORT
{
    public partial class Form1 : IGoogleBasicTranslateAPIContract
    {
        private void InitLocalize()
        {
            AppLanguage language = AppLanguage.Korea; // 어디선가 가져와야 한다
            CultureInfo ci = CultureInfo.InstalledUICulture;

            string result = Util.ParseStringFromFile(GlobalDefine.USER_OPTION_SETTING_FILE, "@APP_LANGUAGE ", '[', ']');

            if(string.IsNullOrEmpty(result))
            {
                result = ci.TwoLetterISOLanguageName;
            }
            language = LocalizeManager.LocalizeManager.ConvertAppLanguage(result);
            LocalizeManager.LocalizeManager.Init(Properties.Resources.localize, language);

            //TODO : 딴곳에 둬야한다
            GoogleBasicTranslateAPI.instance.InitContract(this);
            GoogleBasicTranslateAPI.instance.UpdateCondition();
            //this.lbTransType.Text = "fuck";
        }

        void IGoogleBasicTranslateAPIContract.UpdateCondition(string key)
        {
            lbBasicStatus.LocalizeLabel(key);
        }
    }

    public static class ComponenetLoclize
    {
        public static void LocalizeLabel(this System.Windows.Forms.Control control, string key)
        {
            if(control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text);
            }
        }
    }
}
