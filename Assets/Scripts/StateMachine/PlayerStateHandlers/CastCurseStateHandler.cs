using Assets.Scripts.Audio;
using Assets.Scripts.Ghost;
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
        [SerializeField] private CastCurseController curseController;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private string castCurseSkinName, normalSkinName;
        [SerializeField] private float castTime, animationPlaybackSpeed = 1f;

        private float currentElapsed;
        internal override void OnEnter(int state)
        {
            base.OnEnter(state);
            curseController.StartCurse();
            animationHandler.SetSkin(castCurseSkinName);
            animationHandler.PlayAnimationReference(castCurseAnim, 1, false, true, animationPlaybackSpeed);
            audioController.PlayOneShot();
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
                    curseController.CastSuccessfulCurse();
                    SetState(idleState);
                }
                else if (Input.GetKeyUp(KeyCode.R))
                {
                    curseController.EndCurse();
                    SetState(idleState);
                }
            }
        }


        internal override void OnExit()
        {
            base.OnExit();
            animationHandler.ClearTrack(1);
            animationHandler.SetSkin(normalSkinName);
            curseController.StartCooldown();
            audioController.Stop();            
        }
    }
}