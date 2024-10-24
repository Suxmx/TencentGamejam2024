using System;
using System.Collections;
using Framework;
using Framework.Args;
using Framework.Develop;
using GameMain;
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

        private EMaterial _currentMaterial;
        private ChangeableConfigSO _config;
        private GameObject _debugSphere;
        private UnityEngine.Camera _mainCamera;
        private Player _player;
        private Animator _animator;

        private MoveableCube _movingCube = null;
        private float _distance;

        public void Init(Player player)
        {
            _player = player;
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
                _animator.SetTrigger("fire");
            }

            FireMoveGun();
            

            HandleMovingObj();
        }

        private void LateUpdate()
        {
            UpdateDebugSphere();
        }

        private void HandleMovingObj()
        {
            if (_movingCube is null) return;
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            _movingCube.SetTargetPosition(ray.origin + ray.direction * _distance);
        }

        private void FireMoveGun()
        {
            if (!Input.GetMouseButtonDown(1)) return;
            if (_movingCube is not null)
            {
                _movingCube.EndMove();
                _movingCube = null;
            }
            else
            {
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
                Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
                if (Physics.Raycast(ray.origin, ray.direction, out var info, 10f,
                        _shootingMask, QueryTriggerInteraction.UseGlobal))
                {
                    if (info.transform.TryGetComponent<MoveableCube>(out var moveableCube))
                    {
                        _movingCube = moveableCube;
                        _distance = info.distance;
                        _movingCube.StartMove();
                    }
                }
            }
        }

        private void FireMaterialBullet()
        {
            //震屏
            AGameManager.Instance.PlayerCamera.Impulse(0.2f);
            var targetPos = RaycastFromCursor();
            var direction = (targetPos+0.1f*Vector3.up - Muzzle.transform.position).normalized;

            Material bulletMaterial = _config.MaterialDict[_currentMaterial].BulletMaterial;
            Material objMaterial = _config.MaterialDict[_currentMaterial].ObjMaterial;
            var initInfo = new MaterialBulletInfo(_currentMaterial, bulletMaterial, objMaterial, direction);
            var bullet =
                AGameManager.Entity.Spawn<MaterialBullet>("MaterialBullet", EEntityGroup.Bullet, null, initInfo);
            bullet.transform.position = Muzzle.position;
            var muzzleVFX = AGameManager.Entity.Spawn<DestroyAfterTimeVfx>("MuzzleVFX", EEntityGroup.VFX, Muzzle, 1f);
            muzzleVFX.transform.position = Muzzle.position;
            muzzleVFX.transform.forward = Muzzle.transform.right;
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


        #region DEBUG

        private void UpdateDebugSphere()
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
        

        #endregion
    }
}