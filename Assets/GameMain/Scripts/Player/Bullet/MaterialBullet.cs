using System;
using Framework;
using GameMain;
using MyTimer;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tencent
{
    public class MaterialBulletInfo
    {
        public MaterialBulletInfo(EMaterial materialType, Material bulletMaterial,Material objMaterial, Vector3 direction)
        {
            MaterialType = materialType;
            BulletMaterial = bulletMaterial;
            ObjMaterial = objMaterial;
            Direction = direction;
        }

        public EMaterial MaterialType;
        public Material BulletMaterial;
        public Material ObjMaterial;
        public Vector3 Direction;
    }

    public class MaterialBullet : GameEntityBase
    {
        [LabelText("子弹速度")] private float _bulletSpeed = 20;
        [LabelText("子弹销毁时间")] private float _bulletDestroyTime = 5f;
        private EMaterial _materialType;
        private MeshRenderer _mr;
        private Vector3 _fireDirection;
        private Rigidbody _rigid;
        private TimerOnly _destroyTimer;
        private Material _objMaterial;

        public override void OnInit()
        {
            _mr = GetComponent<MeshRenderer>();
            _rigid = GetComponent<Rigidbody>();

            _destroyTimer = new();
            _destroyTimer.Initialize(_bulletDestroyTime,false);
            _destroyTimer.AfterCompelete += _ => UnspawnObj();
        }

        public override void OnShow(object userData)
        {
            base.OnShow(userData);
            _destroyTimer.Restart();
            _rigid.linearVelocity = Vector3.zero;
            var data = (MaterialBulletInfo)userData;
            _materialType = data.MaterialType;
            _fireDirection = data.Direction.normalized;
            _mr.material = data.BulletMaterial;
            _objMaterial = data.ObjMaterial;
        }

        public override void OnHide()
        {
            base.OnHide();
            _destroyTimer.Paused = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            _destroyTimer.Paused = true;
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            _rigid.linearVelocity = _fireDirection * _bulletSpeed;
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var changeable = other.GetComponent<ChangeableItem>();
            if (changeable is null) return;
            var bulletHitVfx = AGameManager.Entity.Spawn<DestroyAfterTimeVfx>("BulletHitVFX", EEntityGroup.VFX, null, 1f);
            bulletHitVfx.transform.position = transform.position-_fireDirection*0.05f;
            changeable.OnHitMaterialBullet(_materialType,_objMaterial);
            UnspawnObj();
        }
    }
}