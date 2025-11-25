using Assets.Scripts.Audio;
using Assets.Scripts.Data.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    internal class ActionAudioQueue: MonoBehaviour
    {
        [SerializeField] private ActionScriptable actionScript;
        [SerializeField] private CachedAudioController audioController;
        [SerializeField] private float playDelay;

        private void Start() => actionScript.AddAction(HandleAction);

        private void HandleAction() => LeanTween.value(0, 1, playDelay).setOnComplete(() => { audioController.PlayOneShot(); });

        private void OnDestroy() => actionScript.RemoveAction(HandleAction);
    }
}
