using Framework;
using Framework.Develop;
using Services;
using UnityEngine;

namespace GameMain
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private bool _toMenu = false;

        public override void OnEnter()
        {
            base.OnEnter();
            //还原游戏时间
            Time.timeScale = 1;
            //关闭UI
            GameEntry.UI.CloseAllUIForms();
            //开始加载场景
            string nextScene = Owner.GetValue<string>("NextScene");
            _toMenu = string.CompareOrdinal(nextScene, "Menu") == 0;
            
            GameEntry.Event.Subscribe(OnAfterSceneLoadArgs.EventId,OnAfterLoadScene);
            GameEntry.Scene.LoadScene(nextScene);
        }

        public override void OnExit()
        {
            base.OnExit();
            GameEntry.Event.Unsubscribe(OnAfterSceneLoadArgs.EventId,OnAfterLoadScene);
        }

        private void OnAfterLoadScene(object sender,GameEventArgs e)
        {
            if (_toMenu)
            {
                ChangeState<ProcedureMenu>();
            }
            else
            {
                ChangeState<ProcedureMain>();
            }
        }
        
    }
}
