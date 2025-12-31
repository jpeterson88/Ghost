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

        private bool isInTrigger;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            isInTrigger = true;
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
            Debug.Log("Ghost exit");
        }
    }
}
