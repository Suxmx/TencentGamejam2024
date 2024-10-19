using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 所有用于游戏中的实体都继承自本类，由对象池管理
    /// </summary>
    public abstract class GameEntityBase : MonoBehaviour
    {
        [ReadOnly] public EEntityGroup Group;

        /// <summary>
        /// 从对象池生成出来时的名字，只读
        /// </summary>
        [ReadOnly] public string EntityName;

        [ReadOnly] public bool SpawnByPool;

        /// <summary>
        /// 仅在被Instantiate时会被调用
        /// </summary>
        public virtual void OnInit()
        {
        }

        /// <summary>
        /// 每次被从对象池中Spawn出来的时候都会被调用，在Oninit之后
        /// </summary>
        /// <param name="userData"></param>
        public virtual void OnShow(object userData)
        {
        }

        public virtual void OnUpdate(float deltaTime)
        {
        }

        public virtual void OnFixedUpdate(float deltaTime)
        {
        }

        public virtual void OnLateUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// 每次被Unspawn回对象池的时候会调用
        /// </summary>
        public virtual void OnHide()
        {
        }

        /// <summary>
        /// 被销毁时会调用
        /// </summary>
        public virtual void OnDestroy()
        {
        }
    }
}