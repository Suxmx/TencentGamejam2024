using System;
using DG.Tweening;
using Framework;
using MyTimer;
using UnityEngine;

namespace GameMain
{
    public class PlayerTip : MonoBehaviour
    {
        [SerializeField] private Sprite _noBullet;
        [SerializeField] private Sprite _getBullet;
        [SerializeField] private Sprite _getKey;
        [SerializeField] private Sprite _getStar;

        [SerializeField] private Color _crossColor;
        [SerializeField] private Color _getBulletColor;
        [SerializeField] private Color _getKeyColor;
        [SerializeField] private Color _getStarColor;
        private Vector3 _delta;

        private Camera _camera;
        private SpriteRenderer _sr;
        private Animator _animator;

        private void Awake()
        {
            _camera = Camera.main;
            transform.forward = _camera.transform.position - transform.position;
        }


        private void LateUpdate()
        {
            if (_camera is null || AGameManager.Player is null) return;
            transform.position = AGameManager.Player.transform.position + _delta;
        }

        public void Init()
        {
            _sr = GetComponentInChildren<SpriteRenderer>();
            // _activeTimer.Initialize(1f, false);
            // _activeTimer.AfterCompelete += _ => gameObject.SetActive(false);
            _delta = transform.position - transform.parent.transform.position;
            transform.SetParent(null);
            _animator = GetComponent<Animator>();
        }

        public void ShowNoBulletIcon()
        {
            gameObject.SetActive(true);
            _animator.Play("NoBullet", 0, 0);
            _sr.sprite = _noBullet;
            _sr.color = _crossColor;
            // _activeTimer.Restart();
        }

        public void ShowGetBulletIcon()
        {
            gameObject.SetActive(true);
            _animator.Play("NoBullet", 0, 0);
            _sr.sprite = _getBullet;
            _sr.color = _getBulletColor;
            // _activeTimer.Restart();
        }

        public void ShowGetKeyIcon()
        {
            gameObject.SetActive(true);
            _animator.Play("NoBullet", 0, 0);
            _sr.sprite = _getKey;
            _sr.color = _getKeyColor;
        }

        public void ShowGetStarIcon()
        {
            gameObject.SetActive(true);
            _animator.Play("NoBullet", 0, 0);
            _sr.sprite = _getStar;
            _sr.color = _getStarColor;
        }
    }
}