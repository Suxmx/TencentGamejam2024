using MyStateMachine;
using UnityEngine;

namespace Framework
{
    public class ProcedureBase
    {
        public ProcedureFsm Owner;

        public virtual void OnEnter()
        {
            Debug.Log($"Enter {this.GetType()}");
        }

        public virtual void OnLogic(float deltaTime)
        {
        }

        public virtual void OnExit()
        {
        }

        protected void ChangeState<T>() => Owner.ChangeState<T>();
    }
}