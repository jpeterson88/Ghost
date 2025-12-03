using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Utility.Actions
{
    internal class ActionLightEnumHandler: MonoBehaviour
    {
        [SerializeField] private ActionBoolLightEnumScriptable actionBoolLightEnumScriptable;
        [SerializeField] private Light2D light2d;
        [SerializeField] private LightSourceEnum sourceOfLight, offLight;
        [SerializeField] private float onLightIntensity;

        private void Start() => actionBoolLightEnumScriptable.AddAction(HandleLightChange);

        private void HandleLightChange(bool isOff, LightSourceEnum lightSource) 
        {
            if (sourceOfLight == lightSource && isOff)
                light2d.intensity = 0f;
            else
                light2d.intensity = onLightIntensity;
        }

        private void OnDisable() => actionBoolLightEnumScriptable.RemoveAction(HandleLightChange);
    }
}
