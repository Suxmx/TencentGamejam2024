using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 仅用作GO容器，不做类型校验，校验应由上层来完成
    /// </summary>
    public class GameObjectPool
    {
        private Queue<GameObject> _pool=new();
        private GameObject _prefab;
        private Transform _parent;


        public GameObjectPool(GameObject prefab, Transform parent)
        {
            _prefab = prefab;
            _parent = parent;
        }

        public GameObject Spawn(Transform trans=null)
        {
            GameObject obj;
            if (_pool.Count <= 0)
            {
                obj = Object.Instantiate(_prefab);
            }
            else
            {
                obj = _pool.Dequeue();
            }

            if (trans is null)
            {
                obj.transform.SetParent(_parent);
            }
            else
            {
                obj.transform.SetParent(trans);
            }
            obj.SetActive(true);
            return obj;
        }

        public void Unspawn(GameObject obj)
        {
            obj.transform.SetParent(_parent);
            obj.transform.localScale=Vector3.one;
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}