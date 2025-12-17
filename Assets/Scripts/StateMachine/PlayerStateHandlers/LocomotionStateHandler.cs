using Assets.Scripts.Audio;
using Assets.Scripts.Ghost;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class LocomotionStateHandler : StateHandlerBase
    {
        [SerializeField] private PlayerStates idleState, spook1State, castCurseState;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private AnimationReferenceAsset right, left, upward, downward, idleLeft, idleRight;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private float stopMagnitude = 1.5f;
        [SerializeField] private FacingDirection directionUtility;
        [SerializeField] private UncachedAudioController audioController;
        [SerializeField] private CastCurseController curseController;

        private TrackEntry currentIdleTrack;
        private TrackEntry currentMovementTrack;

        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
            audioController.Play();
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();

            if (IsInCurrentHandlerState())
            {
                if (Input.GetKeyDown(KeyCode.Space))                
                    SetState(spook1State);                
                else if (Input.GetKeyDown(KeyCode.R) && !curseController.IsOnCooldown())
                    SetState(castCurseState);
            }
        }



        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();


            if (IsInCurrentHandlerState())
            {
                Direction direction = GetMoveDirection(GetInputVector());

                PlayDirectionalMovementAnimation(direction);
                if (rb2d.linearVelocity.magnitude < stopMagnitude)
                    SetState(idleState);
            }
        }

        private void PlayDirectionalIdleAnimation(Direction direction, bool playBothTracks)
        {
            FacingDirectionEnum facingDirection = directionUtility.GetCurrentFacing();

            

            if (currentIdleTrack != null)
            {             
                if (facingDirection == FacingDirectionEnum.Left && currentIdleTrack.Animation.Name != idleLeft.name)
                {
                    currentIdleTrack = animationHandler.PlayAnimationReference(idleLeft, 0, false, true);

                    if (playBothTracks)
                    {
                        animationHandler.ClearTrack(1);
                        currentIdleTrack = animationHandler.PlayAnimationReference(idleLeft, 1, false, true);
                    }
                }
                else if (facingDirection == FacingDirectionEnum.Right && currentIdleTrack.Animation.Name != idleRight.name)
                {
                    currentIdleTrack = animationHandler.PlayAnimationReference(idleRight, 0, false, true);

                    if(playBothTracks)
                    {
                        animationHandler.ClearTrack(1);
                        animationHandler.PlayAnimationReference(idleLeft, 1, false, true);
                    }
                }
            }
            else
            {
                AnimationReferenceAsset directionalIdle = facingDirection == FacingDirectionEnum.Left ? idleLeft : idleRight;
                currentIdleTrack = animationHandler.PlayAnimationReference(directionalIdle, 0, false, true);
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

        private Vector2 GetInputVector()
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");
            Vector2 input = new Vector2(inputX, inputY);

            return input;
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
    }
}