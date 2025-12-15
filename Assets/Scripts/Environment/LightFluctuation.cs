using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Environment
{
    internal class LightFluctuation : MonoBehaviour
    {
        [SerializeField] private Light2D[] lights;
        [SerializeField] private ActionCurseEnumScriptable startCurseScriptable, stopCurseScriptable;
        [SerializeField] private float fluctuationDuration;
        [SerializeField] private float bottomIntensity, ceilingIntensity;
        [SerializeField] private LeanTweenType easyType;
        List<LTDescr> tweens;
        List<LightStruct> lightStructs;

        private void Start()
        {
            tweens = new List<LTDescr>();
            lightStructs = new List<LightStruct>();
            startCurseScriptable.AddAction(HandleLightCurseStart);
            stopCurseScriptable.AddAction(HandleLightCurseStop);
        }

        private void HandleLightCurseStart(CurseTypeEnum curseType)
        {
            if(curseType == CurseTypeEnum.LightFluctuate)
            {
                lightStructs.Clear();

                foreach (var light in lights)
                {
                    float lightOriginalIntensity = light.intensity;
                    LTDescr tween = LeanTween.value(lightOriginalIntensity, bottomIntensity, fluctuationDuration)
                        .setOnUpdate((float value) => light.intensity = value)
                        .setOnComplete(() =>
                        {

                            LeanTween.value(bottomIntensity, ceilingIntensity, fluctuationDuration)
                        .setOnUpdate((float value) => light.intensity = value)
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
                foreach (var lightStruct in lightStructs)
                {
                    LeanTween.cancel(lightStruct.tween.id);

                    LeanTween.value(lightStruct.light.intensity, lightStruct.originalIntensity, fluctuationDuration / 2)
                        .setOnUpdate((float value) => lightStruct.light.intensity = value)
                        .setEaseInSine();
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
