using Spine;
using System;
using UnityEngine;

namespace Assets.Scripts.Utility
{
    internal class ColorTransitioner : MonoBehaviour
    {
        private float colorTransitionTime, colorTransitionDuration;

        private Color fromColor, toColor;

        private Action<Color> setColorCallback;
        private Action transitionCompleteCallback;

        private bool isTransitioning;

        private void Update()
        {
            if (isTransitioning)
                TransitionColor();
        }

        public void Transition(Color startingColor, Color targetColor, float duration, Action<Color> setColorCallback, Action transitionCompleteCallback)
        {
            this.setColorCallback = setColorCallback;
            this.fromColor = startingColor;
            this.toColor = targetColor;
            this.colorTransitionDuration = duration;
            this.transitionCompleteCallback = transitionCompleteCallback;
            isTransitioning = true;
        }

        private void TransitionColor()
        {
            colorTransitionTime += Time.deltaTime;

            Color lerpedColor = Color.Lerp(fromColor, toColor, colorTransitionTime / colorTransitionDuration);

            setColorCallback(lerpedColor);

            if (colorTransitionTime >= colorTransitionDuration)
                Reset();
        }

        public void Reset()
        {
            transitionCompleteCallback?.Invoke();
            isTransitioning = false;
            setColorCallback = null;
            colorTransitionTime = 0f;

            transitionCompleteCallback = null;
        }
    }
}