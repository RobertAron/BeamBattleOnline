using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PauseMenuHighlightHover : MonoBehaviour, IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField] UnityEvent onClickEvent = default;
    [SerializeField] CanvasGroup canvasGroup = default;
    bool isIn;
    int ltID;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
        LeanTween.cancel(ltID);
        ltID = LeanTween.alphaCanvas(canvasGroup, 1, .1f).setEaseInOutQuad().id;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit();
    }

    void PointerExit(){
        isIn = false;
        LeanTween.cancel(ltID);
        ltID = LeanTween.alphaCanvas(canvasGroup, 0, .1f).setEaseInOutQuad().id;
    }

    private void Update()
    {
        if (!isIn) return;
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0)){
            PointerExit();
            onClickEvent.Invoke();
        }
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
    }
}