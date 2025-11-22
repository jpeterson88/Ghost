using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    internal class UncachedAudioController : MonoBehaviour
    {
        [SerializeField] private float fadeoutTime = 1f;
        [SerializeField] private EventReference eventReference;
        private EventInstance instance;

        public void Play(float parameterValue = 0)
        {
            instance = RuntimeManager.CreateInstance(eventReference);

            instance.start();
            instance.release();

            instance.setParameterByName("pitch", parameterValue);
        }

        public void Stop(bool withFade)
        {
            FMOD.Studio.STOP_MODE stopMode = withFade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE;

            instance.stop(stopMode);
        }

        public bool IsPlaying()
        {
            FMOD.Studio.PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
        }
    }
}