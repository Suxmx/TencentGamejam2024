using Framework;
using GameMain.Metagame;

namespace GameMain
{
    public class ProcedureMenu : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            //打开UI
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm);
            // WindowIconChanger.Instance.ChangeICON("icon1.ico");
        }

        public override void OnExit()
        {
            base.OnExit();
            GameEntry.UI.CloseUIForm(UIFormId.MenuForm);
        }

        public void EnterGame()
        {
            Owner.SetValue("PlayerSpawnPoint", 1);
            Owner.SetValue("NextScene", "MainScene");
            ChangeState<ProcedureChangeScene>();
        }
    }
}