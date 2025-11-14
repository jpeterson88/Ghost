using Assets.Scripts.StateMachine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public class StateMachine<T> : MonoBehaviour where T : Enum
    {
        [SerializeField]
        [Tooltip("States that are allowed to repeat.")]
        private T[] allowedRepeatStates;

        [SerializeField] private bool setStateOnFirstFrameUpdate;
        [SerializeField] private T startingState;

        [SerializeField] private bool enableLogs;

        private int currentState;

        private bool stateSet;

        /// <summary>
        /// An event raised for when a state changes.
        /// <para>Current State</para>
        /// <para>New State</para>
        /// </summary>
        public Action<int, int> OnStateChanged = delegate { };

        private StateHandlerBase currentStateHandler;
        private StateHandlerBase[] stateHandlers;
        private List<int> convertedStates;

        private void Awake()
        {
            stateHandlers = GetComponentsInChildren<StateHandlerBase>();

            if (stateHandlers == null || !stateHandlers.Any())
                Debug.LogError("Failed to get statehandlers.");

            convertedStates = new List<int>();

            foreach (T i in allowedRepeatStates)
                convertedStates.Add(Convert.ToInt32(i));
        }

        private void FixedUpdate()
        {
            if (!stateSet && setStateOnFirstFrameUpdate)
            {
                stateSet = true;
                SetState(Convert.ToInt32(startingState));
            }

            if (currentStateHandler != null)
                currentStateHandler.OnFixedUpdate();
        }

        private void Update()
        {
            if (currentStateHandler != null)
                currentStateHandler.OnUpdate();
        }

        public int GetCurrentState() => currentState;

        public void SetState(int newState)
        {
            //If state is the same state and is not in repeatStates, return.
            if (currentStateHandler != null && newState == currentState && !convertedStates.Any(x => x == newState))
            {
                return;
            }

            StateHandlerBase newStateHandler = stateHandlers.FirstOrDefault(x => x.CanHandle(newState));

            if (newStateHandler != null)
            {
                if (enableLogs)
                    Debug.Log($"Current: {(PlayerStates)currentState} ------ New: {(PlayerStates)newState}. ");

                OnStateChanged?.Invoke(currentState, newState);
                currentState = newState;

                //Should only be null on initialization
                if (currentStateHandler != null)
                    currentStateHandler.OnExit();

                currentStateHandler = newStateHandler;
                currentStateHandler.OnEnter(currentState);
            }
        }
    }
}