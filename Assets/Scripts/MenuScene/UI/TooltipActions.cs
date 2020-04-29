using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipActions : MonoBehaviour
{
    [SerializeField] LeanTweenType leanTweenType = default;


    [SerializeField] CanvasGroup canvasGroup = default;
    [SerializeField] Image image = default;
    
    int ltID;
    int ltColorID;
    [SerializeField] Color targetColor;

    private void Awake() {
        targetColor = PlayerPrefsController.defaultAccentColor;
    }

    public void MakeVisible(){
        LeanTween.cancel(ltID);
        LeanTween.cancel(ltColorID);
        UpdateTTColor(targetColor);
        ltID = LeanTween.alphaCanvas(canvasGroup,1,.05f).setEase(leanTweenType).id;
    }

    public void MakeHidden(){
        LeanTween.cancel(ltID);
        ltID = LeanTween.alphaCanvas(canvasGroup,0,.05f).setEase(leanTweenType).id;

    }

    public void LerpToColor(Color color){
        targetColor = color;
        LeanTween.cancel(ltColorID);
        ltColorID = LeanTween.value(this.gameObject,UpdateTTColor,image.color,color,.05f).setEase(leanTweenType).id;
    }

    void UpdateTTColor(Color color){
        image.color = color;
    }
}
