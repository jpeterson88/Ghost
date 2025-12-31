using Assets.Scripts.StateMachine.Enums;

namespace Assets.Scripts.StateMachine
{
    class PlayerStateMachine : StateMachine<PlayerStates>
    {
        public PlayerStates GetPlayerStateEnum() => (PlayerStates)(GetCurrentState());
    }
}