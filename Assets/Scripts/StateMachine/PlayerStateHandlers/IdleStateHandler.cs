using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class IdleStateHandler : StateHandlerBase
    {
        [SerializeField] private PlayerStates nextState;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private AnimationReferenceAsset idle;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
            animationHandler.PlayAnimationReference(idle, 0, true, false);
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            if (IsInCurrentHandlerState() && rb2d.linearVelocity != Vector2.zero)
                SetState(nextState);
        }

        internal override void OnExit()
        {
            base.OnExit();
        }
    }
}