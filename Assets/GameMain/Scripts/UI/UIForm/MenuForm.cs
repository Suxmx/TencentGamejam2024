using Framework;

namespace GameMain
{
    public partial class MenuForm : UGuiForm
    {
        public override void OnInit()
        {
            base.OnInit();
            GetBindComponents(gameObject);
        }

        public override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_btn_start.onClick.AddListener(OnClickStart);
        }

        public override void OnClose()
        {
            base.OnClose();
            m_btn_start.onClick.RemoveListener(OnClickStart);
        }

        private void OnClickStart()
        {
            ((ProcedureMenu)GameEntry.Procedure.CurrentProcedure).EnterGame();
        }
    }
}