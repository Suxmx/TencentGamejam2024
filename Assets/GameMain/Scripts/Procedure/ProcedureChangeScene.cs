using Framework;
using Framework.Develop;
using Services;
using UnityEngine;

namespace GameMain
{
    public class ProcedureChangeScene : ProcedureBase
    {
        private bool _toMenu = false;
        public Animator LoaderAnim;

        public override void OnEnter()
        {
            base.OnEnter();
            if (LoaderAnim is null)
            {
                var obj = GameObject.Find("LoaderAnim");
                if (obj is not null) LoaderAnim = obj.GetComponent<Animator>();
            }


            //还原游戏时间
            Time.timeScale = 1;
            //关闭UI
            GameEntry.UI.CloseAllUIForms();
            //开始加载场景
            string nextScene = Owner.GetValue<string>("NextScene");
            _toMenu = string.CompareOrdinal(nextScene, "Menu") == 0;


            GameEntry.Event.Subscribe(OnAfterSceneLoadArgs.EventId, OnAfterLoadScene);

            //是否要播放转场动画
            if (!_toMenu)
            {
                GameEntry.Event.Subscribe(OnLoaderAnimStartArg.EventId, StartLoad);
                if (LoaderAnim is not null)
                {
                    LoaderAnim.Play("Start");
                }
                else
                {
                    StartLoad(null, null);
                }
            }
            else
            {
                StartLoad(null, null);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if (!_toMenu)
            {
                if (LoaderAnim is not null)
                {
                    LoaderAnim.Play("End");
                }

                GameEntry.Event.Unsubscribe(OnLoaderAnimStartArg.EventId, StartLoad);
            }

            GameEntry.Event.Unsubscribe(OnAfterSceneLoadArgs.EventId, OnAfterLoadScene);
        }

        private void StartLoad(object sender, GameEventArgs e)
        {
            string nextScene = Owner.GetValue<string>("NextScene");
            GameEntry.Scene.LoadScene(nextScene);
        }

        private void OnAfterLoadScene(object sender, GameEventArgs e)
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