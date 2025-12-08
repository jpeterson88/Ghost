using System;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptables
{
    public class ActionT1T2Scriptable<T1, T2> : ScriptableObject
    {
        #region Public Variables

        public event Action<T1, T2> Value = delegate { };

        #endregion Public Variables

        public void AddAction(Action<T1, T2> callback)
        {
            Value += callback;
        }

        public void RemoveAction(Action<T1, T2> callback)
        {
            Value -= callback;
        }

        public void Clear()
        {
            Value = delegate { };
        }

        public void Invoke(T1 value1, T2 value2)
        {
            Value?.Invoke(value1, value2);
        }
    }
}