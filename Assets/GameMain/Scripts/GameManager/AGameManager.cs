using System;
using System.Collections.Generic;
using Framework;
using Framework.Args;
using GameMain;
using Services;
using Tencent;
using Unity.Cinemachine;
using UnityEngine;

namespace Framework
{
    public class PlayerSpawnInfo
    {
        public Vector3 Position;
        public Quaternion Rotation;
    }

    /// <summary>
    /// 由该Manager作为唯一的Mono去Update其他的Manager
    /// </summary>
    public class AGameManager : MonoBehaviour
    {
        #region Entry

        public static AGameManager Instance
        {
            get { return _instance; }
        }

        private static AGameManager _instance;

        public static GameEntityManager Entity { get; private set; }

        public static Player Player
        {
            get { return _player; }
        }

        private static Player _player;
        public static ECameraMode CameraMode;

        #endregion

        private List<IManager> _managers = new();
        private List<IUpdatable> _updatables = new();

        public PlayerCamera PlayerCamera
        {
            get
            {
                if (_playerCamera is null)
                    _playerCamera = FindAnyObjectByType<PlayerCamera>();
                return _playerCamera;
            }
        }

        public PlayerSpawnInfo SpawnInfo;
        private PlayerCamera _playerCamera;
        private bool _running = false;
        private bool _gameEnd = false;

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
        public static void OnEnter()
        {
            GameObject obj = new GameObject("AGameManager");
            _instance = obj.AddComponent<AGameManager>();
            _instance._running = true;
            _instance._gameEnd = false;
            _instance.InitManagers();
            foreach (var mgr in _instance._managers)
            {
                mgr.OnEnter();
            }

            GameEntry.Event.Fire(null, OnGameManagerInitedArg.Create());

            CameraMode = ECameraMode.TopDownShot;
            _instance.SpawnPlayer();
            Instance.PlayerCamera.Init(AGameManager.CameraMode, _player._eye, _player._topDownGunPos,
                _player._materialGun);
            Instance.PlayerCamera.ChangeCameraMode(AGameManager.CameraMode);
            GameEntry.UI.OpenUIForm(UIFormId.GameForm);
        }

        private void SpawnPlayer()
        {
            Debug.Log("spawn player");
            var spawnPoint = FindAnyObjectByType<PlayerSpawnPoint>();
            var player = Entity.Spawn<Player>("Player", EEntityGroup.Player);
            player.transform.SetParent(null);
            player.transform.localScale = Vector3.one;
            player.transform.rotation = spawnPoint.transform.rotation;
            player.transform.position = spawnPoint.transform.position;
            _player = player;
            spawnPoint.gameObject.SetActive(false);
        }

        /// <summary>
        /// 由ProcedureMain Exit时去调用
        /// </summary>
        public void OnExit()
        {
            foreach (var mgr in _managers)
            {
                mgr.OnExit();
            }

            _running = false;
            _playerCamera = null;
            _player = null;
            _managers.Clear();
            _updatables.Clear();
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
                    LockCursor();
                    GameEntry.UI.OpenUIForm(UIFormId.GameForm);
                    GameEntry.UI.CloseUIForm(UIFormId.SettingForm);
                }
                else
                {
                    FreeCursor(true);
                    GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
                    GameEntry.UI.CloseUIForm(UIFormId.GameForm);
                }

                _settingOpen = !_settingOpen;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                PlayerDie();
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                LevelWin();
            }

            if (Input.GetMouseButtonDown(2))
            {
                CameraMode = CameraMode == ECameraMode.FirstPerson ? ECameraMode.TopDownShot : ECameraMode.FirstPerson;
                PlayerCamera.ChangeCameraMode(CameraMode);
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                GameEntry.UI.OpenUIForm(UIFormId.DialogueForm, FindAnyObjectByType<GameTester>()._dialogueData);
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

        public void PlayerDie()
        {
            if (_gameEnd) return;
            _gameEnd = true;
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain).PlayerDie();
        }

        public void LevelWin()
        {
            if (_gameEnd) return;
            _gameEnd = true;
            (GameEntry.Procedure.CurrentProcedure as ProcedureMain).LevelWin();
        }

        #endregion

        #region Cusor

        public static void FreeCursor(bool visible = false)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = visible;
        }

        public static void LockCursor()
        {
            Cursor.visible = false;
            switch (CameraMode)
            {
                case ECameraMode.FirstPerson:
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                case ECameraMode.TopDownShot:
                    Cursor.lockState = CursorLockMode.Confined;
                    break;
            }
        }

        #endregion
    }
}