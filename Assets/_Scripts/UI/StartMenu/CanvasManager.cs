using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Owner: Spencer Martin
/// CanvasManager manages switching between UI canvases (i.e.: moving from a main menu to a sub menu)
/// It relies on the CanvasGroup component, holds an array of canvas gameobjects in _UICanvasGroups and you can then use the functions ShowCanvas(index),
/// HideCanvas(index) and HideAll() to switch them around as needed
/// </summary>
namespace ReplicantPackage
{
    public class CanvasManager : MonoBehaviour
    {
        public CanvasGroup[] _UICanvasGroups;
        public float fadeDuration = 0.25f;
        public float waitBetweenFadeTime = 0.1f;

        [Space]
        public CanvasGroup _FadeCanvasGroup;

        private int currentGroup;
        private int lastGroup;


        void Awake() //Hide all canvases stored in _UICanvasGroups list so that we don't start the scene with everything showing
        {
            HideAll();
        }

        public void HideAll() //Loops thru to hide all canvases
        {
            currentGroup = 0;
            lastGroup = 0;

            for (int i = 0; i < _UICanvasGroups.Length; i++)
            {
                //making other canvases invisible but also so that they will not interfere with other UI
                _UICanvasGroups[i].alpha = 0;
                _UICanvasGroups[i].interactable = false;
                _UICanvasGroups[i].blocksRaycasts = false;
            }
        }

        public void HideCanvas(int indexNum) //Hides an individual canvas
        {
            StartCoroutine(FadeCanvasOut(_UICanvasGroups[indexNum], fadeDuration));
        }

        public void HideCanvas(int indexNum, bool doFade) //Alternative way to hide an individual canvas, can pass bool for whether canvas should fade or disappear instantly
        {
            if (doFade)
            {
                StartCoroutine(FadeCanvasOut(_UICanvasGroups[indexNum], fadeDuration));
            }
            else
            {
                _UICanvasGroups[indexNum].alpha = 0;
                _UICanvasGroups[indexNum].interactable = false;
                _UICanvasGroups[indexNum].blocksRaycasts = false;
            }
        }

        public void ShowCanvas(int indexNum) //Shows an indivdual canvas
        {
            lastGroup = currentGroup;
            currentGroup = indexNum;

            StartCoroutine(FadeCanvasIn(_UICanvasGroups[indexNum], fadeDuration));
        }

        public void ShowCanvas(int indexNum, bool doFade) //Alternative way to show an individual canvas, can pass bool to specify fade mode
        {
            lastGroup = currentGroup;
            currentGroup = indexNum;

            if (doFade)
            {
                StartCoroutine(FadeCanvasIn(_UICanvasGroups[indexNum], fadeDuration));
            }
            else
            {
                _UICanvasGroups[indexNum].alpha = 1;
                _UICanvasGroups[indexNum].interactable = true;
                _UICanvasGroups[indexNum].blocksRaycasts = true;
            }
        }

        public void LastCanvas() //Method to get back to previous screen easily
        {
            TimedSwitchCanvas(currentGroup, lastGroup);
        }

        public void FadeOut(float timeVal) //Uses FadeCanvasIn coroutine to fade out specific canvas
        {
            StartCoroutine(FadeCanvasIn(_FadeCanvasGroup, timeVal));
        }

        public void FadeIn(float timeVal) //Uses FadeCanvasOut coroutine to fade in specific canvas
        {
            StartCoroutine(FadeCanvasOut(_FadeCanvasGroup, timeVal));
        }

        //Snap fade canvas on or off as needed
        public void FaderOn()
        {
            _FadeCanvasGroup.alpha = 1;
        }

        public void FaderOff()
        {
            _FadeCanvasGroup.alpha = 0;
        }

        public void TimedSwitchCanvas(int indexFrom, int indexTo) //Uses StartFadeCanvasSwitch coroutine
        {
            lastGroup = indexFrom;
            currentGroup = indexTo;

            StartCoroutine(StartFadeCanvasSwitch(_UICanvasGroups[indexFrom], _UICanvasGroups[indexTo], fadeDuration, waitBetweenFadeTime));
        }

        #region Coroutines

        public static IEnumerator FadeCanvasIn(CanvasGroup theGroup, float fadeDuration)
        {
            float currentTime = 0; //tracks timing during the fade
            float currentAlpha = theGroup.alpha; //never assume what alpha level the canvas might be set at, so get value from the canvas itself

            theGroup.interactable = true; // makes canvas we are fading in interactable, could move to end to lock this interaction 
            theGroup.blocksRaycasts = true;
            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime; // tracks how long coroutine has been running
                float newAlpha = Mathf.Lerp(currentAlpha, 1, currentTime / fadeDuration);
                theGroup.alpha = newAlpha;
                yield return null;
            }
        }

        public static IEnumerator FadeCanvasOut(CanvasGroup theGroup, float fadeDuration)
        {
            float currentTime = 0;
            float currentAlpha = theGroup.alpha;

            theGroup.interactable = false;
            theGroup.blocksRaycasts = false;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(currentAlpha, 0, currentTime / fadeDuration);
                theGroup.alpha = newAlpha;
                yield return null;
            }
        }

        public static IEnumerator StartFadeCanvasSwitch(CanvasGroup startGroup, CanvasGroup endGroup, float fadeDuration, float waitTime) // Combines both FadeCanvasIn & FadeCanvasOut to make things neater
        {
            // fade out old UI
            float currentTime = 0;
            float currentAlpha = startGroup.alpha;

            startGroup.interactable = false;
            startGroup.blocksRaycasts = false;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(currentAlpha, 0, currentTime / fadeDuration);
                startGroup.alpha = newAlpha;
                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            // now fade in new UI
            currentTime = 0;
            currentAlpha = endGroup.alpha;

            endGroup.interactable = true;
            endGroup.blocksRaycasts = true;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(currentAlpha, 1, currentTime / fadeDuration);
                endGroup.alpha = newAlpha;
                yield return null;
            }
        }
        #endregion
    }
}


