using UnityEngine;

namespace Assets.Scripts.Utility.RayCasts
{
    internal class BoxCaster : MonoBehaviour
    {
        [SerializeField] private FacingDirection directionController;
        [SerializeField] private LayerMask castLayer;

        [Header("Box Cast Settings")]
        [SerializeField] private Vector2 boxSize = new Vector2(1f, 1f);
        [SerializeField] private float boxCastDistance = 1.5f;

        private Vector2 directionalDashVector = Vector2.right;

        public RaycastHit2D Cast()
        {
            FacingDirectionEnum currentDirection = directionController.GetCurrentFacing();
            directionalDashVector = currentDirection == FacingDirectionEnum.Left ? Vector2.left : Vector2.right;

            RaycastHit2D hit = Physics2D.BoxCast(
                transform.position,
                boxSize,
                0f, // No rotation for the box
                directionalDashVector.normalized,
                boxCastDistance,
                castLayer
            );

            return hit;
        }

        private void OnDrawGizmos()
        {
            var hit = Cast();
            Vector2 origin = transform.position;

            Vector2 direction = directionalDashVector.normalized;

            Gizmos.color = hit.collider != null ? Color.red : Color.green;

            // Draw the BoxCast as a rectangle with a line indicating the cast direction
            Gizmos.matrix = Matrix4x4.TRS(origin, Quaternion.identity, Vector3.one);
            Gizmos.matrix = Matrix4x4.identity;

            Gizmos.DrawLine(origin, origin + direction * boxCastDistance); // Line
            Gizmos.matrix = Matrix4x4.TRS(origin + direction * boxCastDistance, Quaternion.identity, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, boxSize); // Ending box
            Gizmos.matrix = Matrix4x4.identity;
        }
    }
}