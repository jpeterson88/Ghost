using Assets.Scripts.Audio;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Utility.DashInteractions
{
    internal class TransformWobbleDashInteraction : MonoBehaviour, IHandleDashInteraction
    {
        [SerializeField] private Transform targetTransform;
        [SerializeField] private CachedAudioController audioController;

        [SerializeField] private float tiltDuration = 1f;
        [SerializeField] private float tiltAngle = 15f;
        [SerializeField] private int tiltFrequency = 4;

        public void HandleDashInteraction(Vector2 dashDirection)
        {
            if (targetTransform == null)
            {
                Debug.LogWarning("Target Transform is not assigned for TransformWobbleDashInteraction.");
                return;
            }
            audioController?.PlayOneShot();
            StartCoroutine(TiltCoroutine());
        }

        private IEnumerator TiltCoroutine()
        {
            Quaternion originalRotation = targetTransform.localRotation;
            float elapsedTime = 0f;

            while (elapsedTime < tiltDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = elapsedTime / tiltDuration;
                float angle = Mathf.Sin(progress * Mathf.PI * tiltFrequency) * Mathf.Lerp(tiltAngle, 0f, progress);
                targetTransform.localRotation = originalRotation * Quaternion.Euler(0f, 0f, angle);

                yield return null;
            }

            targetTransform.localRotation = originalRotation;
        }
    }
}