using Framework;
using GameMain.Metagame;

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
            // WindowIconChanger.Instance.ChangeICON("icon1.ico");
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
            // if (!Owner.TryGetValue<int>("Level", out int level))
            // {
            //     level = 1;
            //     Owner.SetValue("Level",level);
            // }
            // Owner.SetValue("NextScene",$"Level{level}");
            Owner.SetValue("NextScene","NewMain");
            ChangeState<ProcedureChangeScene>();
        }
    }
}