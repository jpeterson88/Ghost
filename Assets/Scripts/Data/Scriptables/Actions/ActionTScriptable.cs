using System;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptables.Events
{
    public class ActionTScriptable<T1> : ScriptableObject
    {
        #region Public Variables

        public event Action<T1> Value = delegate { };

        #endregion Public Variables

        public void AddAction(Action<T1> callback)
        {
            Value += callback;
        }

        public void RemoveAction(Action<T1> callback)
        {
            Value -= callback;
        }

        public void Clear()
        {
            Value = delegate { };
        }

        public void Invoke(T1 value1)
        {
            Value?.Invoke(value1);
        }
    }
}
