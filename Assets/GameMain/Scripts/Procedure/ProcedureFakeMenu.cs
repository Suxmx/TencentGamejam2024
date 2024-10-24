using Framework;

namespace GameMain
{
    public class ProcedureFakeMenu : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            GameEntry.UI.OpenUIForm(UIFormId.FakeMenuForm);
        }

        public void EnterGame()
        {
            
        }
    }
}