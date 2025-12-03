using Assets.Scripts.Data.Enums;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Actions/ActionBoolLightEnumScriptable")]
    internal class ActionBoolLightEnumScriptable: ActionT1T2Scriptable<bool, LightSourceEnum>
    {
    }
}
