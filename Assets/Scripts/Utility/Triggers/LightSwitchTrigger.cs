//namespace DentedPixel{

using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Events.Implementations;
using UnityEngine;

namespace Assets.Scripts.Utility.Triggers
{
    internal class LightSwitchTrigger: MonoBehaviour
    {

        [SerializeField] private ActionLightEnumScriptable triggerLight;
        [SerializeField] private LightSourceEnum lightSourceEnum;
        [SerializeField] private bool debugOn;

        private bool isInTrigger;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            isInTrigger = true;
            if(debugOn)
                Debug.Log("Ghost enter");
        }

        //private void Update()
        //{
        //    if (isInTrigger && Input.GetKeyDown(KeyCode.E))
        //        triggerLight?.Invoke(lightSourceEnum);
        //}

        private void OnTriggerExit2D(Collider2D collision)
        {
            isInTrigger = false;
            if (debugOn)
                Debug.Log("Ghost exit");
        }
    }
}
