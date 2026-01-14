using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using Assets.Scripts.Environment.Curses;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    internal class CastCurseController: MonoBehaviour
    {
        [SerializeField] private float cooldownDuration = 1f;
        [SerializeField] private CircularPathSpawner curseSpawner;
        [SerializeField] private ActionCurseEnumScriptable actionCurseEnumScriptable;
        [SerializeField] private float delayToNotifyCurseSuccess = 1f;
        [SerializeField] private bool isDebugOn;

        private bool isOnCooldown;
        private float currentCooldownTime;
        private CurseTypeEnum targetCurseType;
        private Transform targetTransform;

        public void StartCooldown() => isOnCooldown = true;

        public void StartCurse() => curseSpawner.SpawnAndMoveObjects(targetTransform);

        public bool CanCurse() => targetCurseType != CurseTypeEnum.None && !isOnCooldown;

        public void EndCurse() => curseSpawner.CancelMovement();

        public Transform GetTargetTransform() => targetTransform;

        private void Update()
        {
            if (isOnCooldown)
            {
                currentCooldownTime += Time.deltaTime;

                if(currentCooldownTime >= cooldownDuration)
                {
                    isOnCooldown = false;
                    currentCooldownTime = 0f;
                }
            }
        }

        private bool IsOnCooldown() => isOnCooldown;

        

        public void CastSuccessfulCurse() => StartCoroutine(DelayedCurseNotification());

        private System.Collections.IEnumerator DelayedCurseNotification()
        {
            yield return new WaitForSeconds(delayToNotifyCurseSuccess);
            if(isDebugOn)
                Debug.Log(($"Trigger successful curse: ${targetCurseType}"));

            actionCurseEnumScriptable.Invoke(targetCurseType);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {           
            // Parent must have ICursedObject
            targetCurseType = collision.gameObject.GetComponentInParent<CursedBase>().objectCurseType;
            targetTransform = collision.transform;

            if (isDebugOn)
                Debug.Log(($"Set targetCurseType to: ${targetCurseType}"));
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            targetCurseType = CurseTypeEnum.None;
            targetTransform = null;
        }
    }
}
