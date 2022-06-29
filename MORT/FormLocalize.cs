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
            CultureInfo ci = CultureInfo.CurrentUICulture;

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

        public static void LocalizeLabel(this System.Windows.Forms.Control control)
        {
            if (control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(control.Name, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(control.Name, control.Text);
            }
        }

        public static void LocalizeLabel(this System.Windows.Forms.Control control, System.Windows.Forms.Control target, string key, int left, int min = 0)
        {
            if (control.InvokeRequired)
            {

                control.BeginInvoke(new Action(() => control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text)));
            }
            else
            {
                control.Text = LocalizeManager.LocalizeManager.GetLocalizeString(key, control.Text);
            }
            control.Anchor(target, left, min);
        }

        public static void Anchor(this System.Windows.Forms.Control control, System.Windows.Forms.Control target, int left, int min = 0)
        {
            int result = target.Location.X + target.Size.Width + left;

            if(result < min)
            {
                result = min;
            }

            control.Location = new System.Drawing.Point(result, control.Location.Y);
        }
    }
}
