using System;
using DG.Tweening;
using Tencent;
using UnityEngine;

namespace GameMain
{
    public class CollectableObjBase : MonoBehaviour, ICollectable
    {
        protected Transform _graphics;
        private Sequence _loopTween;

        protected void Awake()
        {
            _graphics = transform.Find("Graphics");
            float initHeight = _graphics.position.y;

            _loopTween = DOTween.Sequence();
            _loopTween.Append(_graphics.DOMoveY(initHeight + 0.5f, 1f))
                .SetLoops(-1, LoopType.Yoyo);
            _loopTween.onUpdate += () => { _graphics.Rotate(Vector3.up, 60 * Time.deltaTime); };
        }


        public virtual void OnCollected(Player player)
        {
            _loopTween.Kill();
        }
    }
}