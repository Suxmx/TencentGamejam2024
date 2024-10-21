using UnityEngine;

namespace GameMain
{
    public  class SettingPage : MonoBehaviour
    {
        public virtual void Init()
        {
            
        }

        public virtual void Open()
        {
            RegisterEvents();
            gameObject.SetActive(true);
            
        }

        public virtual void Close()
        {
            RemoveEvents();
            gameObject.SetActive(false);
        }

        protected virtual void RegisterEvents(){}
        protected virtual void RemoveEvents(){}
    }
}