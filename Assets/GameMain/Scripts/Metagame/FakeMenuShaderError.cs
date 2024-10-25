using Framework;
using UnityEngine;

namespace GameMain
{
    public class FakeMenuShaderError : MonoBehaviour
    {
        public void OnAnimEnd()
        {
            (GameEntry.Procedure.CurrentProcedure as ProcedureFakeMenu).EnterGame();
        }
    }
}