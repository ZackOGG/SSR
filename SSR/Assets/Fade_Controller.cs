using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SS.Systems
{
    public class Fade_Controller : MonoBehaviour
    {
        [Tooltip("How long between starting to fade in and finishing fading out.")]
        [SerializeField] float standardFadeTime = 1f;
        [Tooltip("How fast to fade in or out")]
        [SerializeField] float defaultFadeSpeed = 1;

        private Image panel;
        private bool fadingIn = false;
        private float alpha = 0;
        private Color currentColour = Color.black;
        private float alphaChange;
        private float fadeSpeed;
        private float fadeTime;

        private void Start()
        {
            panel = this.GetComponent<Image>();
            currentColour.a = 0;
            if(defaultFadeSpeed > (standardFadeTime / 2))
            {
                Debug.LogError("Fade Speed is greater than the Standard Fade Time halfed. This will start fading out before fading in has finished.");
            }
        }

        private void Update()
        {            
            if(Input.GetKeyUp(KeyCode.T))
            {
                StartFade(2f, 0.05f);

            }
            if (fadingIn && panel.color.a < 1)
            {                
                alphaChange = Time.deltaTime / defaultFadeSpeed;
                currentColour.a += alphaChange;
                panel.color = currentColour;   
            }
            if(!fadingIn && panel.color.a > 0)
            {
                alphaChange = Time.deltaTime / defaultFadeSpeed;
                currentColour.a -= alphaChange;
                panel.color = currentColour;
            }
        }

        public void StartDefaultFade()
        {
            print("Fading it");
            panel.enabled = true;
            StopCoroutine("DefaultFade");
            StartCoroutine("DefaultFade");
        }


        IEnumerator DefaultFade()
        {            
            panel.enabled = true;
            fadingIn = true;
            yield return new WaitForSeconds(standardFadeTime - defaultFadeSpeed); // TODO FIGURE THIS OUT
            fadingIn = false;
            yield return new WaitForSeconds(defaultFadeSpeed);
            panel.enabled = false;
        }

        public void StartFade(float newFadeTime, float newFadeSpeed)
        {
            defaultFadeSpeed = newFadeSpeed;
            fadeTime = newFadeTime;
            StartCoroutine("Fade");

        }
        IEnumerator Fade()
        {            
            panel.enabled = true;
            fadingIn = true;
            yield return new WaitForSeconds(fadeTime - defaultFadeSpeed); // TODO FIGURE THIS OUT
            fadingIn = false;
            yield return new WaitForSeconds(defaultFadeSpeed);
            panel.enabled = false;
        }
       
    }
}