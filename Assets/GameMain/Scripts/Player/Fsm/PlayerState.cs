using System;
using KinematicCharacterController;
using UnityEngine;
using UnityHFSM;

namespace Tencent
{
    public class PlayerState : State<EPlayerState>
    {
        protected Player _player => _fsm.Player;
        protected PlayerFsm _fsm => (PlayerFsm)fsm;
        protected KinematicCharacterMotor Motor => _player.Motor;
        protected Vector3 _moveInputVector => _player.MoveInputVector;
        protected Vector3 _lookInputVector => _player.LookInputVector;

        public PlayerState(
            Action<State<EPlayerState, string>> onEnter = null,
            Action<State<EPlayerState, string>> onLogic = null,
            Action<State<EPlayerState, string>> onExit = null,
            Func<State<EPlayerState, string>, bool> canExit = null,
            bool needsExitTime = false,
            bool isGhostState = false)
            : base(
                onEnter,
                onLogic,
                onExit,
                canExit,
                needsExitTime: needsExitTime,
                isGhostState: isGhostState)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"on enter {name}");
        }
    }
}