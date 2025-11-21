using Assets.Scripts.Utility;
using Spine;
using Spine.Unity;
using UnityEngine;

public class SpineSkeletonAnimationHandle : MonoBehaviour
{
    [SerializeField]
    public SkeletonAnimation skeletonAnimation;

    public Spine.Animation TargetAnimation { get; private set; }

    public void PauseAnimation() => skeletonAnimation.timeScale = 0f;

    public void UnpauseAnimation() => skeletonAnimation.timeScale = 1f;

    public TrackEntry PlayAnimationReference(AnimationReferenceAsset animationReference, int layerIndex, bool isQueue, bool loopNewAnimation, float trackSpeed = 1.0f)
    {
        TrackEntry trackEntry;

        if (isQueue)
            trackEntry = skeletonAnimation.AnimationState.AddAnimation(layerIndex, animationReference, loopNewAnimation, 0);
        else
            trackEntry = skeletonAnimation.AnimationState.SetAnimation(layerIndex, animationReference, loopNewAnimation);

        trackEntry.TimeScale = trackSpeed;
        return trackEntry;
    }

    public TrackEntry AddEmptyAnimation(int track, float mixDuration, float delay = 0f)
    {
        return skeletonAnimation.AnimationState.AddEmptyAnimation(track, mixDuration, delay);
    }

    public void ClearTrack(int trackIndex)
    {
        skeletonAnimation.AnimationState.SetEmptyAnimation(trackIndex, .2f);
    }

    public void ToSetupPose() => skeletonAnimation.skeleton.SetToSetupPose();

    public Spine.Animation GetCurrentAnimation(int layerIndex)
    {
        var currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
        return (currentTrackEntry != null) ? currentTrackEntry.Animation : null;
    }

    public TrackEntry GetCurrentTrack(int layerIndex)
    {
        var currentTrackEntry = skeletonAnimation.AnimationState.GetCurrent(layerIndex);
        return (currentTrackEntry != null) ? currentTrackEntry : null;
    }

    public bool CompareTrackName(int layerIndex, string nameToCompare)
    {
        var trackEntry = GetCurrentTrack(layerIndex);
        
        return trackEntry != null && trackEntry.ToString().ToLower().Equals(nameToCompare.ToLower());

    }

    public void SetFacingDirection(FacingDirectionEnum facingDirection)
    {
        if(facingDirection == FacingDirectionEnum.Left && skeletonAnimation.Skeleton.ScaleX == 1)
        {
            skeletonAnimation.Skeleton.ScaleX = -1;
        }
        else if (facingDirection == FacingDirectionEnum.Right && skeletonAnimation.Skeleton.ScaleX != 1)
        {
            skeletonAnimation.Skeleton.ScaleX = 1;
        }
            
    }
}