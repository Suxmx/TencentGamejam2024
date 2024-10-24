using Framework;
using UnityEngine;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void SplashAnimEnd()
        {
            Owner.SetValue("NextScene","FakeMenu");
            ChangeState<ProcedureChangeScene>();
        }
    }
}