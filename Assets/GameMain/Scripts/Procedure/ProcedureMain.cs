using Framework;
using GameMain.Metagame;
using UnityEngine.SceneManagement;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private const int LEVELMAX = 3;

        private int _levelIndex;

        public override void OnEnter()
        {
            base.OnEnter();
            // _levelIndex = Owner.GetValue<int>("Level");
            // WindowIconChanger.SetWindowIcon("Assets/Icon/icon2.ico");
            AGameManager.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
            AGameManager.Instance.OnExit();
        }

        public void LevelWin()
        {
            // if (_levelIndex < LEVELMAX)
            // {
            //     _levelIndex++;
            //     Owner.SetValue("Level", _levelIndex);
            //     Owner.SetValue("PlayerSpawnPoint", 1);
            //     Owner.SetValue("NextScene", $"Level{_levelIndex}");
            //     ChangeState<ProcedureChangeScene>();
            // }
            // else
            // {
            //     //TODO:End
            // }
            Owner.SetValue("NextScene", "EndScene");
            ChangeState<ProcedureChangeScene>();
        }

        public void PlayerDie()
        {
            Owner.SetValue("NextScene", SceneManager.GetActiveScene().name);
            ChangeState<ProcedureChangeScene>();
        }
    }
}