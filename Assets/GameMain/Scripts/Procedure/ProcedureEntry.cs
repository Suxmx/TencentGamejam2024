using Framework;

namespace GameMain
{
    public class ProcedureEntry : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Owner.SetValue("NextScene", "Menu");
            ChangeState<ProcedureChangeScene>();
        }
    }
}