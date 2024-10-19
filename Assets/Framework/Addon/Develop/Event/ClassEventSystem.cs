using System;
using System.Collections.Generic;
using Services;
using UnityEngine;

namespace Framework.Develop
{
    /// <summary>
    /// 事件池模式。
    /// </summary>
    [Flags]
    public enum EventPoolMode : byte
    {
        /// <summary>
        /// 默认事件池模式，即必须存在有且只有一个事件处理函数。
        /// </summary>
        Default = 0,

        /// <summary>
        /// 允许不存在事件处理函数。
        /// </summary>
        AllowNoHandler = 1,

        /// <summary>
        /// 允许存在多个事件处理函数。
        /// </summary>
        AllowMultiHandler = 2,
    }

    public class ClassEventSystem : Service, IClassEventSystem
    {
        private readonly Dictionary<int, Action<object, GameEventArgs>> m_EventHandlers = new();
        private readonly Queue<Event> m_Events = new();
        [SerializeField] private EventPoolMode _eventPoolMode = EventPoolMode.Default;

        private void Update()
        {
            lock (m_Events)
            {
                while (m_Events.Count > 0)
                {
                    Event eventNode = m_Events.Dequeue();
                    HandleEvent(eventNode.Sender, eventNode.EventArgs);
                }
            }
        }

        private void HandleEvent(object sender, GameEventArgs args)
        {
            if (m_EventHandlers.TryGetValue(args.Id, out var action))
            {
                action?.Invoke(sender, args);
            }
        }

        private bool Check(int id,Action<object,GameEventArgs> handler)
        {
            if (!m_EventHandlers.ContainsKey(id)) return false;
            var actionList = m_EventHandlers[id];
            if (actionList == null || handler == null)
            {
                return false;
            }

            foreach (var existingHandler in actionList.GetInvocationList())
            {
                if (existingHandler == (Delegate)handler)
                {
                    return true;
                }
            }

            return false;
        }

        public void Subscribe(int id, Action<object, GameEventArgs> handler)
        {
            if (handler == null)
            {
                throw new Exception("Event handler is invalid.");
            }

            //前两种情况为没有没有过handler或是有过但是unsubscribe了
            if (!m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers.Add(id, handler);
            }
            else if (m_EventHandlers[id] is null)
            {
                m_EventHandlers[id] += handler;
            }
            else if ((_eventPoolMode & EventPoolMode.AllowMultiHandler) != EventPoolMode.AllowMultiHandler)
            {
                throw new Exception($"Event '{id}' not allow multi handler.");
            }
            else
            {
                m_EventHandlers[id] += handler;
            }
        }

        public void Unsubscribe(int id, Action<object, GameEventArgs> handler)
        {
            if (!m_EventHandlers.ContainsKey(id))
            {
                throw new Exception($"Event {id} doesn't exit");
            }

            if (!Check(id, handler))
            {
                throw new Exception($"Event {id} doesn't exit handler");
            }

            m_EventHandlers[id] -= handler;
        }

        public void Fire(object sender, GameEventArgs e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            Event eventNode = Event.Create(sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }

        public void FireNow(object sender, GameEventArgs e)
        {
            if (e == null)
            {
                throw new Exception("Event is invalid.");
            }

            HandleEvent(sender, e);
        }
    }
}