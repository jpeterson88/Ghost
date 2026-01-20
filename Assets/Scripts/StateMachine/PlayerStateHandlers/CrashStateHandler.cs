using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.StateMachine.PlayerStateHandlers
{
    internal class CrashStateHandler : StateHandlerBase
    {
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private FacingDirection directionController;
        [SerializeField] private AnimationReferenceAsset bumpLeft, bumpRight;
        [SerializeField] private PlayerStates nextState;
        [SerializeField] private float stunTime = .5f;
        [SerializeField] private float animTrackSpeed = 1f;

        private bool hasStarted;
        private float currentStunDuration;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);

            var animation =  directionController.GetCurrentFacing() == FacingDirectionEnum.Left ? bumpLeft : bumpRight;
            animationHandler.PlayAnimationReference(animation, 1, false, false, animTrackSpeed);
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
