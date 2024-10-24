using System;
using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.Args;
using Framework.Develop;
using GameMain;
using MyTimer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;

namespace Tencent
{
    public class MaterialGun : MonoBehaviour
    {
        [SerializeField] private GameObject _materialBulletPrefab;
        [SerializeField] public Transform Muzzle;
        [SerializeField] private LayerMask _shootingMask;
        private static string _configPath = "Assets/GameMain/Configs/ChangeableConfig.asset";

        private static List<EMaterial> _canCollectMat = new()
        {
            EMaterial.WhiteError,
            EMaterial.Jelly,
            EMaterial.Climbable,
            EMaterial.Cloud,
            EMaterial.Hover,
        };

        private Dictionary<EMaterial, int> _bulletDict = new();

        private EMaterial _currentMaterial;
        private ChangeableConfigSO _config;
        private GameObject _debugSphere;
        private UnityEngine.Camera _mainCamera;
        private Player _player;
        private Animator _animator;

        private MoveableCube _movingCube = null;
        private float _distance;

        private TimerOnly _cd = new();

        public void Init(Player player)
        {
            _player = player;
            foreach (var emat in _canCollectMat)
            {
                _bulletDict.Add(emat, 0);
                GameEntry.Event.Fire(this, OnBulletNumChangeArg.Create(emat, 0));
            }

            _cd.Initialize(0.8f, true);
        }

        private void Awake()
        {
            _debugSphere = transform.Find("DebugSphere").gameObject;
            _mainCamera = Camera.main;
            _animator = GetComponent<Animator>();
            LoadConfig();
            _currentMaterial = EMaterial.Cloud;
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(OnGameManagerInitedArg.EventId, AfterMgrInited);
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(OnGameManagerInitedArg.EventId, AfterMgrInited);
        }

