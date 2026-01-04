using Assets.Scripts.Ghost;
using Assets.Scripts.Input;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.StateMachine.PlayerStateHandlers;
using Assets.Scripts.Utility;
using Spine.Unity;
using System;
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
        [SerializeField] private DashStateHandler dashStateHandler;

        private IInput input;

        private void Awake()
        {
            input = transform.root.GetComponent<IInput>();

            input.CursePressed += HandleCuresePressed;
            input.DashPressed += HandleDashPressed;
        }

        private void HandleDashPressed()
        {
            if (IsInCurrentHandlerState() && dashStateHandler.CanDash())
                SetState(dashState);
        }

        private void HandleCuresePressed()
        {
            if (IsInCurrentHandlerState() && curseController.CanCurse())
                SetState(castCurseState);
        }

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

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (IsInCurrentHandlerState()) 
            {
                movementController.ApplyMovement();

                if (input.GetMoveVector() != Vector2.zero)
                    SetState(locomotionState);
            }                
        }

        internal override void OnExit()
        {
            base.OnExit();
        }

        private void OnDisable()
        {
            input.CursePressed -= HandleCuresePressed;
            input.DashPressed -= HandleDashPressed;
        }
    }
}