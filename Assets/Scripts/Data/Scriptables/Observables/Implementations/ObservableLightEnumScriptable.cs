using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Observables;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Observables/LightEnum")]
    internal class ObservableLightEnumScriptable: ObservableTScriptable<LightSourceEnum>
    {
    }
}
