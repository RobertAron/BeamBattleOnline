using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSwappingController : MonoBehaviour
{
    [SerializeField] CanvasGroup pauseCanvas = default;
    int lean1;
    [SerializeField] CanvasGroup mainCanvas = default;
    int lean2;

    List<CanvasGroup> canvases = new List<CanvasGroup>();
    List<int>  ids = new List<int>();

    private void Start() {
        pauseCanvas.gameObject.SetActive(false);
        mainCanvas.gameObject.SetActive(true);
        mainCanvas.alpha = 1;
        canvases.Add(pauseCanvas);
        canvases.Add(mainCanvas);
        ids.Add(1);
        ids.Add(2);
    }

    void SetCanvasEnabled(bool enabled,int leanId){
        LeanTween.cancel(ids[leanId]);
        canvases[leanId].gameObject.SetActive(true);
        ids[leanId] = LeanTween.alphaCanvas(canvases[leanId], enabled?1:0 , .03f).setOnComplete(() =>
        {
            canvases[leanId].gameObject.SetActive(enabled);
        }).id;
    }


    
    public void SwapToPause()
    {
        SetCanvasEnabled(true,0);
        SetCanvasEnabled(false,1);
    }
    public void SwapToMain()
    {
        SetCanvasEnabled(false,0);
        SetCanvasEnabled(true,1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) SwapToPause();
    }
}
