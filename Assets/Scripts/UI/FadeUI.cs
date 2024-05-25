using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isFaded = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fader()
    {
        isFaded = !isFaded;

        if (isFaded)
        {
            canvasGroup.DOFade(1, 2);
        }
        else
        {
            canvasGroup.DOFade(0, 2);
        }

    }
}
