using System;
using GameMain;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tencent
{
    public class MaterialBullet : MonoBehaviour
    {
        [LabelText("子弹速度")] private float _bulletSpeed = 20;
        private EMaterial _materialType;
        private MeshRenderer _mr;
        private bool _inited = false;
        private Vector3 _fireDirection;
        private Rigidbody _rigid;

        public void Init(EMaterial materialType, Material bulletMaterial, Vector3 direction)
        {
            _mr = GetComponent<MeshRenderer>();
            _rigid = GetComponent<Rigidbody>();
            _mr.material = bulletMaterial;

            _materialType = materialType;
            _fireDirection = direction.normalized;
            _inited = true;
        }

        private void Update()
        {
            if (!_inited) return;
            _rigid.linearVelocity = _fireDirection * _bulletSpeed;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var changeable = other.GetComponent<ChangeableItem>();
            if (changeable is null) return;

            changeable.OnHitMaterialBullet(_materialType);
            Destroy(gameObject);
        }
    }
}