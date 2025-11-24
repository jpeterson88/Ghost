using Assets.Scripts.StateMachine;
using Assets.Scripts.StateMachine.Enums;
using NUnit.Framework;
using Spine;
using Spine.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Ghost
{
    internal class FadeUtility : MonoBehaviour
    {

        [SerializeField] private SkeletonAnimation skeletonAnim;
        [SerializeField] private float fadeInDuration, fadeOutDuration, timeAllowedTillNextFade;
        [SerializeField] private PlayerStateMachine stateMachine;
        [SerializeField] private PlayerStates[] fadeableStates;
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
            // Reset fade cooldown
            cooldownTime = timeAllowedTillNextFade;
            cooldownIsUp = false;

            isInFadedState = false;
            isInMiddleOfFade = true;

            LeanTween.value(gameObject, skeletonAnim.skeleton.A, 1, fadeInDuration).setOnUpdate((float val) =>
            {
                skeletonAnim.skeleton.A = val;
            }).setOnComplete(() => { isInMiddleOfFade = false; });
        }

        public void FadeImage()
        {
            isInFadedState = true;
            isInMiddleOfFade = true;

            LeanTween.value(gameObject, skeletonAnim.skeleton.A, fadedAlpha, fadeInDuration).setOnUpdate((float val) =>
            {
                skeletonAnim.skeleton.A = val;
            }).setOnComplete(() => { isInMiddleOfFade = false; });
        }
    }
}
