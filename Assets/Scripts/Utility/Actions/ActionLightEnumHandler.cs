using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Utility.Actions
{
    internal class ActionLightEnumHandler: MonoBehaviour
    {
        [SerializeField] private ActionBoolLightEnumScriptable actionBoolLightEnumScriptable;
        [SerializeField] private Light2D[] lightsOnWtihEnum, lightsOffwithEnum;
        [SerializeField] private LightSourceEnum sourceLightEnum;
        [SerializeField] private bool sourceOnByDefault;

        private bool isSourceOn;

        private void Start()
        {
            actionBoolLightEnumScriptable.AddAction(HandleLightChange);
            isSourceOn = sourceOnByDefault;
        }

        private void HandleLightChange(bool isOff, LightSourceEnum lightSource) 
        {
            if (sourceLightEnum == lightSource)
                SwitchLights(isOff);
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

        private void OnDisable() => actionBoolLightEnumScriptable.RemoveAction(HandleLightChange);


        [ContextMenu("Trigger TestToggleLightSwitchOn")]
        private void TestToggleLightSwitchOn()
        {
            isSourceOn = !isSourceOn;
            HandleLightChange(isSourceOn ? false : true, sourceLightEnum);
            
        }
    }
}
