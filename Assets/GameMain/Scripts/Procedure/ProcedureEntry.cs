using Framework;
using JSAM;

namespace GameMain
{
    public class ProcedureEntry : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            Owner.SetValue("NextScene", "Splash");
            ChangeState<ProcedureChangeScene>();
        }
    }
}