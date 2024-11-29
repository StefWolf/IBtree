using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    private CanvasGroup canvas;
    private bool fadeIn = false;
    private bool fadeOut = false;

    private void Start()
    {
        canvas = gameObject.GetComponent<CanvasGroup>();
    }

    public void FadeOut()
    {
        fadeOut = true;
    }

    private void Update()
    {
        if (fadeOut)
        {
            if (canvas.alpha > 0f)
            {
                canvas.alpha -= Time.deltaTime;
            }
            else
            {
                fadeOut = false;
            }
        }
        if (fadeIn)
        {
            if(canvas.alpha < 1f)
            {
                canvas.alpha += Time.deltaTime;
            }
            else
            {
                fadeIn = false;
            }
        }
    }
    public void FadeIn()
    {
        fadeIn = true;
    }

    public void FadeInForFadeOut(float time)
    {
        FadeIn();
        Invoke("FadeOut", time);
    }
}
