using System;
using KinematicCharacterController;
using UnityEngine;
using UnityHFSM;

namespace Tencent
{
    public class GroundCrouchState : PlayerState, IKcc
    {
        public GroundCrouchState(Action<State<EPlayerState, string>> onEnter = null,
            Action<State<EPlayerState, string>> onLogic = null, Action<State<EPlayerState, string>> onExit = null,
            Func<State<EPlayerState, string>, bool> canExit = null, bool needsExitTime = false,
            bool isGhostState = false) : base(onEnter, onLogic, onExit, canExit, needsExitTime, isGhostState)
        {
        }

        private Collider[] _cacheColliders = new Collider[8];

        public override void OnEnter()
        {
            base.OnEnter();
            _player.CanStandupWhenCrouching = false;
            _player.DoCrouch();
        }

        public override void OnExit()
        {
            base.OnExit();
            _player.DoStandUp();
        }

        private void CheckStandup()
        {
            _player.DoStandUp();
            if (Motor.CharacterOverlap(
                    Motor.TransientPosition,
                    Motor.TransientRotation,
                    _cacheColliders,
                    Motor.CollidableLayers,
                    QueryTriggerInteraction.Ignore) > 0)
            {
                //如果试图站起来时被阻挡了就继续蹲着

                _player.CanStandupWhenCrouching = false;
            }
            else
            
            {
                _player.CanStandupWhenCrouching = true;
            }

            _player.DoCrouch();
        }

        #region KCC

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            float currentVelocityMagnitude = currentVelocity.magnitude;

            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

            //将Motor速度重新映射到平面上
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;

            // 将输入的速度重新映射到平面上
            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                      _moveInputVector.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * _player.CrouchMoveSpeed;

            //插值速度
            currentVelocity = targetMovementVelocity;
        }

        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AfterCharacterUpdate(float deltaTime)
        {
            CheckStandup();
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