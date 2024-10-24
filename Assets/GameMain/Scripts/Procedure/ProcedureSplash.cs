using Framework;
using UnityEngine;

namespace GameMain
{
    public class ProcedureSplash : ProcedureBase
    {
        public override void OnEnter()
        {
            base.OnEnter();
            var animator = GameObject.Find("SplashAnim").GetComponent<Animator>();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void SplashAnimEnd()
        {
            
        }
    }
}