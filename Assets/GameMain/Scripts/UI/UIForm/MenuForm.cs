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

        public override void OnOpen()
        {
            base.OnOpen();
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