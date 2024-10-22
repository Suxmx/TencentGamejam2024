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

        private EMaterial _currentMaterial;
        private MeshRenderer _mr;

        private void Awake()
        {
            _mr = GetComponent<MeshRenderer>();
        }

        public virtual void OnHitMaterialBullet(EMaterial materialType, Material _material)
        {
            if (_material is not null)
            {
                _mr.material = _material;
            }

            CurrentMaterial = materialType;
        }

        protected void OnChangeMaterial(EMaterial materialType)
        {
            gameObject.layer = LayerMask.NameToLayer("Environment");
            switch (materialType)
            {
                case EMaterial.Cloud:
                    gameObject.layer = LayerMask.NameToLayer("Cloud");
                    break;
                default:
                    break;
            }
        }
    }
}