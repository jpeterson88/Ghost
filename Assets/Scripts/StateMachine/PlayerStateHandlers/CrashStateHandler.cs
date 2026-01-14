using Assets.Scripts.StateMachine.Enums;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.StateMachine.PlayerStateHandlers
{
    internal class CrashStateHandler : StateHandlerBase
    {
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private AnimationReferenceAsset crashAnimation;
        [SerializeField] private PlayerStates nextState;
        [SerializeField] private float stunTime = .5f;

        private bool hasStarted;
        private float currentStunDuration;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
            animationHandler.PlayAnimationReference(crashAnimation, 1, false, false);
            hasStarted = true;
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();

            if (IsInCurrentHandlerState() && hasStarted)
            {
                currentStunDuration += Time.deltaTime;
                if (currentStunDuration >= stunTime)
                    SetState(nextState);
            }
        }

        internal override void OnExit()
        {
            base.OnExit();
            currentStunDuration = 0f;
        }
    }
}
