using System;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

namespace Assets.Scripts.Input
{
    public class PlayerInputMap : MonoBehaviour, IInput
    {
        Vector2 currentMoveVector;
        public Action CursePressed { get; set; }
        public Action CurseReleased { get; set; }
        public Action DashPressed { get; set; }

        // Actions are from Unity PlayerInput script action
        public void SetMoveVector(CallbackContext callbackContext) => currentMoveVector = callbackContext.ReadValue<Vector2>();

        public Vector2 GetMoveVector() => currentMoveVector;

        public void OnCursePressed(CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                CursePressed?.Invoke();
            else if(callbackContext.canceled)
                CurseReleased?.Invoke();
        }

        public void OnDashPressed(CallbackContext callbackContext)
        {
            if (callbackContext.performed)
                DashPressed?.Invoke();
        }
    }
}