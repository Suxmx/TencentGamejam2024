using System;
using System.Linq;
using DG.Tweening;
using Framework;
using KinematicCharacterController;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;
using UnityHFSM;

namespace Tencent
{
    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
    }

    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor => _motor;

        //实际输入
        public Vector3 MoveInputVector => _moveInputVector;
        public Vector3 LookInputVector => _lookInputVector;
        public float CurHeight => _curHeight;

        private PlayerInput _input;
        private Transform _eye;
        private KinematicCharacterMotor _motor;
        private CinemachineCamera _cinemachine;
        private Transform _graphics;
        private PlayerFsm _fsm;
        private Aim _aim;

        private void Awake()
        {
            InitComponents();
            InitFsm();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            _input.UpdateInput();
            HandleCharacterInput();
            _fsm.OnLogic();
        }

        private void LateUpdate()
        {
            UpdateAimRaycast();
        }

        private void UpdateAimRaycast()
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);
            if (Physics.Raycast(ray.origin, ray.direction, out var info, 10f,
                    LayerMask.GetMask("Ground", "Environment"), QueryTriggerInteraction.UseGlobal))
            {
                _aim.UpdateRaycast(ray.origin, info.point);
                return;
            }

            _aim.UpdateRaycast(ray.origin, _eye.position + ray.direction * 10);
        }

        #region Init

        private void InitComponents()
        {
            _input = GetComponent<PlayerInput>();
            _motor = GetComponent<KinematicCharacterMotor>();
            _cinemachine = FindAnyObjectByType<CinemachineCamera>();
            _aim = GetComponentInChildren<Aim>();

            _eye = transform.Find("Root/Eye");
            _graphics = transform.Find("Root/Graphics");

            _motor.CharacterController = this;

            _curHeight = StandUpHeight;
        }

        private void InitFsm()
        {
            _fsm = new PlayerFsm(this);
            _fsm.AddState(EPlayerState.GroundMove, new GroundMoveState());
            _fsm.AddState(EPlayerState.Crouch, new GroundCrouchState());
            _fsm.AddState(EPlayerState.Air, new AirState());
            _fsm.AddState(EPlayerState.Jump, new JumpState(needsExitTime: true));

            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Jump,
                _ => InputData.HasEventStart(InputEvent.Jump));
            _fsm.AddTransition(EPlayerState.Jump, EPlayerState.Air, _ => Motor.Velocity.y < 0);
            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Air, _ => !Motor.GroundingStatus.FoundAnyGround,
                forceInstantly: true);
            _fsm.AddTransition(EPlayerState.Air, EPlayerState.GroundMove, _ => Motor.GroundingStatus.FoundAnyGround);
            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Crouch,
                _ => InputData.HasEvent(InputEvent.Crouch));
            _fsm.AddTransition(EPlayerState.Crouch, EPlayerState.GroundMove,
                _ => !InputData.HasEvent(InputEvent.Crouch) && CanStandupWhenCrouching);
            _fsm.AddTransition(EPlayerState.Crouch, EPlayerState.Air, _ => !Motor.GroundingStatus.FoundAnyGround);
            _fsm.AddTransition(EPlayerState.Crouch, EPlayerState.Jump,
                _ => InputData.HasEventStart(InputEvent.Jump) && CanStandupWhenCrouching);

            _fsm.Init();
        }

        #endregion

        #region Kcc

        [BoxGroup("KCC"), LabelText("地面移动速度")] public float GroundMoveSpeed = 15f;
        [BoxGroup("KCC"), LabelText("站起高度")] public float StandUpHeight = 1f;

        [BoxGroup("KCC/空中"), LabelText("空中移动速度")]
        public float AirMoveSpeed = 10f;

        [BoxGroup("KCC/空中"), LabelText("重力")] public Vector3 Gravity = new Vector3(0, -30f, 0);

        [BoxGroup("KCC/跳跃"), LabelText("跳跃速度")]
        public float JumpUpSpeed = 10f;


        [BoxGroup("KCC/下蹲"), LabelText("下蹲高度")]
        public float CrouchedCapsuleHeight = 0.5f;

        [BoxGroup("KCC/下蹲"), LabelText("下蹲移动速度")]
        public float CrouchMoveSpeed = 4f;

        [BoxGroup("KCC/下蹲"), LabelText("下蹲插值时间")]
        public float CrouchTime = .3f;

        private Vector3 _moveInputVector, _lookInputVector;
        [NonSerialized] public bool CanStandupWhenCrouching = false;
        [NonSerialized] public bool IsCrouching = false;
        private Tween _crouchTween;
        private float _curHeight;

        public void DoCrouchSmoothly()
        {
            if (_crouchTween is not null) _crouchTween.Kill();
            IsCrouching = true;
            _crouchTween = DOTween.To(x => _curHeight = x, _curHeight, CrouchedCapsuleHeight, CrouchTime);
            _crouchTween.onUpdate += () =>
            {
                Debug.Log(_curHeight);
                Motor.SetCapsuleDimensions(0.245f, _curHeight, _curHeight / 2f);
                _graphics.localScale = new Vector3(0.5f, _curHeight / 2f, 0.5f);
                _graphics.localPosition = new Vector3(0, _curHeight / 2f, 0);
                var eyePos = _eye.transform.localPosition;
                eyePos.y = _curHeight;
                _eye.localPosition = eyePos;
            };
            _crouchTween.onComplete += () => IsCrouching = false;
        }

        public void DoStandUpSmoothly()
        {
            if (_crouchTween is not null) _crouchTween.Kill();
            _crouchTween = DOTween.To(x => _curHeight = x, _curHeight, StandUpHeight, CrouchTime);
            _crouchTween.onUpdate += () =>
            {
                Debug.Log(_curHeight);
                Motor.SetCapsuleDimensions(0.245f, _curHeight, _curHeight / 2f);
                _graphics.localScale = new Vector3(0.5f, _curHeight / 2f, 0.5f);
                _graphics.localPosition = new Vector3(0, _curHeight / 2f, 0);
                var eyePos = _eye.transform.localPosition;
                eyePos.y = _curHeight;
                _eye.localPosition = eyePos;
            };
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs inputs = new PlayerCharacterInputs();

            inputs.MoveAxisForward = InputData.MoveInput.y;
            inputs.MoveAxisRight = InputData.MoveInput.x;
            inputs.CameraRotation = _cinemachine.transform.rotation;

            //handle input
            Vector3 moveInputVector =
                Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection =
                Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp)
                    .normalized;
            }

            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);
            //生成实际的输入
            _moveInputVector = cameraPlanarRotation * moveInputVector;
            _lookInputVector = cameraPlanarDirection;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector.sqrMagnitude > 0f)
            {
                currentRotation = Quaternion.LookRotation(_lookInputVector, Motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.UpdateVelocity(ref currentVelocity, deltaTime);
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.BeforeCharacterUpdate(deltaTime);
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.PostGroundingUpdate(deltaTime);
            }
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.AfterCharacterUpdate(deltaTime);
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                return kcc.IsColliderValidForCollisions(coll);
            }

            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.OnGroundHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
            }
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.OnMovementHit(hitCollider, hitNormal, hitPoint, ref hitStabilityReport);
            }
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.ProcessHitStabilityReport(hitCollider, hitNormal, hitPoint, atCharacterPosition,
                    atCharacterRotation, ref hitStabilityReport);
            }
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.OnDiscreteCollisionDetected(hitCollider);
            }
        }

        #endregion
    }
}