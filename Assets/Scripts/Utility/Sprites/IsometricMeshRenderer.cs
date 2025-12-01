using System;
using UnityEngine;

namespace Assets.Scripts.Utility.Sprites
{
    [ExecuteInEditMode]
    public class IsometricMeshSorter : MonoBehaviour
    {
        public int IsometricRangePerYUnit = 100;

        [SerializeField] private MeshRenderer meshRenderer;

        private bool canUpdate = true;

        public void SetCanUpdate(bool canUpdate) => this.canUpdate = canUpdate;

        private void Awake()
        {
            if (meshRenderer == null)
                meshRenderer = GetComponent<MeshRenderer>();

            if (meshRenderer == null)
                throw new NullReferenceException(nameof(meshRenderer));
        }

        private void Update()
        {
            if (canUpdate)
            {
                meshRenderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
            }
        }

        public void ManuallySetOrder(int orderValue) => meshRenderer.sortingOrder = orderValue;
    }
}
