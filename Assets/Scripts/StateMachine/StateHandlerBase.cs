using Assets.Scripts.StateMachine.Enums;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    internal class StateHandlerBase : MonoBehaviour
    {
        [SerializeField] protected PlayerStates[] canHandleStates;

        private StateMachine<PlayerStates> stateMachine;

        //TODO: this should be Awake and children statehandlers start
        private void Start()
        {
            stateMachine = GetComponentInParent<StateMachine<PlayerStates>>();

            if (stateMachine == null)
                throw new NullReferenceException(nameof(stateMachine));
            if(canHandleStates.Length == 0)
                throw new Exception("No canHandleStates found.");
        }

        internal bool CanHandle(int desiredState)
        {
            return canHandleStates.Any(x => (int)x == desiredState);
        }

        protected bool IsInCurrentHandlerState()
        {
            return canHandleStates.Any(x => x == GetCurrentState());
        }

        protected void SetState(PlayerStates desiredState)
        {
            stateMachine.SetState((int)desiredState);
        }

        internal PlayerStates GetCurrentState()
        {
            return (PlayerStates)stateMachine.GetCurrentState();
        }

        internal virtual void OnEnter(int state)
        {
        }

        internal virtual void OnExit()
        {
        }

        internal virtual void OnFixedUpdate()
        {
        }

        internal virtual void OnUpdate()
        {
        }
    }
}