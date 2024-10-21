using System;
using System.Collections.Generic;
using Framework;
using Services;
using Unity.Cinemachine;
using UnityEngine;

namespace Framework
{
    /// <summary>
    /// 由该Manager作为唯一的Mono去Update其他的Manager
    /// </summary>
    public class AGameManager : MonoBehaviour
    {
        #region Entry

        public static AGameManager Instance
        {
            get
            {
                if (_instance is null)
                {
                    GameObject obj = new GameObject("AGameManager");
                    _instance = obj.AddComponent<AGameManager>();
                }

                return _instance;
            }
        }

        private static AGameManager _instance;

        public static GameEntityManager Entity { get; private set; }

        #endregion


        private List<IManager> _managers = new();
        private List<IUpdatable> _updatables = new();
        public CinemachineCamera CinemachineCamera => FindAnyObjectByType<CinemachineCamera>();


        private bool _running = false;

        /// <summary>
        /// 创建各个子Manager
        /// </summary>
        private void InitManagers()
        {
            var trans = new GameObject("Managers").transform;
            trans.SetParent(transform);
            trans.localScale = Vector3.one;

            Entity = CreateManager<GameEntityManager>("Entity", trans);
        }

        private T CreateManager<T>(string name, Transform trans) where T : ManagerBase
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(trans);
            T mgr = obj.AddComponent<T>();
            _managers.Add(mgr);
            if (mgr.TryGetComponent<IUpdatable>(out var updatable))
            {
                _updatables.Add(updatable);
            }

            return mgr;
        }

        #region 生命周期

        /// <summary>
        /// 由ProcedureMain Enter时去调用
        /// </summary>
        public void OnEnter()
        {
            _running = true;
            InitManagers();
            foreach (var mgr in _managers)
            {
                mgr.OnEnter();
            }
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <summary>
        /// 由ProcedureMain Exit时去调用
        /// </summary>
        public void OnExit()
        {
            _running = false;
            _instance = null;
        }

        private bool _settingOpen = false;

        private void Update()
        {
            if (!_running) return;
            foreach (var updatable in _updatables)
            {
                updatable.OnUpdate(Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_settingOpen)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    GameEntry.UI.CloseUIForm(UIFormId.SettingForm);
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
                }

                _settingOpen = !_settingOpen;
            }
        }

        private void FixedUpdate()
        {
            if (!_running) return;
            foreach (var updatable in _updatables)
            {
                updatable.OnFixedUpdate(Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (!_running) return;
            foreach (var updatable in _updatables)
            {
                updatable.OnLateUpdate(Time.deltaTime);
            }
        }

        #endregion

        #region Events

        #endregion
    }
}