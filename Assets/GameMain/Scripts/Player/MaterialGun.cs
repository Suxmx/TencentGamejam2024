using System;
using GameMain;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tencent
{
    public class MaterialGun : MonoBehaviour
    {
        [SerializeField] private GameObject _materialBulletPrefab;
        private static string _configPath = "Assets/GameMain/Configs/ChangeableConfig.asset";

        private EMaterial _currentMaterial;
        private ChangeableConfigSO _config;
        private GameObject _debugSphere;
        private Camera _mainCamera;

        private void Awake()
        {
            _debugSphere = transform.Find("DebugSphere").gameObject;
            _mainCamera = Camera.main;
            LoadConfig();
            _currentMaterial = EMaterial.Test1;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                _currentMaterial = EMaterial.Test1;
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _currentMaterial = EMaterial.Test2;
            }

            FireMaterialBullet();
            UpdateDebugSphere();
        }

        private void FireMaterialBullet()
        {
            if (!InputData.HasEventStart(InputEvent.Fire)) return;
            Debug.Log("fire");
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);

            var bullet = Instantiate(_materialBulletPrefab).GetComponent<MaterialBullet>();
            var bulletMaterial = _config.MaterialDict[_currentMaterial].BulletMaterial;
            bullet.transform.position = ray.origin;
            bullet.Init(_currentMaterial, bulletMaterial, ray.direction);
        }

        private void RaycastOnce()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray.origin, ray.direction, out var info, 10f,
                    LayerMask.GetMask("Ground", "Environment"), QueryTriggerInteraction.UseGlobal))
            {
            }
        }

        private void LoadConfig()
        {
            AsyncOperationHandle<ChangeableConfigSO>
                handle = Addressables.LoadAssetAsync<ChangeableConfigSO>(_configPath);
            _config = handle.WaitForCompletion();
        }


        #region DEBUG

        private void UpdateDebugSphere()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray.origin, ray.direction, out var info, 10f,
                    LayerMask.GetMask("Ground", "Environment"), QueryTriggerInteraction.UseGlobal))
            {
                Debug.DrawLine(ray.origin, info.point, Color.red);
                _debugSphere.transform.position = info.point;
                return;
            }

            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 10, Color.red);
            _debugSphere.transform.position = ray.origin + ray.direction * 10;
        }

        #endregion
    }
}