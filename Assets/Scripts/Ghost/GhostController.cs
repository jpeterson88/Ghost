using Assets.Scripts.Audio;
using Assets.Scripts.Data.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    internal class GhostController: MonoBehaviour
    {
        [SerializeField] private ActionTScriptable<bool> onFadeAction;
        [SerializeField] private ActionScriptable onSeenActionHandler;
        [SerializeField] private CachedAudioController fadeInAudio, fadeOutAudio;
        [SerializeField] private FadeUtility fadeUtility;
        private void Awake()
        {
            onFadeAction.AddChangeAction(PlayFade);
            onSeenActionHandler.AddAction(HandleOnSeen);
        }



        private void PlayFade(bool isFadeIn)
        {
            //if (isFadeIn)
            //    fadeInAudio?.PlayOneShot();
            //else 
            //    fadeOutAudio?.PlayOneShot();
        }

        private void HandleOnSeen() 
        {
            fadeUtility.ReappearImage();
        }
        private void OnDestroy() => onFadeAction.RemoveChangeAction(PlayFade);
    }
}
