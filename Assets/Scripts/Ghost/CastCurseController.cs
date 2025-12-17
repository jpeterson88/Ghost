using Assets.Scripts.Data.Enums;
using Assets.Scripts.Data.Scriptables.Actions.Implementations;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    internal class CastCurseController: MonoBehaviour
    {
        [SerializeField] private float cooldownDuration = 1f;
        [SerializeField] private CircularPathSpawner curseSpawner;
        [SerializeField] private ActionCurseEnumScriptable actionCurseEnumScriptable;
        [SerializeField] private float delayToNotifyCurseSuccess = 1f;

        private bool isOnCooldown;
        private float currentCooldownTime;
        public void StartCooldown()
        {
            isOnCooldown = true;
        }

        public void StartCurse()
        {
            curseSpawner.SpawnAndMoveObjects();
        }

        public void EndCurse()
        {
            curseSpawner.CancelMovement();
        }

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

        public bool IsOnCooldown() => isOnCooldown;

        public void CastSuccessfulCurse() => StartCoroutine(DelayedCurseNotification());

        private System.Collections.IEnumerator DelayedCurseNotification()
        {
            yield return new WaitForSeconds(delayToNotifyCurseSuccess);

            actionCurseEnumScriptable.Invoke(CurseTypeEnum.LightFluctuate);
        }
    }
}
