using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KillFeedText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMesh = default;
    [SerializeField] CanvasGroup canvasGroup = default;
    void Start(){
        StartCoroutine(DestroySelf());
    }

    public void SetActors(string slayer,string victim){
        textMesh.text = $"{slayer} has defeated {victim}";
    }

    IEnumerator DestroySelf(){
        yield return new WaitForSeconds(1);
        float timeTransparent= .5f;
        float currentTimeTransparent = 0;
        while(currentTimeTransparent<timeTransparent){
            currentTimeTransparent+=Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1,0,currentTimeTransparent/timeTransparent);
            yield return null;
        }
        Destroy(gameObject);
    }
}
