using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeIn)
        {
            if(canvasGroup.alpha < 1)
            {
                canvasGroup.alpha += Time.deltaTime;
                if(canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }

        }

        if (fadeOut)
        {
            if (canvasGroup.alpha >= 1)
            {
                canvasGroup.alpha -= Time.deltaTime;
                if (canvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }

        }
    }

    public void ShowUI()
    {
        fadeIn = true;

    }

    public void HideUI()
    {
        fadeOut = true;
    }
}
