using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Spine;
using Spine.Unity;
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
		TrackEntry currentTrack;

		internal override void OnEnter(int state)
		{
			base.OnEnter(state);		

            // Stop player moving
            rb2d.linearVelocity = Vector2.zero;

            SetSkin(spookSkinName);
            currentTrack = animationHandler.PlayAnimationReference(spook1Anim, 0, false, false);
		}

		private void SetSkin(string skinname)
		{
            animationHandler.skeletonAnimation.skeleton.SetSkin(skinname);
            animationHandler.skeletonAnimation.skeleton.SetSlotsToSetupPose();
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
            currentTrack = null;
            SetSkin(normalSkinName);
        }
	}
}