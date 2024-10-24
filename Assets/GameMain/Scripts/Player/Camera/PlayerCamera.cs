using System;
using DG.Tweening;
using Framework;
using Unity.Cinemachine;
using UnityEngine;

namespace Tencent
{
    public class PlayerCamera : MonoBehaviour
    {
        [NoSaveDuringPlay] public CinemachineCamera FirstPersonCinemachine;
        [NoSaveDuringPlay] public CinemachineCamera TopDownShotCinemachine;
        private CinemachineInputAxisController _inputAxis;
        private Transform _topDownGunPos, _firstPersonGunPos;
        private Sequence _changeTween;
        private MaterialGun _gun;
        private CinemachineImpulseSource _impulseSource;
        private CinemachinePositionComposer _topdownPosComposer;
        private Vector3 _basicTopdownTrackingOffset;

        public void Init(ECameraMode mode, Transform eye, Transform topdownGunPos, MaterialGun gun)
        {
            FirstPersonCinemachine = transform.Find("FirstPerson").GetComponent<CinemachineCamera>();
            TopDownShotCinemachine = transform.Find("TopDownShot").GetComponent<CinemachineCamera>();
            _impulseSource = GetComponent<CinemachineImpulseSource>();
            FirstPersonCinemachine.Follow = eye;
            FirstPersonCinemachine.LookAt = eye;

            TopDownShotCinemachine.Follow = eye;
            TopDownShotCinemachine.LookAt = eye;
            _inputAxis = FirstPersonCinemachine.GetComponent<CinemachineInputAxisController>();
            _topDownGunPos = topdownGunPos;
            _firstPersonGunPos = FirstPersonCinemachine.transform.Find("FirstPersonGunPos");
            _gun = gun;
            _topdownPosComposer = TopDownShotCinemachine.GetComponent<CinemachinePositionComposer>();
            _basicTopdownTrackingOffset = _topdownPosComposer.TargetOffset;
            ChangeCameraMode(mode);
        }

        public Quaternion GetCameraRotation()
        {
            return FirstPersonCinemachine.transform.rotation;
        }

        /// <summary>
        /// 设置灵敏度
        /// </summary>
        /// <param name="gain"></param>
        public void SetInputAxisGain(float gain)
        {
            _inputAxis.Controllers[0].Input.Gain = gain;
            _inputAxis.Controllers[1].Input.Gain = -gain;
        }

        /// <summary>
        /// 震屏
        /// </summary>
        /// <param name="force"></param>
        public void Impulse(float force = 0.2f)
        {
            _impulseSource.GenerateImpulseWithForce(force);
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

        public void SetMouseOffset(Vector2 mousePosition)
        {
            mousePosition -= new Vector2(Screen.width / 2f, Screen.height / 2f);
            var offset = new Vector2(-mousePosition.x / Screen.width, mousePosition.y / Screen.height);
            offset.x = Mathf.Clamp(offset.x, -0.3f, 0.3f);
            offset.y = Mathf.Clamp(offset.y, -0.3f, 0.3f);
            _topdownPosComposer.Composition.ScreenPosition =
                Vector2.Lerp(_topdownPosComposer.Composition.ScreenPosition, offset, Time.deltaTime * 5);
        }
    }
}