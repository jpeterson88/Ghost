using Assets.Scripts.Ghost;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class IdleStateHandler : StateHandlerBase
    {
        [SerializeField] private PlayerStates locomotionState, dashState, castCurseState;
        [SerializeField] private AnimationReferenceAsset idleLeft, idleRight;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private FacingDirection facingDirection;
        [SerializeField] private CastCurseController curseController;
        [SerializeField] private GhostMovement movementController;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);

            var currentFacing = facingDirection.GetCurrentFacing();
            
            if(currentFacing == FacingDirectionEnum.Left)
            {
                animationHandler.PlayAnimationReference(idleLeft, 0, false, true);
                animationHandler.PlayAnimationReference(idleLeft, 1, false, true);
            }
            else
            {
                animationHandler.PlayAnimationReference(idleRight, 0, false, true);
                animationHandler.PlayAnimationReference(idleRight, 1, false, true);
            }
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();

            if (IsInCurrentHandlerState())
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    SetState(dashState);
                else if (Input.GetKeyDown(KeyCode.R) && curseController.CanCurse())
                    SetState(castCurseState);
            }
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (IsInCurrentHandlerState()) 
            {
                movementController.ApplyMovement();
                float inputX = Input.GetAxisRaw("Horizontal");
                float inputY = Input.GetAxisRaw("Vertical");

                if (inputX != 0 || inputY != 0)
                    SetState(locomotionState);
            }                
        }

        internal override void OnExit()
        {
            base.OnExit();
        }
    }
}