using Assets.Scripts.Data.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    internal class ActionPlayParticleSystem : MonoBehaviour
    {
        [SerializeField] private ActionScriptable actionScript;
        [SerializeField] private ParticleSystem ps;


        private void Start() => actionScript.AddAction(HandleAction);

        private void HandleAction() => ps.Play();

        private void OnDestroy() => actionScript.RemoveAction(HandleAction);
    }
}
