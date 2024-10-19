using UnityEngine;

namespace Framework
{
    public abstract class ManagerBase : MonoBehaviour,IManager
    {
        public abstract void OnEnter();
       
        public abstract void OnExit();
    }
}