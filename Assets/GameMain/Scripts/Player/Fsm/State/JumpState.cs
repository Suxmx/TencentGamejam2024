using System;
using KinematicCharacterController;
using UnityEngine;
using UnityHFSM;

namespace Tencent
{
    public class JumpState : PlayerState, IKcc
    {
        private bool _jumped;

        #region KCC

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if (!_jumped)
            {
                _jumped = true;

                Vector3 jumpDirection = Motor.CharacterUp;
                if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                {
                    jumpDirection = Motor.GroundingStatus.GroundNormal;
                }

                Motor.ForceUnground();
                currentVelocity += (jumpDirection * _player.JumpUpSpeed) -
                                   Vector3.Project(currentVelocity, Motor.CharacterUp);
            }
            else
            {
                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    Vector3 addedVelocity = _moveInputVector * (15 * deltaTime);

                    Vector3 currentVelocityOnInputsPlane = Vector3.ProjectOnPlane(currentVelocity, Motor.CharacterUp);

                    // Limit air velocity from inputs
                    if (currentVelocityOnInputsPlane.magnitude < _player.AirMoveSpeed)
                    {
                        // clamp addedVel to make total vel not exceed max vel on inputs plane
                        Vector3 newTotal = Vector3.ClampMagnitude(currentVelocityOnInputsPlane + addedVelocity,
                            _player.AirMoveSpeed);
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
                currentVelocity += _player.Gravity * deltaTime;
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

        public JumpState(Action<State<EPlayerState, string>> onEnter = null,
            Action<State<EPlayerState, string>> onLogic = null, Action<State<EPlayerState, string>> onExit = null,
            Func<State<EPlayerState, string>, bool> canExit = null, bool needsExitTime = false,
            bool isGhostState = false) : base(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _jumped = false;
        }

        public override void OnLogic()
        {
            base.OnLogic();
            if (_jumped) _fsm.StateCanExit();
            if (timer.Elapsed > 0.2f && Motor.GroundingStatus.FoundAnyGround)
                _fsm.RequestStateChange(EPlayerState.GroundMove);
        }
    }
}