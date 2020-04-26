using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillFeedText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh = default;
    [SerializeField] CanvasGroup canvasGroup = default;
    [SerializeField] RectTransform rectTransform = default;

    void Start(){
        LeanTween.alphaCanvas(canvasGroup,0,.2f).setDelay(1f).setOnComplete(()=>{
            rectTransform.LeanSize(new Vector2(rectTransform.rect.width,0),.1f).setOnComplete(()=>{
                Destroy(gameObject);
            });
        });
    }

    public void SetActors(string slayer,string victim){
        textMesh.text = $"{slayer} has defeated {victim}";
    }
}
