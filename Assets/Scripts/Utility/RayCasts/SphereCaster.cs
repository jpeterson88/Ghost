using UnityEngine;

namespace Assets.Scripts.Utility.RayCasts
{


    internal class SphereCaster: MonoBehaviour
    {
        [SerializeField] private FacingDirection directionController;
        [SerializeField] private LayerMask castLayer;

        
        [SerializeField] private float dashSphereRadius = 0.5f;
        [SerializeField] private float dashSphereDistance = 1.5f;

        private Vector2 directionalDashVector = Vector2.right;

        public RaycastHit2D Cast()
        {
            FacingDirectionEnum currentDirection = directionController.GetCurrentFacing();
            directionalDashVector = currentDirection == FacingDirectionEnum.Left ? Vector2.left : Vector2.right;

            RaycastHit2D hit = Physics2D.CircleCast(
                transform.position,
                dashSphereRadius,
                directionalDashVector.normalized,
                dashSphereDistance, castLayer);

            return hit;
        }

        private void OnDrawGizmos()
        {
            Vector2 origin = transform.position;
            Vector2 direction = directionalDashVector.normalized;

            RaycastHit2D hit = Physics2D.CircleCast(
                origin,
                dashSphereRadius,
                direction,
                dashSphereDistance,
                castLayer
            );

            // Set Gizmo color based on whether a hit was detected
            Gizmos.color = hit.collider != null ? Color.red : Color.green;

            // Draw the CircleCast as a line with a sphere at the end
            Gizmos.DrawLine(origin, origin + direction * dashSphereDistance); // Line
            Gizmos.DrawWireSphere(origin + direction * dashSphereDistance, dashSphereRadius); // End point
        }
    }
}
