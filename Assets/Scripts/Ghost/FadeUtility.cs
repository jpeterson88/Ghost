using Assets.Scripts.Data.Scriptables.Observables;
using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    internal class FadeUtility : MonoBehaviour
    {

        [SerializeField] private SkeletonAnimation skeletonAnim;
        [SerializeField] private float fadeInDuration, fadeOutDuration, timeAllowedTillNextFade;
        [SerializeField] private PlayerStateMachine stateMachine;
        [SerializeField] private PlayerStates[] fadeableStates;        
        [SerializeField] private ObservableTScriptable<bool> fadeAction;
        [SerializeField] private bool startFaded = true;
        [SerializeField] private float fadedAlpha = 0.5f;

        private List<int> fadeableStateInts;

        private bool cooldownIsUp = true, isInFadedState = false, isInMiddleOfFade = false;
        private float cooldownTime;
        

        private void Start()
        {
            fadeableStateInts = new List<int>();

            foreach (var state in fadeableStates)
                fadeableStateInts.Add((int)state);

            // Set starting alpha
            isInFadedState = startFaded;
            skeletonAnim.skeleton.A = startFaded ? fadedAlpha : 1f;
        }

        private void Update()
        {
            if (!cooldownIsUp)
            {
                cooldownTime -= Time.deltaTime;

                cooldownIsUp = cooldownTime <= 0;
            }
            // If cooldown is up, try to fade
            else if (fadeableStateInts.Contains(stateMachine.GetCurrentState()) && !isInFadedState && !isInMiddleOfFade)
            {
                FadeImage();
            }
        }

        public void ReappearImage()
        {
            if(isInFadedState && !isInMiddleOfFade)
            {
                cooldownTime = timeAllowedTillNextFade;
                cooldownIsUp = false;

                isInFadedState = false;
                isInMiddleOfFade = true;
                fadeAction.Set(true);

                LeanTween.value(gameObject, skeletonAnim.skeleton.A, 1, fadeInDuration).setOnUpdate((float val) =>
                {
                    skeletonAnim.skeleton.A = val;
                }).setOnComplete(() => { isInMiddleOfFade = false; });
            }
            // Reset fade cooldown

        }

        private void FadeImage()
        {
            isInFadedState = true;
            isInMiddleOfFade = true;
            fadeAction.Set(false);

            LeanTween.value(gameObject, skeletonAnim.skeleton.A, fadedAlpha, fadeInDuration).setOnUpdate((float val) =>
            {
                skeletonAnim.skeleton.A = val;
            }).setOnComplete(() => { isInMiddleOfFade = false; });
        }
    }
}
