using System;
using DG.Tweening;
using Framework;
using MyTimer;
using Sirenix.OdinInspector;
using Tencent;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameMain
{
    public class ChangeableItem : MonoBehaviour
    {
        [SerializeField, LabelText("果冻跳跃高度")] private float _jellyJumpHeight = 5f;
        [SerializeField, LabelText("初始材质枚举")] private EMaterial _initMat = EMaterial.WhiteError;
        private MeshRenderer _mainMr;
        private MeshRenderer _changeMr;
        private float _curDissolveValue = 0;

        private Collider _collider;
        private bool _isTouchingWithPlayer;
        private Material[] _curMeshMaterials;
        private Sequence _materialChangeTween;

        public EMaterial CurrentMaterial
        {
            get => _currentMaterial;
            protected set
            {
                if (value != _currentMaterial)
                {
                    OnChangeMaterial(value);
                }

                _currentMaterial = value;
            }
        }

        private float _initHeight;
        private EMaterial _currentMaterial;
        private Tween _hoverTween;

        private void Awake()
        {
            var mainObj = gameObject;
            var changeObj = transform.GetChild(0);
            _mainMr = mainObj.GetComponent<MeshRenderer>();
            if(changeObj is not null)
            {
                _changeMr = changeObj.GetComponent<MeshRenderer>();
            }
            else
            {
                _changeMr = GetComponentInChildren<MeshRenderer>();
            }
            _changeMr.gameObject.SetActive(false);
            _collider = GetComponent<Collider>();
            _currentMaterial = _initMat;
            _initHeight = transform.position.y;
            LoadConfig();
            DoChangeMaterial(_config.MaterialDict[_currentMaterial].ObjMaterials.ToArray());
            OnChangeMaterial(_currentMaterial);
            _hoverTween.Kill();
            _changeMr.gameObject.SetActive(false);
        }

        public virtual void OnHitMaterialBullet(EMaterial materialType, Material[] materials)
        {
            _curMeshMaterials = materials;
            CurrentMaterial = materialType;
        }

        private void DoChangeMaterial(Material[] materials)
        {
            _mainMr.materials = materials;
        }

        private static string _configPath = "Assets/GameMain/Configs/ChangeableConfig.asset";
        private ChangeableConfigSO _config;

        private void LoadConfig()
        {
            AsyncOperationHandle<ChangeableConfigSO>
                handle = Addressables.LoadAssetAsync<ChangeableConfigSO>(_configPath);
            _config = handle.WaitForCompletion();
        }

        protected void OnChangeMaterial(EMaterial materialType)
        {
            if (_curMeshMaterials is not null)
            {
                if (_materialChangeTween is not null && _materialChangeTween.active)
                {
                    _materialChangeTween.Kill();
                }

                _changeMr.gameObject.SetActive(true);
                _changeMr.material.SetFloat("_DissolveAmount", 0);

                _materialChangeTween = DOTween.Sequence().SetTarget(this);
                _materialChangeTween.Append(DOTween.To(() => _changeMr.material.GetFloat("_DissolveAmount"),
                        x => _changeMr.material.SetFloat("_DissolveAmount", x), 0.5f, 1f)
                    .OnComplete(() => DoChangeMaterial(_curMeshMaterials)));
                _materialChangeTween.Append(DOTween.To(() => _changeMr.material.GetFloat("_DissolveAmount"),
                        x => _changeMr.material.SetFloat("_DissolveAmount", x), 1, 2f)
                    .OnComplete(() => _changeMr.gameObject.SetActive(false)));
            }


            gameObject.layer = LayerMask.NameToLayer("Environment");
            if (_hoverTween is not null)
            {
                _hoverTween.Kill();
            }

            if (_currentMaterial == EMaterial.Hover)
            {
                _hoverTween = transform.DOMoveY(_initHeight, 2f);
            }

            switch (materialType)
            {
                case EMaterial.Cloud:
                    gameObject.layer = LayerMask.NameToLayer("Cloud");
                    break;
                case EMaterial.Climbable:
                    break;
                case EMaterial.Dangerous:
                    if (_isTouchingWithPlayer)
                    {
                        AGameManager.Instance.PlayerDie();
                    }

                    break;
                case EMaterial.Hover:
                    _hoverTween = transform.DOMoveY(_initHeight + 2, 2f);
                    break;
                default:
                    break;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            Debug.Log("collision enter");
            if (other.gameObject.TryGetComponent<PlayerTrigger>(out var playerTrigger))
            {
                _isTouchingWithPlayer = true;
                switch (_currentMaterial)
                {
                    case EMaterial.Jelly:
                        foreach (ContactPoint contact in other.contacts)
                        {
                            if (Vector3.Dot(contact.normal, Vector3.up) < -0.9f)
                            {
                                playerTrigger.Player.SuperJump(_jellyJumpHeight);
                            }
                        }

                        break;
                    case EMaterial.Dangerous:
                        AGameManager.Instance.PlayerDie();
                        break;
                }
            }
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject.TryGetComponent<PlayerTrigger>(out var playerTrigger))
            {
                _isTouchingWithPlayer = false;
            }
        }
    }
}