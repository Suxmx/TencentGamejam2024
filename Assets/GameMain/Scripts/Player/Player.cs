using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Framework;
using Framework.Args;
using Framework.Develop;
using GameMain;
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
    public class Player : GameEntityBase, ICharacterController
    {
        [SerializeField] private GameObject _playerTriggerPrefab;

        [BoxGroup("摄像机"), LabelText("视角灵敏度"), OnValueChanged(nameof(OnMouseGainChange))]
        public float MouseGain = 8f;

        public KinematicCharacterMotor Motor => _motor;
        public Vector3 FootPosition => _foot.position;

        //实际输入
        public Vector3 MoveInputVector => _moveInputVector;
        public Vector3 LookInputVector => _lookInputVector;

        public float ClimbInput => _climbUp;

        private ECameraMode _cameraMode => AGameManager.CameraMode;

        private PlayerInput _input;

        private KinematicCharacterMotor _motor;

        private PlayerFsm _fsm;


        public MaterialGun _materialGun;
        private PlayerTrigger _playerTrigger;

        #region 子物体

        private Transform _graphics;
        private Vector3 _graphicsDelta;
        private Transform _foot;
        private Transform _root;
        public Transform _eye;
        public Transform _topDownGunPos;
        private Transform _directionPointer;
        private Quaternion _targetDirectionPtrDir = Quaternion.Euler(90, 0, 0);
        private PlayerTip _playerTip;

        #endregion

        private void OnDialoguePlay(object sender, GameEventArgs arg)
        {
            var e = (OnDialoguePlayArg)arg;
            if (e.Start)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }


        public override void OnInit()
        {
            base.OnInit();
            _motor = GetComponent<KinematicCharacterMotor>();
            _motor.enabled = false;
            InitVariables();
            InitComponents();
            InitFsm();
        }

        public override void OnShow(object userData)
        {
            base.OnShow(userData);

            GameEntry.Event.Subscribe(OnDialoguePlayArg.EventId, OnDialoguePlay);
        }

        public override void OnHide()
        {
            base.OnHide();
            if (_crouchTween is not null)
                _crouchTween.Kill();
            GameEntry.Event.Unsubscribe(OnDialoguePlayArg.EventId, OnDialoguePlay);
        }

        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (Input.GetKeyDown(KeyCode.P))
            {
                SuperJump(10);
            }

            _input.UpdateInput();
            HandleCharacterInput();
            _fsm.OnLogic();
            //set gun anim
            _materialGun.SetBool("walk", Motor.Velocity.magnitude > 0.1f);
            AGameManager.Instance.PlayerCamera.SetMouseOffset(Input.mousePosition);
        }

        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);
            _playerTrigger.transform.position = _root.position;
            _directionPointer.transform.position = _root.position + Vector3.up * 0.02f;
            if (_moveInputVector.sqrMagnitude != 0)
            {
                _targetDirectionPtrDir = Quaternion.LookRotation(_moveInputVector, Motor.CharacterUp) *
                                         Quaternion.Euler(90, 0, 0);
            }

            _directionPointer.rotation = Quaternion.Slerp(_directionPointer.rotation, _targetDirectionPtrDir
                , Time.deltaTime * 10);
        }

        public void Pause()
        {
            Motor.enabled = false;
            _input.InputMap.Disable();
        }

        public void Resume()
        {
            Motor.enabled = true;
            _input.InputMap.Enable();
        }

        #region 收集

        private List<KeyInfo> _keyInfos = new();

        public void GetKey(KeyInfo info)
        {
            _keyInfos.Add(info);
            GameEntry.Event.Fire(this, OnGetKeyArgs.Create(info.KeyString, info.KeyIcon));
            _playerTip.ShowGetKeyIcon();
        }

        public bool TryUseKey(string key)
        {
            int index = _keyInfos.FindIndex(x => x.KeyString == key);
            if (index < 0)
                return false;
            _keyInfos.RemoveAt(index);
            GameEntry.Event.Fire(this, OnUseKeyArgs.Create(key));
            return true;
        }

        #endregion

        #region Init

        private void InitVariables()
        {
            JumpUpSpeed = Mathf.Sqrt(2 * JumpHeight * (Mathf.Abs(Gravity.y)));
        }

        private void InitComponents()
        {
            _input = GetComponent<PlayerInput>();
            _topDownGunPos = transform.Find("Root/TopDownGunPos");
            _materialGun = FindAnyObjectByType<MaterialGun>();
            _playerTrigger = Instantiate(_playerTriggerPrefab).GetComponent<PlayerTrigger>();

            _root = transform.Find("Root");
            _playerTrigger.transform.position = _root.position;

            _eye = transform.Find("Root/Eye");
            _graphics = transform.Find("Root/Graphics");
            _graphicsDelta = _graphics.localPosition;
            _foot = transform.Find("Root/Foot");
            _directionPointer = transform.Find("DirectionPointer");
            _playerTip = transform.Find("Root/PlayerTip").GetComponent<PlayerTip>();
            _directionPointer.SetParent(null);

            _playerTrigger.Init(this);
            _playerTrigger.ResetCollider(Vector3.up * StandUpHeight / 2f, 0.245f, StandUpHeight);

            _materialGun.Init(this);
            _playerTip.Init();

            _motor.CharacterController = this;
            _motor.enabled = true;

            _curHeight = StandUpHeight;
        }

        private void InitFsm()
        {
            _fsm = new PlayerFsm(this);
            _fsm.AddState(EPlayerState.GroundMove, new GroundMoveState());
            _fsm.AddState(EPlayerState.Crouch, new GroundCrouchState());
            _fsm.AddState(EPlayerState.Air, new AirState());
            _fsm.AddState(EPlayerState.Jump, new JumpState(needsExitTime: true));
            _fsm.AddState(EPlayerState.Climb, new ClimbState());

            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Jump,
                _ => InputData.HasEventStart(InputEvent.Jump));
            _fsm.AddTransition(EPlayerState.Jump, EPlayerState.Air, _ => Motor.Velocity.y < 0);
            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Air, _ => !Motor.GroundingStatus.IsStableOnGround,
                forceInstantly: true);
            _fsm.AddTransition(EPlayerState.Air, EPlayerState.GroundMove, _ => Motor.GroundingStatus.IsStableOnGround);
            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Crouch,
                _ => InputData.HasEvent(InputEvent.Crouch));
            _fsm.AddTransition(EPlayerState.Crouch, EPlayerState.GroundMove,
                _ => !InputData.HasEvent(InputEvent.Crouch) && CanStandupWhenCrouching);
            _fsm.AddTransition(EPlayerState.Crouch, EPlayerState.Air, _ => !Motor.GroundingStatus.IsStableOnGround);
            _fsm.AddTransition(EPlayerState.Crouch, EPlayerState.Jump,
                _ => InputData.HasEventStart(InputEvent.Jump) && CanStandupWhenCrouching);
            _fsm.AddTransition(EPlayerState.GroundMove, EPlayerState.Climb,
                _ => Input.GetKeyDown(KeyCode.T) && CheckClimb());
            _fsm.AddTransition(EPlayerState.Air, EPlayerState.Climb, _ => Input.GetKeyDown(KeyCode.T) && CheckClimb());
            _fsm.AddTransition(EPlayerState.Jump, EPlayerState.Climb, _ => Input.GetKeyDown(KeyCode.T) && CheckClimb());
            _fsm.AddTransition(EPlayerState.Climb, EPlayerState.GroundMove, _ => Input.GetKeyDown(KeyCode.T));
            _fsm.AddTransition(EPlayerState.Climb, EPlayerState.GroundMove,
                _ => ClimbingObj is null || ClimbingObj.CurrentMaterial != EMaterial.Climbable);
            _fsm.AddTriggerTransition("ClimbTop", EPlayerState.Climb, EPlayerState.GroundMove);

            _fsm.Init();
        }

        #endregion

        #region 运动相关

        [BoxGroup("KCC"), LabelText("地面移动速度")] public float GroundMoveSpeed = 15f;
        [BoxGroup("KCC"), LabelText("站起高度")] public float StandUpHeight = 1f;

        [BoxGroup("KCC/空中"), LabelText("空中移动速度")]
        public float AirMoveSpeed = 10f;

        [BoxGroup("KCC/空中"), LabelText("重力")] public Vector3 Gravity = new Vector3(0, -30f, 0);

        [BoxGroup("KCC/跳跃"), LabelText("跳跃高度")]
        public float JumpHeight = 10f;

        [BoxGroup("KCC/跳跃"), LabelText("跳跃速度"), Sirenix.OdinInspector.ReadOnly]
        public float JumpUpSpeed = 10f;


        [BoxGroup("KCC/下蹲"), LabelText("下蹲高度")]
        public float CrouchedCapsuleHeight = 0.5f;

        [BoxGroup("KCC/下蹲"), LabelText("下蹲移动速度")]
        public float CrouchMoveSpeed = 4f;

        [BoxGroup("KCC/下蹲"), LabelText("下蹲插值时间")]
        public float CrouchTime = .3f;

        [BoxGroup("KCC/攀爬"), LabelText("攀爬速度")]
        public float ClimbSpeed = 2f;

        private Vector3 _moveInputVector, _lookInputVector;
        private float _climbUp;
        [NonSerialized] public bool CanStandupWhenCrouching = false;
        [NonSerialized] public bool IsCrouching = false;
        private Tween _crouchTween;
        private float _curHeight;


        private bool _superJumpRequest = false;
        private float _superJumpVelocity = 0;

        public void SuperJump(float jumpHeight)
        {
            _superJumpVelocity = Mathf.Sqrt(2 * jumpHeight * (Mathf.Abs(Gravity.y)));
            _superJumpRequest = true;
        }

        public void DoCrouchSmoothly()
        {
            if (_crouchTween is not null) _crouchTween.Kill();
            IsCrouching = true;
            _crouchTween = DOTween.To(x => _curHeight = x, _curHeight, CrouchedCapsuleHeight, CrouchTime);
            _crouchTween.onUpdate += () =>
            {
                Debug.Log(_curHeight);
                Motor.SetCapsuleDimensions(0.245f, _curHeight, _curHeight / 2f);
                _graphics.localScale = new Vector3(1, _curHeight, 1);
                _graphics.localPosition = new Vector3(0, _curHeight / 2f, 0) + _graphicsDelta / 2f;
                var eyePos = _eye.transform.localPosition;
                eyePos.y = _curHeight;
                _eye.localPosition = eyePos;
                _playerTrigger.ResetCollider(Vector3.up * _curHeight / 2f, 0.245f, _curHeight);
            };
            _crouchTween.onComplete += () => IsCrouching = false;
            _crouchTween.SetTarget(this);
        }

        public void DoStandUpSmoothly()
        {
            if (_crouchTween is not null) _crouchTween.Kill();
            _crouchTween = DOTween.To(x => _curHeight = x, _curHeight, StandUpHeight, CrouchTime);
            _crouchTween.onUpdate += () =>
            {
                Debug.Log(_curHeight);
                Motor.SetCapsuleDimensions(0.245f, _curHeight, _curHeight / 2f);
                _graphics.localScale = new Vector3(1, _curHeight, 1);
                _graphics.localPosition = new Vector3(0, _curHeight / 2f, 0) + _graphicsDelta / 2f;
                var eyePos = _eye.transform.localPosition;
                eyePos.y = _curHeight;
                _eye.localPosition = eyePos;
                _playerTrigger.ResetCollider(Vector3.up * _curHeight / 2f, 0.245f, _curHeight);
            };
            _crouchTween.SetTarget(this);
        }

        [BoxGroup("KCC"), LabelText("攀爬层"), SerializeField]
        private LayerMask _climbableLayer;

        [NonSerialized] public ChangeableItem ClimbingObj;
        private Collider[] _cacheColliders = new Collider[8];

        private bool CheckClimb()
        {
            if (Motor.CharacterOverlap(Motor.TransientPosition + Motor.CharacterForward * 0.1f, Motor.TransientRotation,
                    _cacheColliders,
                    _climbableLayer, QueryTriggerInteraction.Collide) > 0)
            {
                if (_cacheColliders[0] is not null)
                {
                    Debug.Log("detect climbable");
                    ChangeableItem item = _cacheColliders[0].gameObject.GetComponent<ChangeableItem>();
                    if (item is null || item.CurrentMaterial != EMaterial.Climbable) return false;
                    ClimbingObj = item;
                    return true;
                }
            }

            return false;
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs inputs = new PlayerCharacterInputs();

            inputs.MoveAxisForward = InputData.MoveInput.y;
            inputs.MoveAxisRight = InputData.MoveInput.x;
            // inputs.CameraRotation = AGameManager.Instance.PlayerCamera.GetCameraRotation();
            Vector3 moveInputVector = Vector3.zero, cameraPlanarDirection = Vector3.zero;
            switch (_cameraMode)
            {
                case ECameraMode.FirstPerson:
                    //handle input
                    moveInputVector =
                        Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

                    // Calculate camera direction and rotation on the character plane
                    cameraPlanarDirection =
                        Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
                    if (cameraPlanarDirection.sqrMagnitude == 0f)
                    {
                        cameraPlanarDirection = Vector3
                            .ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp)
                            .normalized;
                    }

                    Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);
                    //生成实际的输入
                    _moveInputVector = cameraPlanarRotation * moveInputVector;
                    _lookInputVector = cameraPlanarDirection;
                    return;
                case ECameraMode.TopDownShot:
                    _moveInputVector =
                        Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);
                    var dir = Vector3.ProjectOnPlane(Motor.Transform.rotation * Vector3.forward, Motor.CharacterUp)
                        .normalized;
                    if (_moveInputVector.sqrMagnitude == 0)
                    {
                        _climbUp = 0;
                    }
                    else
                    {
                        _climbUp = (Vector3.Dot(dir, _moveInputVector) < 0 ? -1f : 1f);
                    }

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    Plane groundPlane = new Plane(Vector3.up, new Vector3(0, Motor.transform.position.y, 0));

                    float distance;
                    if (groundPlane.Raycast(ray, out distance))
                    {
                        // 计算射线与水平面的交点
                        Vector3 mouseWorldPosition = ray.GetPoint(distance);

                        // 计算方向并忽略Y轴高度
                        _lookInputVector = mouseWorldPosition - Motor.Transform.position;
                        _lookInputVector.y = 0;
                        if (_lookInputVector.sqrMagnitude < 0.16f)
                        {
                            _lookInputVector = Vector3.zero;
                        }
                    }
                    else
                    {
                        // 如果没有找到交点，使用枪口的前方方向作为默认值
                        _lookInputVector = Vector3.zero;
                    }


                    return;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            switch (_cameraMode)
            {
                case ECameraMode.FirstPerson:
                    if (_lookInputVector.sqrMagnitude > 0f && _fsm.CurrentState.name != EPlayerState.Climb)
                    {
                        currentRotation = Quaternion.LookRotation(_lookInputVector, Motor.CharacterUp);
                    }

                    break;
                case ECameraMode.TopDownShot:
                    if (_lookInputVector.sqrMagnitude > 0f && _fsm.CurrentState.name != EPlayerState.Climb)
                    {
                        Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector,
                            1 - Mathf.Exp(-10 * deltaTime)).normalized;
                        currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
                    }

                    break;
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (_fsm.CurrentState is IKcc kcc)
            {
                kcc.UpdateVelocity(ref currentVelocity, deltaTime);
            }

            if (_superJumpRequest)
            {
                Motor.ForceUnground();
                _fsm.RequestStateChange(EPlayerState.Air, forceInstantly: true);
                _superJumpRequest = false;
                currentVelocity.y = _superJumpVelocity;
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

        #region Inspector

        private void OnMouseGainChange()
        {
            if (!Application.isPlaying) return;
            AGameManager.Instance.PlayerCamera.SetInputAxisGain(MouseGain);
        }

        #endregion

        public void GetMaterialBullet(EMaterial eMaterial)
        {
            _materialGun.GetMaterialBullet(eMaterial);
            _playerTip.ShowGetBulletIcon();
        }

        public void ChangeMaterialGunMat(EMaterial eMaterial)
        {
            _materialGun.ChangeMaterialGunMat(eMaterial);
        }

        public void OnHaveNoBullet()
        {
            _playerTip.ShowNoBulletIcon();
        }
    }
}