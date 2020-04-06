using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooltipActions : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup = default;
    [SerializeField] LeanTweenType leanTweenType = default;
    int ltID;
    public void Enable(){
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
        ltID = LeanTween.alphaCanvas(canvasGroup,1,.1f).setEase(leanTweenType).id;

    }
    public void Disable(){
        LeanTween.cancel(ltID);
        gameObject.SetActive(false);
    }
}
