using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tencent
{
    public class Lock : MonoBehaviour
    {
        [SerializeField, LabelText("锁对应钥匙的Key")] private string _lockKey;
        [SerializeField, LabelText("锁对应的门")] private ActiveableDoor _door;

        private void Awake()
        {
            if (_door is not null)
            {
                _door.Lock();
            }
        }

        private void Unlock()
        {
            _door.Unlock();
            gameObject.SetActive(false);
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (!other.TryGetComponent<PlayerTrigger>(out var playerTrigger))
                return;
            var player = playerTrigger.Player;
            if (!player.TryUseKey(_lockKey))
                return;
            else
            {
                Unlock();
            }
        }
    }
}