using System;
using System.Collections.Generic;
using MyTimer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tencent
{
    public class PressurePlate : MonoBehaviour
    {
        [SerializeField, LabelText("激活的Key")] private string _triggerKey;
        private bool _pressed = false;
        private List<GameObject> _upObjs = new();

        private HashSet<GameObject> _waitToAdd = new();
        private Dictionary<GameObject, TimerOnly> _addDict = new();

        private void Update()
        {
            if (_upObjs.Count == 0 && _pressed)
            {
                OnPressEnd();
            }

            if (_upObjs.Count > 0)
            {
                if (!_pressed)
                {
                    OnStartPressed();
                }

                OnBePressing();
            }
        }

        /// <summary>
        /// 开始被按压
        /// </summary>
        private void OnStartPressed()
        {
            _pressed = true;
        }

        /// <summary>
        /// 正在被按压
        /// </summary>
        private void OnBePressing()
        {
        }

        /// <summary>
        /// 按压结束
        /// </summary>
        private void OnPressEnd()
        {
            _pressed = false;
        }

        private void OnCollisionStay(Collision other)
        {
            //处理玩家
            if (other.gameObject.TryGetComponent<PlayerTrigger>(out var playerTrigger))
            {
                var player = playerTrigger.Player;
                if (player.FootPosition.y > transform.position.y && player.Motor.GroundingStatus.FoundAnyGround)
                {
                    if (!_upObjs.Contains(playerTrigger.gameObject) && !_waitToAdd.Contains(playerTrigger.gameObject))
                    {
                        AddToWaitQueue(playerTrigger.gameObject);
                    }
                }

                return;
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (_upObjs.Contains(other.gameObject))
            {
                _upObjs.Remove(other.gameObject);
            }

            if (_waitToAdd.Contains(other.gameObject))
            {
                _waitToAdd.Remove(other.gameObject);
                _addDict[other.gameObject].Paused = true;
                _addDict.Remove(other.gameObject);
            }
        }

        /// <summary>
        /// 增加一个时间限制 防止误触发
        /// </summary>
        /// <param name="go"></param>
        private void AddToWaitQueue(GameObject go)
        {
            if (_waitToAdd.Contains(go)) return;
            var timer = new TimerOnly();
            timer.Initialize(0.5f);
            timer.AfterCompelete += _ => { OnAddTimerEnd(timer, go); };
            _waitToAdd.Add(go);
            _addDict.Add(go, timer);
        }

        private void OnAddTimerEnd(TimerOnly timer, GameObject go)
        {
            _waitToAdd.Remove(go);
            _addDict.Remove(go);
            timer.Paused = true;
            _upObjs.Add(go);
        }
    }
}