using Assets.Scripts.Audio;
using Assets.Scripts.Input;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.RayCasts;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.StateMachine.PlayerStateHandlers
{
    internal class DashStateHandler : StateHandlerBase
    {
        [Header("References")]
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private AnimationReferenceAsset dashLeftAnimation, dashRightAnimation, dashUpAnimation, dashDownAnimation;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private FacingDirection facingDirectionController;
        [SerializeField] private PlayerStates crashState, idleState;
        [SerializeField] private BoxCaster castUtility;

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 14f;
        [SerializeField] private float dashDuration = 0.2f;
        [SerializeField] private float minVelocityForDirection = 0.1f;

        private Vector2 dashDirection;
        private float dashTimer;
        private float startDamping;
        private IInput input;

        private void Awake() => input = transform.root.GetComponent<IInput>();

        internal override void OnEnter(int state)
        {
            base.OnEnter(state);

            // Determine dash direction based on current velocity
            Vector2 currentVelocity = rb2d.linearVelocity;
            if (currentVelocity.magnitude >= minVelocityForDirection)
            {
                // Normalize the velocity to get the dash direction
                dashDirection = input.GetMoveVector().normalized;
                Debug.Log($"Dash Vector {input.GetMoveVector().normalized}");
            }
            else
            {
                // Use facing direction if not moving
                var facing = facingDirectionController.GetCurrentFacing();
                switch (facing)
                {
                    case FacingDirectionEnum.Left:
                        dashDirection = Vector2.left;
                        break;
                    case FacingDirectionEnum.Right:
                        dashDirection = Vector2.right;
                        break;
                }
            }

            PlayDirectionalAnimation();


            // Save the current damping and prepare for dash
            startDamping = rb2d.linearDamping;
            rb2d.linearVelocity = Vector2.zero;
            rb2d.linearDamping = 0f;

            dashTimer = dashDuration;

            // Apply the dash velocity
            rb2d.linearVelocity = dashDirection * dashSpeed;
            audioController.PlayOneShot();
        }

        private void PlayDirectionalAnimation()
        {
            AnimationReferenceAsset animation = dashRightAnimation;
            if (dashDirection.x < 0)
                animation = dashLeftAnimation;
            else if (dashDirection.x > 0)
                animation = dashRightAnimation;
            else if (dashDirection == Vector2.up)
                animation = dashUpAnimation;
            else if (dashDirection == Vector2.down)
                animation = dashDownAnimation;

            animationHandler.PlayAnimationReference(animation, 1, false, true);
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (!IsInCurrentHandlerState())
                return;

            dashTimer -= Time.fixedDeltaTime;

            // Maintain constant dash velocity
            rb2d.linearVelocity = dashDirection * dashSpeed;

            var castResult = castUtility.Cast();
            if (castResult.collider != null)
            {
                rb2d.linearVelocity = Vector2.zero;
                SetState(crashState);
                return;
            }

            if (dashTimer <= 0f)
            {
                rb2d.linearVelocity = Vector2.zero;
                SetState(idleState);
            }
        }

        internal override void OnExit()
        {
            base.OnExit();

            // Reset Values
            rb2d.linearDamping = startDamping;
            dashTimer = 0f;
        }
    }
}