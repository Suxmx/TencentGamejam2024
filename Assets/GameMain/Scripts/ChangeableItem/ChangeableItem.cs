using System;
using MyTimer;
using Tencent;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameMain
{
    public class ChangeableItem : MonoBehaviour
    {
        private static string _configPath = "Assets/GameMain/Configs/ChangeableConfig.asset";

        public EMaterial CurrentMaterial = EMaterial.None;
        private ChangeableConfigSO _config;
        private MeshRenderer _mr;
        private Material _defaultMaterial;
        private TimerOnly _cdTimer = new();

        private bool _canUseSkill = true;

        private void Awake()
        {
            _mr = GetComponent<MeshRenderer>();
            _defaultMaterial = _mr.material;
            LoadConfig();
            _cdTimer.Initialize(1f, false);
            _cdTimer.AfterCompelete += _ => _canUseSkill = true;
        }

        public virtual void OnHitMaterialBullet(EMaterial materialType)
        {
            ChangeMaterial(materialType);
        }

        private void LoadConfig()
        {
            AsyncOperationHandle<ChangeableConfigSO>
                handle = Addressables.LoadAssetAsync<ChangeableConfigSO>(_configPath);
            _config = handle.WaitForCompletion();
        }

        protected void ChangeMaterial(EMaterial materialType)
        {
            CurrentMaterial = materialType;
            if (materialType != EMaterial.None)
            {
                var m = _config.MaterialDict[materialType].ItemMaterial;
                _mr.material = m;
            }
            else
            {
                _mr.material = _defaultMaterial;
            }
        }

        private void TriggerSkill()
        {
            
        }
        
    }
}