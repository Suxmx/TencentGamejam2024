using System;
using UnityEngine;

namespace Tencent
{
    public class Aim : MonoBehaviour
    {
        private GameObject _debugSphere;

        private void Awake()
        {
            _debugSphere = transform.Find("DebugSphere").gameObject;
        }

        public void UpdateRaycast(Vector3 position, Vector3 target)
        {
            Debug.DrawLine(position, target, Color.red);
            _debugSphere.transform.position = target;
        }
    }
}