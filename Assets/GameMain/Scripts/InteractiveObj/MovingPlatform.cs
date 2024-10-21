using System;
using Framework;
using Framework.Develop;
using KinematicCharacterController;
using MyTimer;
using Sirenix.OdinInspector;
using Tencent.Args;
using UnityEngine;

namespace Tencent
{
    [RequireComponent(typeof(PhysicsMover))]
    public class MovingPlatform : MonoBehaviour, IMoverController
    {
        [SerializeField, LabelText("开关Key")] private string _triggerKey;
        [SerializeField, LabelText("初始状态")] private bool _initState;

        [SerializeField, LabelText("移动速度")] private float _moveSpeed;
        [SerializeField, LabelText("停留时间")] private float _stopTime;
        [SerializeField, LabelText("往返点A")] private Transform _pointA;
        [SerializeField, LabelText("往返点B")] private Transform _pointB;

        private PhysicsMover _mover;
        private Transform _currentTarget;
        private bool _moving;
        private TimerOnly _stopTimer = new();

        private void Awake()
        {
            _mover = GetComponent<PhysicsMover>();
            _mover.MoverController = this;
            //把point的parent脱离自己，防止一起移动了
            var go = GameObject.Find("MovingPlatformTargets");
            if (go is null)
            {
                go = new GameObject("MovingPlatformTargets");
            }
            _pointA.SetParent(go.transform);
            _pointB.SetParent(go.transform);

            _currentTarget = _pointA;
            _stopTimer.Initialize(_stopTime,false);
            _stopTimer.AfterCompelete += _ => _moving = true;
            _moving = _initState;
            
            GameEntry.NewEvent.Subscribe(OnPressurePlateStateChangeArg.EventId,OnPressurePlateTriggered);
        }

        private void OnDestroy()
        {
            GameEntry.NewEvent.Unsubscribe(OnPressurePlateStateChangeArg.EventId,OnPressurePlateTriggered);
        }

        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            goalRotation = transform.rotation;
            //停留
            if (!_moving)
            {
                goalPosition = transform.position;
                return;
            }
            //到达后切换目标
            if (Vector3.Distance(transform.position, _currentTarget.position) < 0.2f)
            {
                _moving = false;
                _stopTimer.Restart();
                goalPosition = transform.position;
                _currentTarget = _currentTarget == _pointA ? _pointB : _pointA;
            }
            //正常移动
            var direction = (_currentTarget.position - transform.position).normalized;
            goalPosition = transform.position + direction * (_moveSpeed * deltaTime);

        }

        private void OnPressurePlateTriggered(object sender, GameEventArgs arg)
        {
            var e = (OnPressurePlateStateChangeArg)arg;
            if (e.TriggerKey != _triggerKey) return;
            _stopTimer.Paused = true;
            _moving = _initState ? !e.Enable : e.Enable;

        }
    }
}