using Assets.Scripts.Audio;
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
        [SerializeField] private AnimationReferenceAsset dashLeftAnimation, dashRightAnimation;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private FacingDirection facingDirectionController;
        [SerializeField] private PlayerStates crashState, idleState;
        [SerializeField] private BoxCaster castUtility;

        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 14f;
        [SerializeField] private float dashDuration = 0.2f;

        private Vector2 dashDirection;
        private float dashTimer;
        private float startDamping;

        internal override void OnEnter(int state)
        {
            base.OnEnter(state);

            var facing = facingDirectionController.GetCurrentFacing();
            dashDirection = facing == FacingDirectionEnum.Left ? Vector2.left : Vector2.right;

            var animation = facing == FacingDirectionEnum.Left
                ? dashLeftAnimation
                : dashRightAnimation;

            animationHandler.PlayAnimationReference(animation, 1, false, true);

            startDamping = rb2d.linearDamping;

            // Prepare rigidbody for dash
            rb2d.linearVelocity = Vector2.zero;
            rb2d.linearDamping = 0f;

            dashTimer = dashDuration;

            // Apply dash immediately
            rb2d.linearVelocity = dashDirection * dashSpeed;
            audioController.PlayOneShot();
        }

        internal override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (!IsInCurrentHandlerState())
                return;

            dashTimer -= Time.fixedDeltaTime;

            // Maintain constant dash velocity
            rb2d.linearVelocity = dashDirection * dashSpeed;

            // Collision check
            var castResult = castUtility.Cast();
            if (castResult.collider != null)
            {
                rb2d.linearVelocity = Vector2.zero;
                SetState(crashState);
                return;
            }

            // Dash finished
            if (dashTimer <= 0f)
            {
                rb2d.linearVelocity = Vector2.zero;
                SetState(idleState);
            }
        }

        internal override void OnExit()
        {
            base.OnExit();

            rb2d.linearDamping = startDamping;
            dashTimer = 0f;
        }
    }
}
