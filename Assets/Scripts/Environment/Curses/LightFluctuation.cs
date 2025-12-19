using Assets.Scripts.Audio;
using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Environment.Curses
{
    internal class LightFluctuation : CursedBase
    {
        [SerializeField] private Light2D[] lights;
        [SerializeField] private ActionCurseEnumScriptable startCurseScriptable, stopCurseScriptable;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private float fluctuationDuration;
        [SerializeField] private float bottomIntensity, ceilingIntensity;
        [SerializeField] private LeanTweenType easyType;


        List<LightStruct> lightStructs;
        LTDescr audioTween;

        private void Start()
        {
            lightStructs = new List<LightStruct>();
            startCurseScriptable.AddAction(HandleLightCurseStart);
            stopCurseScriptable.AddAction(HandleLightCurseStop);
        }

        private void HandleLightCurseStart(CurseTypeEnum curseType)
        {
            if(curseType == CurseTypeEnum.LightFluctuate)
            {
                lightStructs.Clear();

                audioController.PlayOneShot();

                // Tween audio
                audioTween = LeanTween.value(audioController.GetVolume(), 0, fluctuationDuration)
                .setOnUpdate((float value) => audioController.SetParam("Volume", value))
                .setLoopPingPong()
                .setEase(easyType);

                // Tween lights
                foreach (var light in lights)
                {
                    float lightOriginalIntensity = light.intensity;
                    
                    // Drop light to zero at first
                    LTDescr tween = LeanTween.value(lightOriginalIntensity, bottomIntensity, fluctuationDuration)
                        .setOnUpdate((float value) => light.intensity = value)
                        .setOnComplete(() =>
                        {
                            // AFter light is at 0, then ping pong between the ceiling intensity declared
                            LeanTween.value(bottomIntensity, ceilingIntensity, fluctuationDuration)
                            .setOnUpdate((float value)=> light.intensity = value)
                            .setLoopPingPong()
                            .setEase(easyType);
                        });
                        

                    var t = new LightStruct();
                    t.tween = tween;
                    t.originalIntensity = lightOriginalIntensity;
                    t.light = light;

                    lightStructs.Add(t);
                }
            }
        }


        private void HandleLightCurseStop(CurseTypeEnum curseType)
        {
            if (curseType == CurseTypeEnum.LightFluctuate)
            {
                if(audioTween != null)                
                    LeanTween.cancel(audioTween.id);
                
                // Tween audio
                LeanTween.value(audioController.GetVolume(), 0, fluctuationDuration / 2)
                    .setOnUpdate((float value) => {
                        audioController.SetParam("Volume", value);
                    }).setEase(easyType)
                    .setOnComplete(() => audioController.Stop(false));

                // Tween lights
                foreach (var lightStruct in lightStructs)
                {
                    LeanTween.cancel(lightStruct.tween.id);

                    LeanTween.value(lightStruct.light.intensity, lightStruct.originalIntensity, fluctuationDuration / 2)
                        .setOnUpdate((float value) => lightStruct.light.intensity = value)
                        .setEase(easyType);
                }
            }
        }

        private void OnDisable()
        {
            startCurseScriptable.RemoveAction(HandleLightCurseStart);
            stopCurseScriptable.RemoveAction(HandleLightCurseStop);
        }

        [ContextMenu("Trigger Enable fluctuation")]
        private void StartFlux() => HandleLightCurseStart(CurseTypeEnum.LightFluctuate);

        [ContextMenu("Trigger Disable fluctuation")]
        private void EndFlux() => HandleLightCurseStop(CurseTypeEnum.LightFluctuate);
    }
    

    struct LightStruct
    {
        public LTDescr tween;
        public float originalIntensity;
        public Light2D light;
    }
}
