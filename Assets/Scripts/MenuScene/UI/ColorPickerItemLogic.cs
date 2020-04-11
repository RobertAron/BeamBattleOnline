using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColorPickerItemLogic :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    bool isIn = false;
    [SerializeField] TooltipActions tooltip = default;
    [SerializeField] RawImage image = default;
    MenuBikeColorSelector menuBikeColorSelector = default;


    private void Start() {
        menuBikeColorSelector = MenuBikeColorSelector.instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isIn = true;
        tooltip.MakeVisible();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isIn = false;
        tooltip.MakeHidden();
    }

    private void Update() {
        if(!isIn) return;
        if(Input.GetKeyDown(KeyCode.Z)) menuBikeColorSelector.SetAccentColor(image.color);
    }

    public void Init(Color color){
        image.color = color;
    }
    
}
