using KinematicCharacterController;
using UnityEngine;

namespace Tencent
{
    public class GroundMoveState : PlayerState, IKcc
    {
        #region KCC

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            if(!Motor.GroundingStatus.FoundAnyGround) return;
            float currentVelocityMagnitude = currentVelocity.magnitude;

            Vector3 effectiveGroundNormal = Motor.GroundingStatus.GroundNormal;

            //将Motor速度重新映射到平面上
            currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, effectiveGroundNormal) *
                              currentVelocityMagnitude;

            // 将输入的速度重新映射到平面上
            Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
            Vector3 reorientedInput = Vector3.Cross(effectiveGroundNormal, inputRight).normalized *
                                      _moveInputVector.magnitude;
            Vector3 targetMovementVelocity = reorientedInput * _player.GroundMoveSpeed;

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