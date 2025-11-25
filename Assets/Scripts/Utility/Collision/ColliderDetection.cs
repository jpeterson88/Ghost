using Assets.Scripts.Data.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Utility.Collision
{
    internal class ColliderDetection: MonoBehaviour
    {
        [SerializeField] private ActionScriptable enterCollision, exitCollision;
        [SerializeField] private bool enableDebugging;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(enableDebugging)
                Debug.Log($"Detected {collision.gameObject.name}");

            enterCollision?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (enableDebugging)
                Debug.Log($"Detected {collision.gameObject.name}");

            exitCollision?.Invoke();
        }
    }
}