        private void AfterMgrInited(object sender, GameEventArgs e)
        {
            Debug.Log("AfterMgrInited");
            GameEntry.Event.Fire(this, OnGunMaterialChangeArg.Create(_currentMaterial));
        }

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Q))
            // {
            //     int count = Enum.GetValues(typeof(EMaterial)).Length;
            //     int index = (int)_currentMaterial;
            //     index = index - 1 < 0 ? count - 1 : index - 1;
            //     _currentMaterial = (EMaterial)index;
            //     GameEntry.Event.Fire(this, OnGunMaterialChangeArg.Create(_currentMaterial));
            // }
            //
            // if (Input.GetKeyDown(KeyCode.E))
            // {
            //     int count = Enum.GetValues(typeof(EMaterial)).Length;
            //     int index = (int)_currentMaterial;
            //     index = index + 1 >= count ? 0 : index + 1;
            //     _currentMaterial = (EMaterial)index;
            //     GameEntry.Event.Fire(this, OnGunMaterialChangeArg.Create(_currentMaterial));
            // }

            if (InputData.HasEventStart(InputEvent.Fire))
            {
                if (!_cd.Completed) return;
                _animator.SetTrigger("fire");
                _cd.Restart();
            }

            FireMoveGun();
        }

        private void LateUpdate()
        {
            UpdateCrossHair();
        }


        private void FireMoveGun()
        {
            if (!Input.GetMouseButtonDown(1))
                return;
            if (_movingCube is not null)
            {
                _movingCube.EndMove();
                _movingCube = null;
                return;
            }

            var targetPos = RaycastFromCursor(out var hit);
            if (Vector3.Distance(Muzzle.transform.position, targetPos) > 2f)
            {
                return;
            }
            if (hit.transform.TryGetComponent<MoveableCube>(out var cube))
            {
                _movingCube = cube;
                cube.StartMove();
                Debug.Log("start move");
            }
        }

        private void FireMaterialBullet()
        {
            if (_bulletDict[_currentMaterial] <= 0)
            {
                _player.OnHaveNoBullet();
                return;
            }

            //震屏
            AGameManager.Instance.PlayerCamera.Impulse(0.2f);
            var targetPos = RaycastFromCursor();
            var direction = (targetPos + 0.1f * Vector3.up - Muzzle.transform.position).normalized;

            Material bulletMaterial = _config.MaterialDict[_currentMaterial].BulletMaterial;
            Material[] objMaterials = _config.MaterialDict[_currentMaterial].ObjMaterials.ToArray();
            var initInfo = new MaterialBulletInfo(_currentMaterial, bulletMaterial, objMaterials, direction);
            var bullet =
                AGameManager.Entity.Spawn<MaterialBullet>("MaterialBullet", EEntityGroup.Bullet, null, initInfo);
            bullet.transform.position = Muzzle.position;
            var muzzleVFX = AGameManager.Entity.Spawn<DestroyAfterTimeVfx>("MuzzleVFX", EEntityGroup.VFX, Muzzle, 1f);
            muzzleVFX.transform.position = Muzzle.position;
            muzzleVFX.transform.forward = Muzzle.transform.right;
            //消耗子弹
            _bulletDict[_currentMaterial]--;
            GameEntry.Event.Fire(this, OnBulletNumChangeArg.Create(_currentMaterial, _bulletDict[_currentMaterial]));
        }

        /// <summary>
        /// 获得从准心射击得到的目标点
        /// </summary>
        /// <returns></returns>
        private Vector3 RaycastFromCursor()
        {
            switch (AGameManager.CameraMode)
            {
                case ECameraMode.FirstPerson:
                    Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
                    Ray screenCenterRay = _mainCamera.ScreenPointToRay(screenCenter);
                    if (Physics.Raycast(screenCenterRay.origin, screenCenterRay.direction, out var screenCenterInfo,
                            10f,
                            _shootingMask, QueryTriggerInteraction.UseGlobal))
                    {
                        return screenCenterInfo.point;
                    }

                    return screenCenterRay.origin + screenCenterRay.direction * 10;
                case ECameraMode.TopDownShot:
                    Ray mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(mouseRay.origin - mouseRay.direction * 5, mouseRay.direction,
                            out var mouseHitInfo, 1000f,
                            _shootingMask, QueryTriggerInteraction.UseGlobal))
                    {
                        return mouseHitInfo.point;
                    }

                    return Vector3.zero;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// 获得从准心射击得到的目标点
        /// </summary>
        /// <returns></returns>
        private Vector3 RaycastFromCursor(out RaycastHit hit)
        {
            hit = default;
            switch (AGameManager.CameraMode)
            {
                case ECameraMode.FirstPerson:
                    Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
                    Ray screenCenterRay = _mainCamera.ScreenPointToRay(screenCenter);
                    if (Physics.Raycast(screenCenterRay.origin, screenCenterRay.direction, out var screenCenterInfo,
                            10f,
                            _shootingMask, QueryTriggerInteraction.UseGlobal))
                    {
                        hit = screenCenterInfo;
                        return screenCenterInfo.point;
                    }

                    return screenCenterRay.origin + screenCenterRay.direction * 10;
                case ECameraMode.TopDownShot:
                    Ray mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(mouseRay.origin - mouseRay.direction * 5, mouseRay.direction,
                            out var mouseHitInfo, 1000f,
                            _shootingMask, QueryTriggerInteraction.UseGlobal))
                    {
                        hit = mouseHitInfo;
                        return mouseHitInfo.point;
                    }

                    return Vector3.zero;
            }

            return Vector3.zero;
        }

        private void LoadConfig()
        {
            AsyncOperationHandle<ChangeableConfigSO>
                handle = Addressables.LoadAssetAsync<ChangeableConfigSO>(_configPath);
            _config = handle.WaitForCompletion();
        }

        #region 动画

        public void SetTrigger(string name)
        {
            _animator.SetTrigger(name);
        }

        public void SetBool(string name, bool value)
        {
            _animator.SetBool(name, value);
        }

        #endregion

        public void GetMaterialBullet(EMaterial eMaterial)
        {
            _bulletDict[eMaterial]++;
            GameEntry.Event.Fire(this, OnBulletNumChangeArg.Create(eMaterial, _bulletDict[eMaterial]));
        }

        public void ChangeMaterialGunMat(EMaterial eMaterial)
        {
            _currentMaterial = eMaterial;
        }


        private void UpdateCrossHair()
        {
            if (_debugSphere is null) return;
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Ray screenRay = _mainCamera.ScreenPointToRay(screenCenter);

            var target = RaycastFromCursor();
            var direction = (target - Muzzle.transform.position).normalized;
            Ray muzzleRay = new Ray(Muzzle.transform.position, direction);
            if (Physics.Raycast(muzzleRay, out var info, 10f, _shootingMask, QueryTriggerInteraction.UseGlobal))
            {
                Debug.DrawLine(muzzleRay.origin, info.point, Color.red);
                _debugSphere.transform.position = info.point;
                return;
            }

            Debug.DrawLine(muzzleRay.origin, (screenRay.origin + screenRay.direction * 10), Color.red);
            _debugSphere.transform.position = muzzleRay.origin + muzzleRay.direction * 10;
        }

        private void OnHaveNoBullet()
        {
        }
    }
}