using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerItemLogic :
    MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    bool isIn = false;
    [SerializeField] TooltipActions tooltip = default;
    [SerializeField] RawImage image = default;
    [SerializeField] MenuBikeColorSelector menuBikeColorSelector = default;
    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
        tooltip.Enable();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
        tooltip.Disable();
    }

    private void Update() {
        if(!isIn) return;
        if(Input.GetKeyDown(KeyCode.Z)) menuBikeColorSelector.SetPrimaryColor(image.color);
        if(Input.GetKeyDown(KeyCode.X)) menuBikeColorSelector.SetAccentColor(image.color);
    }

    public void Init(Color color){
        image.color = color;
    }
}
