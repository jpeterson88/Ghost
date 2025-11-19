using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Spine.Unity;
using UnityEditor.Tilemaps;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class IdleStateHandler : StateHandlerBase
    {
        [SerializeField] private PlayerStates locomotionState, spook1State;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private AnimationReferenceAsset idle;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
            animationHandler.PlayAnimationReference(idle, 0, true, false);
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();

            if (IsInCurrentHandlerState())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    SetState(spook1State);
                }
            }
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (IsInCurrentHandlerState()) 
            {
                if (rb2d.linearVelocity != Vector2.zero)
                {
                    SetState(locomotionState);
                }
            }                
        }

        internal override void OnExit()
        {
            base.OnExit();
        }
    }
}