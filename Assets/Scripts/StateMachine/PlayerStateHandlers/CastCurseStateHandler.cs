using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Spine.Unity;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
    class CastCurseStateHandler: StateHandlerBase
    {
        [SerializeField] private PlayerStates idleState;
        [SerializeField] private AnimationReferenceAsset castCurseAnim;
        [SerializeField] private SpineSkeletonAnimationHandle animationHandler;
        [SerializeField] private string castCurseSkinName;
        [SerializeField] private float castTime, animationPlaybackSpeed = 1f;

        private float currentElapsed;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
            animationHandler.SetSkin(castCurseSkinName);
            animationHandler.PlayAnimationReference(castCurseAnim, 1, false, true, animationPlaybackSpeed);
            currentElapsed = 0f;
        }

        internal override void OnUpdate()
        {
            base.OnUpdate();

            if (IsInCurrentHandlerState())
            {
                currentElapsed += Time.deltaTime;

                if (currentElapsed >= castTime)
                {
                    // TODO: Inform game of successful cast
                    SetState(idleState);
                }
                else if (Input.GetKeyUp(KeyCode.R))
                {
                    
                    SetState(idleState);
                }
            }
        }


        internal override void OnExit()
        {
            base.OnExit();
            //TODO: Start cast cooldown timer
        }
    }
}