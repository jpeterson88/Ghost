using UnityEngine;

namespace Assets.Scripts.Utility.DashInteractions
{
    internal class DashDetection: MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private Collider2D collider2D;
        public void EnableDetector()
        {
            collider2D.enabled = true;
        }

        public void DisableDetector()
        {
            collider2D.enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var dashInteraction = collision.GetComponent<IHandleDashInteraction>();

            if (dashInteraction != null)
                dashInteraction.HandleDashInteraction(rb2d.linearVelocity);
        }
    }
}
