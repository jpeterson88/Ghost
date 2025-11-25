using Assets.Scripts.Data.Scriptables;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    internal class ActionDisplaySpriteFade : MonoBehaviour
    {
        [SerializeField] private ActionScriptable actionScript;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private float fadeDuration;
        

        private void Start() => actionScript.AddAction(HandleAction);

        private void HandleAction()
        {
            spriteRenderer.enabled = true;

            LeanTween.value(gameObject, spriteRenderer.color.a, 0, fadeDuration)
                .setOnUpdate((float value) =>
                {
                    spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, value);
                })
                .setOnComplete(() => 
                {
                    //Reset spriteRenderer
                    spriteRenderer.enabled = false;
                    spriteRenderer.color = spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1);
                });

            
        }

        private void OnDestroy() => actionScript.RemoveAction(HandleAction);
    }
}
