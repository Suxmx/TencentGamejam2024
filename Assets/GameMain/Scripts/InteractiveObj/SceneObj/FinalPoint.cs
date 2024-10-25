using System;
using Framework;
using UnityEngine;

namespace Tencent
{
    public class FinalPoint : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerTrigger>(out var playerTrigger))
            {
                AGameManager.Instance.LevelWin();
            }
        }
    }
}