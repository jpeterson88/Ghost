using Assets.Scripts.Audio;
using UnityEngine;

namespace Assets.Scripts.Utility.DashInteractions
{
    internal class DashDetection: MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb2d;
        [SerializeField] private Collider2D dashCollider;
        [SerializeField] private string collideTagName;
        private bool isColliding;
        public void EnableDetector()
        {
            dashCollider.enabled = true;
        }

        public void DisableDetector()
        {
            dashCollider.enabled = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var dashInteraction = collision.GetComponent<IHandleDashInteraction>();

            if (dashInteraction != null)
            {
                Debug.Log("IHandleDashInteraction");
                dashInteraction.HandleDashInteraction(rb2d.linearVelocity);

                if(collision.CompareTag(collideTagName))
                    isColliding = true;
            }
            else
            {
                
                Debug.Log($"IHandleDashInteraction was null on Detect. Name {collision.transform.name}");
            }
        }
        private void OnTriggerExit2D(Collider2D collision) => isColliding = false;

        public bool IsColliding() => isColliding;
    }
}
