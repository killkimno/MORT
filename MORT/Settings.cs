namespace MORT.Properties
{


    // 이 클래스를 사용하여 설정 클래스에 대한 특정 이벤트를 처리할 수 있습니다.
    //  SettingChanging 이벤트는 설정 값이 변경되기 전에 발생합니다.
    //  PropertyChanged 이벤트는 설정 값이 변경된 후에 발생합니다.
    //  SettingsLoaded 이벤트는 설정 값이 로드된 후에 발생합니다.
    //  SettingsSaving 이벤트는 설정 값이 저장되기 전에 발생합니다.
    internal sealed partial class Settings
    {

        public Settings()
        {
            // // 설정을 저장 및 변경하기 위한 이벤트 처리기를 추가하려면 아래 줄에서 주석 처리를 제거하십시오.
            //
            // this.SettingChanging += this.SettingChangingEventHandler;
            //
            // this.SettingsSaving += this.SettingsSavingEventHandler;
            //
        }

        private void SettingChangingEventHandler(object sender, System.Configuration.SettingChangingEventArgs e)
        {
            // SettingChangingEvent 이벤트를 처리하는 코드를 여기에 추가하십시오.
        }

        private void SettingsSavingEventHandler(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // SettingsSaving 이벤트를 처리하는 코드를 여기에 추가하십시오.
        }
    }
}
