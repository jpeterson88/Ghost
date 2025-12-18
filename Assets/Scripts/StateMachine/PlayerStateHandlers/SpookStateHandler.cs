using Assets.Scripts.Audio;
using Assets.Scripts.Ghost;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Assets.Scripts.Utility;
using FMODUnity;
using Spine;
using Spine.Unity;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets.Scripts.State.StateHandlers
{
	class SpookStateHandler : StateHandlerBase
	{
		[SerializeField] private PlayerStates nextState;
		[SerializeField] private Rigidbody2D rb2d;
		[SerializeField] private AnimationReferenceAsset spook1Anim;
		[SerializeField] private SpineSkeletonAnimationHandle animationHandler;
		[SerializeField] private string spookSkinName, normalSkinName;
		[SerializeField] private CachedAudioController audioController;
		[SerializeField] private StudioEventEmitter bringDownEmitter;
		[SerializeField] private FadeUtility fadeUtility;
		//TODO Camera direct reference should be switched out to an event based scriptable
		[SerializeField] private CameraPriorityUtility cameraPriorityUtility;
		[SerializeField] private CinemachineCamera closeUpCamera, mainCamera;

		[SerializeField] private float playSpeed = 1f;

		TrackEntry currentTrack;

		internal override void OnEnter(int state)
		{
			base.OnEnter(state);

            // Stop player moving
            rb2d.linearVelocity = Vector2.zero;

			fadeUtility.ReappearImage();

            bringDownEmitter.Play();
			cameraPriorityUtility.SwitchCameras(closeUpCamera);
            audioController.PlayOneShot();
            animationHandler.SetSkin(spookSkinName);
            currentTrack = animationHandler.PlayAnimationReference(spook1Anim, 0, false, false, playSpeed);
		}

		internal override void OnFixedUpdate()
		{
			base.OnFixedUpdate();
			if (IsInCurrentHandlerState() && currentTrack != null && currentTrack.IsComplete)
				SetState(nextState);
		}

		internal override void OnExit()
		{
			base.OnExit();
            cameraPriorityUtility.SwitchCameras(mainCamera);
            bringDownEmitter.Stop();
            currentTrack = null;
        }


	}
}