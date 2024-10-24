using System;
using DG.Tweening;
using MyTimer;
using UnityEngine;

namespace GameMain
{
    public class PlayerTip : MonoBehaviour
    {
        [SerializeField] private Sprite _noBullet;
        [SerializeField] private Sprite _getBullet;

        private Camera _camera;
        private SpriteRenderer _sr;
        private TimerOnly _activeTimer = new();

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_camera is null) return;
            transform.forward = _camera.transform.position - transform.position;
        }

        public void Init()
        {
            _sr = GetComponent<SpriteRenderer>();
            _activeTimer.Initialize(1f, false);
        }

        public void ShowNoBulletIcon()
        {
            gameObject.SetActive(true);
        }

        public void ShowGetBulletIcon()
        {
            gameObject.SetActive(true);
        }
    }
}