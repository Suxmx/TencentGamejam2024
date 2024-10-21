using System;
using GameMain;
using UnityEngine;

namespace Tencent
{
    public class PlayerTrigger : MonoBehaviour
    {
        public Player Player => _player;

        private CapsuleCollider _collider;
        private Player _player;

        public void Init(Player player)
        {
            _collider = GetComponent<CapsuleCollider>();
            _player = player;
        }

        public void ResetCollider(Vector3 center, float radius, float height)
        {
            _collider.center = center;
            _collider.radius = radius + 0.001f;
            _collider.height = height + 0.001f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ICollectable>(out var collect))
            {
                collect.OnCollected(_player);
                Debug.Log("on trigger enter");
            }
        }
    }
}