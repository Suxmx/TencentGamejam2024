using System;
using DG.Tweening;
using Framework;
using MyTimer;
using UnityEngine;

namespace Tencent
{
    public class MoveableCube : MonoBehaviour
    {
        private Rigidbody _rigid;
        private BoxCollider _collider;
        private bool _moving = false;
        // private TimerOnly _dotTimer = new();

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _rigid = GetComponent<Rigidbody>();
            // _dotTimer.Initialize(0.5f, false);
        }

        public void StartMove()
        {
            _collider.enabled = true;
            _rigid.useGravity = false;
            _moving = true;
            gameObject.layer = LayerMask.NameToLayer("Cloud");
            // _dotTimer.OnTick += OnTimerTick;
            // _dotTimer.AfterCompelete += OnTimerEnd;
            // _dotTimer.Restart();
        }


        private void LateUpdate()
        {
            if (_moving)
            {
                transform.position = Vector3.Lerp(transform.position, AGameManager.Player.transform.position +
                                                                      AGameManager.Player.Motor.CharacterForward *
                                                                      (1.5f) +
                                                                      Vector3.up * 0.5f, Time.deltaTime * 5);
            }
        }

        public void EndMove()
        {
            _collider.enabled = true;
            _rigid.useGravity = true;
            _moving = false;
            gameObject.layer = LayerMask.NameToLayer("Environment");
        }
    }
}