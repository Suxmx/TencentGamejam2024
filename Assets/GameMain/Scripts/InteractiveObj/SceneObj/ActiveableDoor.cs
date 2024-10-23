using System;
using DG.Tweening;
using Framework;
using Framework.Develop;
using Sirenix.OdinInspector;
using Tencent.Args;
using UnityEngine;

namespace Tencent
{
    public class ActiveableDoor : PressureActiveObj
    {
        [SerializeField, LabelText("开门上升最高点")] private Transform _targetPoint;
        [SerializeField, LabelText("开门时间")] private float _openTime = 1f;

        private bool _locked=false;
        private float _openSpeed;
        private float _initHeight;
        private float _targetHeight;
        private Tween _openTween;
        private bool _shouldOpen;

        private void OnDestroy()
        {
            if(_openTween is not null)
                _openTween.Kill();
        }

        protected override void InitState(bool enable)
        {
            _initHeight = transform.position.y;
            _targetHeight = _targetPoint.position.y;
            _openSpeed = Mathf.Abs(_targetPoint.position.y - _initHeight) / _openTime;

            if (enable)
            {
                DoMove(_targetHeight);
            }
        }

        private void DoMove(float target)
        {
            if (_openTween is not null)
            {
                _openTween.Kill();
            }

            var time = Mathf.Abs(transform.position.y - target) / _openSpeed;
            _openTween = transform.DOMoveY(target, time);
        }

        public void Lock()
        {
            _locked = true;
        }
        public void Unlock()
        {
            _locked = false;
            if (_shouldOpen)
            {
                DoMove(_targetHeight);
            }
            else
            {
                DoMove(_initHeight);
            }
        }
        
        protected override void OnTriggerStateChange(bool enable)
        {
            _shouldOpen = !_initState ? enable : !enable;
            if (_locked) return;
            if (_shouldOpen)
            {
                DoMove(_targetHeight);
            }
            else
            {
                DoMove(_initHeight);
            }
        }
    }
}