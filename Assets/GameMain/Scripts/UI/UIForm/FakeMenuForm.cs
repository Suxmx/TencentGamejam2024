using Framework;

namespace GameMain
{
    public partial class FakeMenuForm : UGuiForm
    {
        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            RegisterEvents();
        }

        public override void OnClose()
        {
            base.OnClose();
            RemoveEvents();
        }

        private void RegisterEvents()
        {
            m_btn_newgame.onClick.AddListener(OnClickNewGame);
            m_btn_continueGame.onClick.AddListener(OnClickContinueGame);
            m_btn_setting.onClick.AddListener(OnClickSettings);
            m_btn_makers.onClick.AddListener(OnClickShowMakers);
        }

        private void RemoveEvents()
        {
            m_btn_newgame.onClick.RemoveListener(OnClickNewGame);
            m_btn_continueGame.onClick.RemoveListener(OnClickContinueGame);
            m_btn_setting.onClick.RemoveListener(OnClickSettings);
            m_btn_makers.onClick.RemoveListener(OnClickShowMakers);
        }

        private void OnClickNewGame()
        {
            
        }

        private void OnClickContinueGame()
        {
            
        }

        private void OnClickShowMakers()
        {
            
        }

        private void OnClickSettings()
        {
            GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
        }
    }
}