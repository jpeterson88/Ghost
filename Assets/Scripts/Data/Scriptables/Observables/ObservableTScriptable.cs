using System;
using UnityEngine;

namespace Assets.Scripts.Data.Scriptables.Observables
{    
    public class ObservableTScriptable<T> : ScriptableObject
    {

        #region Public Variables

        public T Value;
        public bool Locked;
        public bool InvokeOnValidate;
        public Action<T> OnValueChanged = delegate { };

        #endregion Public Variables

        // editor call when GUI is changed
        private void OnValidate()
        {
            if (InvokeOnValidate)
                InvokeChanged();
        }

        /// <summary>
        /// Set value, trigger will invoke the OnValueChaned event
        /// </summary>
        public virtual void Set(T value, bool trigger = true)
        {
            if (Locked)
                return;

            Value = value;

            if (trigger)
                InvokeChanged();
        }

        /// <summary>
        /// Sets value and triggers changed event
        /// </summary>
        public virtual void Set(T value)
        {
            if (Locked)
                return;

            Value = value;
            InvokeChanged();
        }

        public virtual void Lock(bool value)
        {
            Locked = value;
        }

        public virtual T Get()
        {
            return Value;
        }

        /// <summary>
        /// Add single action to the change event
        /// </summary>
        public virtual void AddChangeAction(Action<T> action)
        {
            OnValueChanged += action;
        }

        /// <summary>
        /// Remove single action from the change event
        /// If an action is added, it should be removed, usually OnDisable/OnDestroy
        /// </summary>
        public virtual void RemoveChangeAction(Action<T> action)
        {
            OnValueChanged -= action;
        }

        /// <summary>
        /// Remove all actions from the events, use with caution
        /// </summary>
        public virtual void ClearChangeAction()
        {
            OnValueChanged = delegate { };
        }

        /// <summary>
        /// Invoke Unity Event
        /// </summary>
        public virtual void InvokeChanged()
        {
            OnValueChanged?.Invoke(Value);
        }
    }
}