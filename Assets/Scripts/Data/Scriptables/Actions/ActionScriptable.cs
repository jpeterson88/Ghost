using System;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptables
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Actions/ActionScriptable")]
    internal class ActionScriptable : ScriptableObject
    {
        public event Action Value = delegate { };

        public void AddAction(Action callback)
        {
            Value += callback;
        }

        public void RemoveAction(Action callback)
        {
            Value -= callback;
        }

        public void Clear()
        {
            Value = delegate { };
        }

        public void Invoke()
        {
            Value?.Invoke();
        }
    }
}