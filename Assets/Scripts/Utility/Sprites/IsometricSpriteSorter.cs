using System;
using UnityEngine;

namespace Assets.Scripts.Utility.Sprites
{
    [ExecuteInEditMode]
    internal class IsometricSpriteSorter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private const int IsometricRangePerYUnit = 100;

        private void Awake()
        {
            if (spriteRenderer == null)
                spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null)
                throw new NullReferenceException(nameof(spriteRenderer));

            spriteRenderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
        }

        private void Update()
        {
            spriteRenderer.sortingOrder = -(int)(transform.position.y * IsometricRangePerYUnit);
        }
    }
}
