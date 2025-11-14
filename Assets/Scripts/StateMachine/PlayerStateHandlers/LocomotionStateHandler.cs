using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class LocomotionStateHandler : StateHandlerBase
    {
        [SerializeField] private PlayerStates nextState;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private AnimationReferenceAsset right, left, upward, downward;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private float stopMagnitude = 1.5f;

        private TrackEntry recentTrack;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();


            PlayAnimationBasedOnDirection();

            if (IsInCurrentHandlerState() && rb2d.linearVelocity.magnitude < stopMagnitude)
                SetState(nextState);
        }

        private void PlayAnimationBasedOnDirection()
        {
            Direction direction = GetMoveDirection(GetInputVector());
            

            var trackPlaying = animationHandler.GetCurrentTrack(1);

            if(trackPlaying != null)
                Debug.Log(trackPlaying.ToString());

            if (!animationHandler.CompareTrackName(1, upward.name) && direction == Direction.Up)
            {
                recentTrack = animationHandler.PlayAnimationReference(upward, 1, false, true);
            }
            else if (!animationHandler.CompareTrackName(1, right.name) && (direction == Direction.Right || direction == Direction.UpRight || direction == Direction.DownRight))
            {
                recentTrack = animationHandler.PlayAnimationReference(right, 1, false, true);
            }
            else if (!animationHandler.CompareTrackName(1, left.name) && (direction == Direction.Left || direction == Direction.UpLeft || direction == Direction.DownLeft))
            {
                recentTrack = animationHandler.PlayAnimationReference(left, 1, false, true);
            }
            else if(!animationHandler.CompareTrackName(1, downward.name) && direction == Direction.Down)
            {
                recentTrack = animationHandler.PlayAnimationReference(downward, 1, false, true);
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
            animationHandler.ClearTrack(1);
        }
    }
}