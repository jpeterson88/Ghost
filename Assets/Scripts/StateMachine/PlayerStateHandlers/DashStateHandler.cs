using Assets.Scripts.Audio;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using Assets.Scripts.Utility.RayCasts;
using Mono.Cecil;
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
        [SerializeField] private float dashForce = 20f;
        [SerializeField] private float stopMagnitude = 1.5f;
        
        
        

        private bool isDashing;
        private Vector2 directionalDashVector;

        internal override void OnEnter(int state)
        {
            base.OnEnter(state);

            var facingDirection = facingDirectionController.GetCurrentFacing();
            var animation = facingDirection == FacingDirectionEnum.Left ? dashLeftAnimation : dashRightAnimation;
            directionalDashVector = facingDirection == FacingDirectionEnum.Left ? Vector2.left : Vector2.right;

            animationHandler.PlayAnimationReference(animation, 1, false, true);

            rb2d.linearVelocity = Vector2.zero;
            rb2d.AddForce(directionalDashVector.normalized * dashForce, ForceMode2D.Impulse);
            isDashing = true;
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();

            if (!isDashing || !IsInCurrentHandlerState())
                return;

            if (rb2d.linearVelocity.magnitude < stopMagnitude)                
            {
                SetState(idleState);
            }
            else
            {
                var castResult = castUtility.Cast();

                // We collide with something
                if (castResult.collider != null)
                {
                    rb2d.linearVelocity = Vector2.zero;

                    if (IsInCurrentHandlerState())
                        SetState(crashState);
                }
            }
        }

        internal override void OnExit()
        {
            base.OnExit();
            isDashing = false;
        }


    }
}