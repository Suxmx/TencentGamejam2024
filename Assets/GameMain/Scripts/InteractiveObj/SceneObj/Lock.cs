using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tencent
{
    public class Lock : MonoBehaviour
    {
        [SerializeField, LabelText("锁对应钥匙的Key")]
        private string _lockKey;

        private Collider[] _cache = new Collider[10];
        private bool _unlock = false;

        private void Update()
        {
            var size = Physics.OverlapSphereNonAlloc(transform.position, 2, _cache, LayerMask.GetMask("Player"));
            for (int i = 0; i < Mathf.Min(size, 10); i++)
            {
                if (_cache[i].gameObject.TryGetComponent<PlayerTrigger>(out var playerTrigger))
                {
                    var player = playerTrigger.Player;
                    if (!player.TryUseKey(_lockKey))
                        return;
                    else Unlock();
                }
            }
        }

        private void Unlock()
        {
            if (_unlock) return;
            _unlock = true;
            transform.DOMoveY(transform.position.y + 2, 2f);
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