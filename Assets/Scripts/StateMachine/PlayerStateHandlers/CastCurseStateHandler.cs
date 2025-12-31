using Assets.Scripts.Audio;
using Assets.Scripts.Ghost;
using Assets.Scripts.Input;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using Spine.Unity;
using System;
using System.Collections;
using Unity.Cinemachine;
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
        [SerializeField] private CameraPriorityUtility cameraPriorityUtility;
        [SerializeField] private CinemachineCamera main, close;
        [SerializeField] private string castCurseSkinName, normalSkinName;
        [SerializeField] private float castTime, animationPlaybackSpeed = 1f, waitToSetSkinOnExit = .5f;
        

        private float currentElapsed;
        private IInput input;
        private bool curseReleased;

        private void Awake()
        {
            input = transform.root.GetComponent<IInput>();

            input.CurseReleased += HandleCurseReleased;
        }

        private void HandleCurseReleased()
        {
            if(IsInCurrentHandlerState())
                curseReleased = true;
        }

        internal override void OnEnter(int state)
        {
            base.OnEnter(state);            

            curseController.StartCurse();
            animationHandler.SetSkin(castCurseSkinName);
            animationHandler.PlayAnimationReference(castCurseAnim, 1, false, true, animationPlaybackSpeed);
            audioController.PlayOneShot();
            currentElapsed = 0f;
            cameraPriorityUtility.SwitchCameras(close);
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
                else if (curseReleased)
                {
                    curseController.EndCurse();
                    SetState(idleState);
                }
            }
        }

        // TODO: Move this to maybe a GhostSkinHandler
        private IEnumerator WaitToSetSkin()
        {
            cameraPriorityUtility.SwitchCameras(main);
            yield return new WaitForSeconds(waitToSetSkinOnExit);
            animationHandler.SetSkin(normalSkinName);
        }

        private void OnDisable() => input.CurseReleased -= HandleCurseReleased;

        internal override void OnExit()
        {
            base.OnExit();
            curseReleased = false;

            animationHandler.ClearTrack(1);
            curseController.StartCooldown();
            audioController.Stop();

            StartCoroutine(WaitToSetSkin());
        }
    }
}