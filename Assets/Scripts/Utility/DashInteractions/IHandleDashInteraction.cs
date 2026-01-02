using UnityEngine;

namespace Assets.Scripts.Utility.DashInteractions
{
    internal interface IHandleDashInteraction
    {
        void HandleDashInteraction(Vector2 dashDirection);
    }
}
