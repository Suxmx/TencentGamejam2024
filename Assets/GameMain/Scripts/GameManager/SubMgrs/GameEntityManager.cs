using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.Develop;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 全局实体管理类，所有实体都应用这个类生成；需要提前添加prefab，并且设置为Addressable
    /// </summary>
    public class GameEntityManager : ManagerBase, IUpdatable
    {
        private static string EntityPrefabRootPath = "Assets/GameMain/Prefabs";

        public static string GetEntityPath(EEntityGroup group, string entityName)
        {
            return $"{EntityPrefabRootPath}/{group.ToString()}/{entityName}.prefab";
        }

        public bool Active
        {
            get => _active;
            set => _active = value;
        }

        private Dictionary<EEntityGroup, EntityGroup> _groups = new();
        private bool _active = true;

        public override void OnEnter()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            if (Active)
            {
                var keys = _groups.Keys.ToList();
                foreach (var key in keys)
                {
                    if (!_groups[key].enabled) continue;
                    _groups[key].OnUpdate(deltaTime);
                }
            }
        }

        public void OnFixedUpdate(float deltaTime)
        {
            if (Active)
            {
                var keys = _groups.Keys.ToList();
                foreach (var key in keys)
                {
                    if (!_groups[key].enabled) continue;
                    _groups[key].OnFixedUpdate(deltaTime);
                }
            }
        }

        public void OnLateUpdate(float deltaTime)
        {
            if (Active)
            {
                var keys = _groups.Keys.ToList();
                foreach (var key in keys)
                {
                    if (!_groups[key].enabled) continue;
                    _groups[key].OnLateUpdate(deltaTime);
                }
            }
        }

        public override void OnExit()
        {
        }

        /// <summary>
        /// 将不是由对象池创建的物体注册到对象池中管理生命周期
        /// </summary>
        public void AttachToEntityLifeCycle(GameEntityBase entity, EEntityGroup groupEnum)
        {
            EntityGroup group;
            if (!_groups.TryGetValue(groupEnum, out group))
            {
                group = CreateGroup(groupEnum);
            }

            group.AttachToEntityLifeCycle(entity);
        }

        public T Spawn<T>(string name, EEntityGroup groupEnum, Transform parent = null, object userData = null)
            where T : GameEntityBase
        {
            EntityGroup group;
            if (!_groups.TryGetValue(groupEnum, out group))
            {
                group = CreateGroup(groupEnum);
            }

            return group.Spawn<T>(name, parent, userData);
        }

        public void Unspawn(GameEntityBase entity)
        {
            //一个Entity已经被spawn出来它是必定有group的，如果这里报null说明逻辑有问题
            var group = _groups[entity.Group];
            group.Unspawn(entity);
        }

        /// <summary>
        /// 当传入新的Group时利用此方法初始化该Group，并且创建相应的GO
        /// </summary>
        private EntityGroup CreateGroup(EEntityGroup groupEnum)
        {
            var obj = new GameObject($"Entity Group - {groupEnum.ToString()}");
            var group = obj.AddComponent<EntityGroup>();
            group.Type = groupEnum;
            group.transform.SetParent(transform);
            group.transform.localScale = Vector3.one;
            _groups.Add(groupEnum, group);
            return group;
        }

        #region Events

        private void OnPlaySlate(object sender, GameEventArgs arg)
        {
            Active = false;
        }

        private void OnPlaySlateEnd(object sender, GameEventArgs arg)
        {
            Active = true;
        }

        #endregion
    }
}