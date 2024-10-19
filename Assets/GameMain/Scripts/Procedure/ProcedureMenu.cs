using Framework;

namespace GameMain
{
    public class ProcedureMenu : ProcedureBase
    {
        private bool _enterGame;
        public override void OnEnter()
        {
            base.OnEnter();
            _enterGame = false;
            //打开UI
            GameEntry.UI.OpenUIForm(UIFormId.MenuForm);
        }

        public override void OnLogic(float deltaTime)
        {
            base.OnLogic(deltaTime);
        
            
            if (_enterGame)
            {
                OnPressEnterGame();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            GameEntry.UI.CloseUIForm(UIFormId.MenuForm);
        }

        public void EnterGame()
        {
            _enterGame = true;
        }
        
        private void OnPressEnterGame()
        {
            Owner.SetValue("NextScene","Main");
            ChangeState<ProcedureChangeScene>();
        }
    }
}