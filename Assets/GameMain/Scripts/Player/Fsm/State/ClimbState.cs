using System;
using Framework;
using KinematicCharacterController;
using MyTimer;
using UnityEngine;
using UnityHFSM;

namespace Tencent
{
    public class ClimbState : PlayerState, IKcc
    {
        enum EClimbState
        {
            Anchor,
            Climb,
            EnterTop
        }

        private float _enterHeight;
        private EClimbState _state;

        public override void OnEnter()
        {
            base.OnEnter();
            _state = EClimbState.Anchor;
            Motor.ForceUnground();
            _enterHeight = Motor.Transform.position.y;
            Debug.Log("start climbing");
        }

        public override void OnExit()
        {
            base.OnExit();
            if (_state == EClimbState.Climb)
            {
                Motor.SetPosition(Motor.Transform.position + Motor.CharacterForward * -0.1f);
            }

            _player.ClimbingObj = null;
        }

        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
        }

        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            switch (_state)
            {
                case EClimbState.Anchor:
                    Motor.SetPosition(Motor.Transform.position + Motor.CharacterForward * 0.1f +
                                      Motor.CharacterUp * 0.2f);
                    _state = EClimbState.Climb;
                    break;
                case EClimbState.Climb:
                    float vertical = 0;
                    if (AGameManager.CameraMode == ECameraMode.FirstPerson)
                    {
                        vertical = (InputData.MoveInput.y < -0.1f) ? -1 : (InputData.MoveInput.y > 0.1f ? 1 : 0);
                    }

                    else
                    {
                        Debug.Log(_player.ClimbInput);
                        vertical = (_player.ClimbInput < -0.1f)
                            ? -1
                            : (_player.ClimbInput > 0.1f ? 1 : 0);
                    }

                    currentVelocity = Vector3.Lerp(currentVelocity, Vector3.up * (vertical * _player.ClimbSpeed),
                        0.2f);
                    var detectPos = _player.FootPosition;
                    if (!Physics.Raycast(detectPos, Motor.CharacterForward, 0.4f,
                            LayerMask.GetMask("Environment", "Ground", "Climbable"),
                            QueryTriggerInteraction.Collide))
                    {
                        _state = EClimbState.EnterTop;
                    }

                    break;
                case EClimbState.EnterTop:
                    currentVelocity = Vector3.zero;
                    Motor.SetPosition(Motor.Transform.position + Motor.CharacterForward * 0.1f +
                                      Motor.CharacterUp * 0.1f);
                    _fsm.Trigger("ClimbTop");
                    break;
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
    }
}