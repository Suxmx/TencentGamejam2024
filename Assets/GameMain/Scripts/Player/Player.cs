using System;
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
        public bool JumpDown;
        public bool CrouchDown;
        public bool CrouchUp;
    }

    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor => _motor;

        private PlayerInput _input;
        private Transform _eye;
        private KinematicCharacterMotor _motor;
        private CinemachineCamera _cinemachine;

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
        }

        private void FixedUpdate()
        {
        }

        #region Init

        private void InitComponents()
        {
            _input = GetComponent<PlayerInput>();
            _motor = GetComponent<KinematicCharacterMotor>();
            _cinemachine = FindAnyObjectByType<CinemachineCamera>();

            _eye = transform.Find("Eye");

            _motor.CharacterController = this;
        }

        private void InitFsm()
        {
        }

        #endregion

        #region Kcc

        [BoxGroup("KCC"), LabelText("地面移动速度")] public float GroundMoveSpeed = 15f;

        [BoxGroup("KCC/空中"), LabelText("空中移动速度")]
        public float AirMoveSpeed = 10f;

        [BoxGroup("KCC/空中"), LabelText("空中移动加速度")]
        public float AirAccelerationSpeed = 15f;

        [BoxGroup("KCC/空中"), LabelText("空中下降阻力")]
        public float Drag = 0.1f;

        [BoxGroup("KCC/空中"), LabelText("重力")] public Vector3 Gravity = new Vector3(0, -30f, 0);

        [BoxGroup("KCC/跳跃"), LabelText("跳跃间隔时间")]
        public float JumpPreGroundingGraceTime = 0.1f;

        [BoxGroup("KCC/跳跃"), LabelText("跳跃速度")]
        public float JumpUpSpeed = 10f;

        [BoxGroup("KCC/跳跃"), LabelText("跳跃获得向前速度")]
        public float JumpScalableForwardSpeed = 10f;

        private Vector3 _moveInputVector, _lookInputVector;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;


        private void HandleCharacterInput()
        {
            PlayerCharacterInputs inputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            inputs.MoveAxisForward = InputData.MoveInput.y;
            inputs.MoveAxisRight = InputData.MoveInput.x;
            inputs.CameraRotation = _cinemachine.transform.rotation;
            inputs.JumpDown = InputData.HasEventStart(InputEvent.Jump);
            inputs.CrouchDown = Input.GetKeyDown(KeyCode.C);
            inputs.CrouchUp = Input.GetKeyUp(KeyCode.C);

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
            if (inputs.JumpDown)
            {
                _timeSinceJumpRequested = 0f;
                _jumpRequested = true;
            }
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector.sqrMagnitude > 0f)
            {
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector,
                    1 - Mathf.Exp(-1 * deltaTime)).normalized;
                // 设置当前旋转
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                float currentVelocityMagnitude = currentVelocity.magnitude;

                Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                                  currentVelocityMagnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                          _moveInputVector.magnitude;
                Vector3 targetMovementVelocity = reorientedInput * GroundMoveSpeed;

                // Smooth movement Velocity
                currentVelocity =
                    Vector3.Lerp(currentVelocity, targetMovementVelocity, 1f - Mathf.Exp(-10 * deltaTime));
            }
            else
            {
                // Add move input
                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = _moveInputVector * AirAccelerationSpeed * deltaTime;

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < AirMoveSpeed)
                    {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity,
                            AirMoveSpeed);
                        addedVelocity = newTotal - currentVelocityOnInputsPlane;
                    }
                    else
                    {
                        // Make sure added vel doesn't go in the direction of the already-exceeding velocity
                        if (Vector3.Dot(currentVelocityOnInputsPlane, addedVelocity) > 0f)
                        {
                            addedVelocity =
                                Vector3.ProjectOnPlane(addedVelocity, currentVelocityOnInputsPlane.normalized);
                        }
                    }

                    // Prevent air-climbing sloped walls
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        if (Vector3.Dot(currentVelocity + addedVelocity, addedVelocity) > 0f)
                        {
                            Vector3 perpenticularObstructionNormal = Vector3
                                .Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal),
                                    Motor.CharacterUp).normalized;
                            addedVelocity = Vector3.ProjectOnPlane(addedVelocity, perpenticularObstructionNormal);
                        }
                    }

                    // Apply added velocity
                    currentVelocity += addedVelocity;
                }

                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }

            // Handle jumping
            _jumpedThisFrame = false;
            _timeSinceJumpRequested += deltaTime;
            if (_jumpRequested)
            {
                // See if we actually are allowed to jump
                if (!_jumpConsumed && ((Motor.GroundingStatus.FoundAnyGround) || _timeSinceLastAbleToJump <= 0.1f))
                {
                    // Calculate jump direction before ungrounding
                    Vector3 jumpDirection = Motor.CharacterUp;
                    if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                    {
                        jumpDirection = Motor.GroundingStatus.GroundNormal;
                    }

                    // Makes the character skip ground probing/snapping on its next update. 
                    // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                    Motor.ForceUnground();

                    // Add to the return velocity and reset jump state
                    currentVelocity += (jumpDirection * JumpUpSpeed) -
                                       Vector3.Project(currentVelocity, Motor.CharacterUp);
                    currentVelocity += (_moveInputVector * JumpScalableForwardSpeed);
                    _jumpRequested = false;
                    _jumpConsumed = true;
                    _jumpedThisFrame = true;
                }
            }
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period
                if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                {
                    _jumpRequested = false;
                }

                if (Motor.GroundingStatus.FoundAnyGround)
                {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame)
                    {
                        _jumpConsumed = false;
                    }

                    _timeSinceLastAbleToJump = 0f;
                }
                else
                {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            ref HitStabilityReport hitStabilityReport)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint,
            Vector3 atCharacterPosition,
            Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        #endregion
    }
}