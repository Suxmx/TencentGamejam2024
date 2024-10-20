using UnityHFSM;

namespace Tencent
{
    public class PlayerFsm : StateMachine<EPlayerState>
    {
        public PlayerFsm(Player player, bool needsExitTime = false, bool isGhostState = false,
            bool rememberLastState = false) : base(needsExitTime, isGhostState, rememberLastState)
        {
            Player = player;
        }

        public PlayerState CurrentState => GetState();
        public Player Player;

        public PlayerState GetState()
        {
            var t = ActiveState;
            while (t is PlayerFsm playerFsm)
            {
                t = playerFsm.ActiveState;
            }
            
            return (PlayerState)t;
        }
    }
}