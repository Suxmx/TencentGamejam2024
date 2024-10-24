using System.Collections.Generic;
using Framework;
using NUnit.Framework;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// EntityGroup枚举，根据实际需要情况添加
    /// </summary>
    public enum EEntityGroup
    {
        Bullet,
        Player,
        VFX
    }

    public class EntityGroup : MonoBehaviour
    {
        public EEntityGroup Type;
        private List<GameEntityBase> _activeEntities = new(); //已经初始化完毕正在活跃的entities
        private Dictionary<string, GameObjectPool> _goPools = new();
        private Queue<KeyValuePair<GameEntityBase, object>> _onShowQueue = new(); //用于间隔帧调用onshow

        public void OnUpdate(float deltaTime)
        {
            foreach (var entity in _activeEntities)
            {
                entity.OnUpdate(deltaTime);
            }
        }

        public void OnFixedUpdate(float deltaTime)
        {
            foreach (var entity in _activeEntities)
            {
                entity.OnFixedUpdate(deltaTime);
            }
        }

        public void OnLateUpdate(float deltaTime)
        {
            HandleInitingEntities();
            foreach (var entity in _activeEntities)
            {
                entity.OnLateUpdate(deltaTime);
            }
        }


        public T Spawn<T>(string name, Transform parent = null, object userData = null) where T : GameEntityBase
        {
            if (!_goPools.TryGetValue(name, out var pool))
            {
                GameObject prefab = GameEntry.Resource.Load<GameObject>(GameEntityManager.GetEntityPath(Type, name));
                pool = new GameObjectPool(prefab, transform);
                _goPools.Add(name, pool);
            }

            var go = pool.Spawn(parent);
            T entity = go.GetOrAddComponent<T>();
            entity.EntityName = name;
            entity.Group = Type;
            entity.SpawnByPool = true;
            // _activeEntities.Add(entity);
            WaitToOnShow(entity, userData);
            if(!entity.Inited)
            {
                entity.OnInit();
                entity.Inited = true;
            }
            // entity.OnShow(userData);
            return entity;
        }

        public void Unspawn(GameEntityBase entity)
        {
            entity.OnHide();
            if (!_activeEntities.Contains(entity)) return;
            _activeEntities.Remove(entity);
            if (entity.SpawnByPool)
            {
                var pool = _goPools[entity.EntityName];
                pool.Unspawn(entity.gameObject);
            }
            else
            {
                Destroy(entity);
            }
        }

        public void AttachToEntityLifeCycle(GameEntityBase entity)
        {
            entity.SpawnByPool = false;
            entity.OnInit();
            WaitToOnShow(entity, null);
        }

        private void WaitToOnShow(GameEntityBase entity, object data)
        {
            KeyValuePair<GameEntityBase, object> kvp = new KeyValuePair<GameEntityBase, object>(entity, data);
            _onShowQueue.Enqueue(kvp);
        }

        /// <summary>
        /// 延迟调用所有初始化中的entity onshow
        /// </summary>
        private void HandleInitingEntities()
        {
            while (_onShowQueue.Count > 0)
            {
                var kvp = _onShowQueue.Dequeue();
                var entity = kvp.Key;
                var data = kvp.Value;
                entity.OnShow(data);
                _activeEntities.Add(entity);
            }
        }

        protected void OnDestroy()
        {
        }
    }
}