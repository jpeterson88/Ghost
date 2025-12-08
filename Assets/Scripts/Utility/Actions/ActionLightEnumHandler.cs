using Assets.Scripts.Audio;
using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Events.Implementations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Utility.Actions
{
    internal class ActionLightEnumHandler: MonoBehaviour
    {
        [SerializeField] private ActionLightEnumScriptable lightSwitchAction;
        [SerializeField] private Light2D[] lightsOnWtihEnum, lightsOffwithEnum;
        [SerializeField] private LightSourceEnum sourceLightEnum;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private bool sourceOnByDefault;

        private bool isSourceOn;

        private void Start()
        {
            lightSwitchAction.AddAction(ToggleLightSwitch);
            isSourceOn = sourceOnByDefault;
        }

        private void HandleLightChange(bool isOff, LightSourceEnum lightSource) 
        {
            if (sourceLightEnum == lightSource)
            {
                audioController?.PlayOneShot();
                SwitchLights(isOff);
            }
        }

        private void SwitchLights(bool isOff)
        {
            // Turn off lights that should be on
            foreach (var lightOn in lightsOnWtihEnum)
                lightOn.enabled = isOff ? false : true;

            // Turn on lights that should be off
            foreach (var lightOff in lightsOffwithEnum)
                lightOff.enabled = isOff ? true : false;
        }

        private void OnDisable() => lightSwitchAction.RemoveAction(ToggleLightSwitch);

        [ContextMenu("Trigger TestToggleLightSwitchOn")]
        private void ToggleSwitchFromEditor() => ToggleLightSwitch(sourceLightEnum);
        private void ToggleLightSwitch(LightSourceEnum lightSource)
        {
            isSourceOn = !isSourceOn;
            HandleLightChange(isSourceOn ? false : true, sourceLightEnum);
        }
    }
}
