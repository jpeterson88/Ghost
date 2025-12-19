using Assets.Scripts.Audio;
using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Assets.Scripts.Environment.Curses
{
    internal class RadioCursed: CursedBase
    {
        [SerializeField] private ActionCurseEnumScriptable startCurseScriptable, stopCurseScriptable;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private Light2D[] lights;


        private void Start()
        {
            startCurseScriptable.AddAction(HandleCurseStart);
            stopCurseScriptable.AddAction(HandleCurseStop);
        }

        private void HandleCurseStart(CurseTypeEnum targetedCurse)
        {
            if (targetedCurse == CurseTypeEnum.Radio)
            {
                audioController.PlayOneShot();
                foreach (var light in lights)
                    light.enabled = true;
            }
        }

        private void HandleCurseStop(CurseTypeEnum targetedCurse)
        {
            if (targetedCurse == CurseTypeEnum.Radio)
            {
                audioController.Stop();
                foreach (var light in lights)
                    light.enabled = false;
            }
        }

        [ContextMenu("Trigger Enable Radio")]
        private void StartFlux() => HandleCurseStart(CurseTypeEnum.Radio);

        [ContextMenu("Trigger Disable Radio")]
        private void EndFlux() => HandleCurseStop(CurseTypeEnum.Radio);
    }
}
