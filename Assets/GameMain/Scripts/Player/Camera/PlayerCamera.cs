using System;
using DG.Tweening;
using Framework;
using Unity.Cinemachine;
using UnityEngine;

namespace Tencent
{
    public class PlayerCamera : MonoBehaviour
    {
        public CinemachineCamera FirstPersonCinemachine;
        public CinemachineCamera TopDownShotCinemachine;
        private CinemachineInputAxisController _inputAxis;
        private Transform _topDownGunPos, _firstPersonGunPos;
        private Sequence _changeTween;
        private MaterialGun _gun;

        public void Init(ECameraMode mode, Transform eye, Transform topdownGunPos, MaterialGun gun)
        {
            FirstPersonCinemachine = transform.Find("FirstPerson").GetComponent<CinemachineCamera>();
            TopDownShotCinemachine = transform.Find("TopDownShot").GetComponent<CinemachineCamera>();
            FirstPersonCinemachine.Follow = eye;
            FirstPersonCinemachine.LookAt = eye;

            TopDownShotCinemachine.Follow = eye;
            TopDownShotCinemachine.LookAt = eye;
            _inputAxis = FirstPersonCinemachine.GetComponent<CinemachineInputAxisController>();
            _topDownGunPos = topdownGunPos;
            _firstPersonGunPos = FirstPersonCinemachine.transform.Find("FirstPersonGunPos");
            _gun = gun;
            ChangeCameraMode(mode);
        }

        public Quaternion GetCameraRotation()
        {
            return FirstPersonCinemachine.transform.rotation;
        }

        public void SetInputAxisGain(float gain)
        {
            _inputAxis.Controllers[0].Input.Gain = gain;
            _inputAxis.Controllers[1].Input.Gain = -gain;
        }

        public void ChangeCameraMode(ECameraMode mode)
        {
            if (mode == ECameraMode.FirstPerson)
            {
                var graphics = AGameManager.Player.transform.Find("Root/Graphics");
                foreach (var trans in graphics.GetComponentsInChildren<Transform>())
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("CameraIgnore");
                }

                TopDownShotCinemachine.gameObject.SetActive(false);
                FirstPersonCinemachine.gameObject.SetActive(true);
                _gun.transform.SetParent(_firstPersonGunPos);
                _gun.transform.localScale = Vector3.one;
                _gun.transform.rotation = Quaternion.identity;
                _gun.transform.localPosition = Vector3.zero;
            }
            else if (mode == ECameraMode.TopDownShot)
            {
                var graphics = AGameManager.Player.transform.Find("Root/Graphics");
                foreach (var trans in graphics.GetComponentsInChildren<Transform>())
                {
                    trans.gameObject.layer = LayerMask.NameToLayer("Player");
                }

                FirstPersonCinemachine.gameObject.SetActive(false);
                TopDownShotCinemachine.gameObject.SetActive(true);
                _gun.transform.SetParent(_topDownGunPos);
                _gun.transform.localScale = Vector3.one;
                _gun.transform.rotation = Quaternion.identity;
                _gun.transform.localPosition = Vector3.zero;
            }

            GameEntry.Event.Fire(this, OnCameraModeChangeArg.Create(mode));
        }
    }
}