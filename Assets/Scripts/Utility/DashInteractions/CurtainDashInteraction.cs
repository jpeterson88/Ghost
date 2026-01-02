using Spine;
using UnityEngine;

namespace Assets.Scripts.Utility.DashInteractions
{
    internal class CurtainDashInteraction : MonoBehaviour, IHandleDashInteraction
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string whooshLeft, whooshRight, whooshOutwards;

        [ContextMenu("Trigger Enable fluctuation")]
        public void TriggerDashInteraction() => HandleDashInteraction(Vector2.left);
        public void HandleDashInteraction(Vector2 dashDirection)
        {
            if(dashDirection.x < 0)
                animator.SetTrigger(whooshLeft);
            else if(dashDirection.x > 0)
                animator.SetTrigger(whooshRight);
            
        }
    }
}
