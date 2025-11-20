using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Assets.Scripts.Audio
{
    internal class CachedAudioController
        : MonoBehaviour
    {
        [SerializeField] private EventReference eventReference;
        [SerializeField] private bool allowOverridePlay = true;
        [SerializeField] private bool playOnAwake;

        private EventInstance instance;
        private float currentParamValue;

        private void Awake()
        {
            instance = RuntimeManager.CreateInstance(eventReference);

            if (playOnAwake)
                PlayOneShot();
        }

        public EventInstance GetInstance() => instance;

        public void OnDisable() => instance.release();

        [ContextMenu("Play")]
        public void PlayOneShot()
        {
            if (allowOverridePlay || (!allowOverridePlay && !IsPlaying()))
                instance.start();
        }

        public void Stop(bool withFade)
        {
            FMOD.Studio.STOP_MODE stopMode = withFade ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE;
            instance.stop(stopMode);
        }

        public bool IsPlaying()
        {
            PLAYBACK_STATE state;
            instance.getPlaybackState(out state);
            return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
        }

        public void SetParam(string name, float value)
        {
            currentParamValue = value;
            instance.setParameterByName(name, value);
        }

        public float GetParamValue() => currentParamValue;
    }
}