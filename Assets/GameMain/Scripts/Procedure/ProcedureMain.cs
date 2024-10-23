using Framework;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        private const int LEVELMAX = 3;
        
        private int _levelIndex;
        public override void OnEnter()
        {
            base.OnEnter();
            _levelIndex = Owner.GetValue<int>("Level");
            AGameManager.Instance.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
            AGameManager.Instance.OnExit();
        }

        public void LevelWin()
        {
            if (_levelIndex < LEVELMAX)
            {
                _levelIndex++;
                Owner.SetValue("Level",_levelIndex);
                Owner.SetValue("NextScene",$"Level{_levelIndex}");
                ChangeState<ProcedureChangeScene>();
            }
            else
            {
                //TODO:End
            }
        }

        public void PlayerDie()
        {
            Owner.SetValue("NextScene",$"Level{_levelIndex}");
            ChangeState<ProcedureChangeScene>();
        }
    }
}