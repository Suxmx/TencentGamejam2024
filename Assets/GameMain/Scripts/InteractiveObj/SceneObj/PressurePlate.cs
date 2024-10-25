using System;
using System.Collections.Generic;
using DG.Tweening;
using Framework;
using MyTimer;
using Sirenix.OdinInspector;
using Tencent.Args;
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
        private Tween _materialTween;
        private Transform _red;

        private void Awake()
        {
            _red = transform;
        }

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
            if (_materialTween is not null && _materialTween.active)
            {
                _materialTween.Kill();
            }

            _materialTween = _red.GetComponent<MeshRenderer>().material
                .DOColor(GetHrdColor(new Color(25 / 255f, 2 / 255f, 2 / 255f), 2), "_EmissionColor", 0.1f)
                .OnUpdate(() => { Debug.Log(_red.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor")); });
            _pressed = true;
            GameEntry.Event.Fire(this, OnPressurePlateStateChangeArg.Create(_triggerKey, true));
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
            if (_materialTween is not null && _materialTween.active)
            {
                _materialTween.Kill();
            }

            _materialTween = _red.GetComponent<MeshRenderer>().material
                .DOColor(GetHrdColor(new Color(25 / 255f, 2 / 255f, 2 / 255f), 0), "_EmissionColor", 0.1f);
            GameEntry.Event.Fire(this, OnPressurePlateStateChangeArg.Create(_triggerKey, false));
        }

        private static Color GetHrdColor(Color c, float i)
        {
            return c * Mathf.Pow(2, i);
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

            if (other.gameObject.TryGetComponent<MoveableCube>(out var cube))
            {
                foreach (ContactPoint contact in other.contacts)
                {
                    if (Vector3.Dot(contact.normal, Vector3.up) < -0.9f)
                    {
                        if (!_upObjs.Contains(cube.gameObject) && !_waitToAdd.Contains(cube.gameObject))
                        {
                            Debug.Log("方块在上方");
                            AddToWaitQueue(cube.gameObject);
                        }

                        break;
                    }
                }
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
            timer.Initialize(0.1f);
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