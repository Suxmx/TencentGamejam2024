using Framework;

namespace GameMain
{
    public class ProcedureMain : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            AGameManager.Instance.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
            AGameManager.Instance.OnExit();
        }
    }
}