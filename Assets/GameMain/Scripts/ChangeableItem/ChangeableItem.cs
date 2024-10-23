using System;
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
        private Collider _collider;
        private bool _isTouchingWithPlayer;

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
            _collider = GetComponent<Collider>();
            _currentMaterial = EMaterial.WhiteError;
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
                case EMaterial.Climbable:
                    break;
                case EMaterial.Dangerous:
                    if (_isTouchingWithPlayer)
                    {
                        AGameManager.Instance.PlayerDie();
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
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