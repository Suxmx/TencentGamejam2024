using System;
using Framework;
using Framework.Develop;
using Sirenix.OdinInspector;
using Tencent.Args;
using UnityEngine;

namespace Tencent
{
    /// <summary>
    /// 可被压力板激活的物体
    /// </summary>
    public abstract class PressureActiveObj : MonoBehaviour
    {
        [SerializeField, LabelText("开关Key")] private string _triggerKey;
        [SerializeField, LabelText("初始状态")] protected bool _initState;


        protected virtual void OnEnable()
        {
            GameEntry.Event.Subscribe(OnPressurePlateStateChangeArg.EventId,OnPressurePlateTriggered);
            InitState(_initState);
        }

        protected virtual void OnDisable()
        {
            GameEntry.Event.Unsubscribe(OnPressurePlateStateChangeArg.EventId,OnPressurePlateTriggered);
        }

        /// <summary>
        /// 用于接受事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="arg"></param>
        private void OnPressurePlateTriggered(object sender, GameEventArgs arg)
        {
            var e = (OnPressurePlateStateChangeArg)arg;
            Debug.Log("trigger");
            if (e.TriggerKey != _triggerKey) return;
            OnTriggerStateChange(e.Enable);
        }

        protected abstract void InitState(bool enable);

        protected abstract void OnTriggerStateChange(bool enable);
    }
}