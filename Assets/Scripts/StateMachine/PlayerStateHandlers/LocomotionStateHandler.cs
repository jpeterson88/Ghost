using Assets.Scripts.Audio;
using Assets.Scripts.Ghost;
using Assets.Scripts.Input;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.StateMachine.PlayerStateHandlers;
using Assets.Scripts.Utility;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class LocomotionStateHandler : StateHandlerBase
    {
        [SerializeField] private PlayerStates idleState, spook1State, castCurseState, dashState;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private AnimationReferenceAsset right, left, upward, downward, idleLeft, idleRight;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        
        [SerializeField] private FacingDirection directionUtility;
        [SerializeField] private UncachedAudioController audioController;
        [SerializeField] private CastCurseController curseController;
        [SerializeField] private GhostMovement movementController;
        [SerializeField] private DashStateHandler dashStateHandler;
        [SerializeField] private float stopMagnitude = 1.5f;

        private TrackEntry currentIdleTrack;
        private TrackEntry currentMovementTrack;
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
            audioController.Play();
        }


        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            
            if (IsInCurrentHandlerState())
            {
                movementController.ApplyMovement();
                Direction direction = GetMoveDirection(input.GetMoveVector());

                PlayDirectionalMovementAnimation(direction);
                if (rb2d.linearVelocity.magnitude < stopMagnitude)
                    SetState(idleState);
            }
        }

        private void PlayDirectionalMovementAnimation(Direction direction)
        {
            if (!animationHandler.CompareTrackName(1, upward.name) && direction == Direction.Up)
            {
                currentMovementTrack = animationHandler.PlayAnimationReference(upward, 1, false, true);
            }
            else if (!animationHandler.CompareTrackName(1, right.name) && (direction == Direction.Right || direction == Direction.UpRight || direction == Direction.DownRight))
            {
                directionUtility.SetFacingDirection(FacingDirectionEnum.Right);
                currentMovementTrack = animationHandler.PlayAnimationReference(right, 1, false, true);
            }
            else if (!animationHandler.CompareTrackName(1, left.name) && (direction == Direction.Left || direction == Direction.UpLeft || direction == Direction.DownLeft))
            {
                directionUtility.SetFacingDirection(FacingDirectionEnum.Left);
                currentMovementTrack = animationHandler.PlayAnimationReference(left, 1, false, true);
            }
            else if (!animationHandler.CompareTrackName(1, downward.name) && direction == Direction.Down)
            {
                currentMovementTrack = animationHandler.PlayAnimationReference(downward, 1, false, true);
            }
            else if (!animationHandler.CompareTrackName(1, downward.name) && direction == Direction.Down)
            {
                currentMovementTrack = animationHandler.PlayAnimationReference(downward, 1, false, true);
            }
        }


        private Direction GetMoveDirection(Vector2 input)
        {
            if (input == Vector2.zero)
                return Direction.None;

            // Normalize the input to handle diagonal movement
            input.Normalize();

            // Determine the direction based on input vector
            if (input.y > 0.5f)
            {
                if (input.x > 0.5f)
                    return Direction.UpRight;
                else if (input.x < -0.5f)
                    return Direction.UpLeft;
                else
                    return Direction.Up;
            }
            else if (input.y < -0.5f)
            {
                if (input.x > 0.5f)
                    return Direction.DownRight;
                else if (input.x < -0.5f)
                    return Direction.DownLeft;
                else
                    return Direction.Down;
            }
            else
            {
                if (input.x > 0.5f)
                    return Direction.Right;
                else if (input.x < -0.5f)
                    return Direction.Left;
            }

            return Direction.None;
        }

        internal override void OnExit()
        {
            base.OnExit();
            //animationHandler.ClearTrack(1);
            audioController.Stop(true);
        }

        private void OnDisable()
        {
            input.CursePressed -= HandleCuresePressed;
            input.DashPressed -= HandleDashPressed;
        }
    }
}